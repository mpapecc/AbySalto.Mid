using System.Linq.Expressions;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Models.GridProcessors;

namespace AbySalto.Mid.Application.Services
{
    public class GridProcessor<T> : IGridProcessor<T> where T : class
    {
        public IQueryable<T> ApplyPagination(IQueryable<T> query, GridPagination gridPagination)
        {
            var page = gridPagination.PageNumber <= 0 ? 1 : gridPagination.PageNumber;
            var pageSize = gridPagination.PageSize <= 0 ? 10 : gridPagination.PageSize;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<T> ApplySort(IQueryable<T> source, IList<GridSort> gridSort)
        {
            if (gridSort == null || gridSort.Count == 0)
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            bool isFirst = true;

            foreach (var sort in gridSort)
            {
                var property = typeof(T).GetProperty(sort.PropertyName);
                if (property == null)
                    throw new ArgumentException($"Property '{sort.PropertyName}' not found on type '{typeof(T).Name}'.");

                var propertyAccess = Expression.Property(parameter, property);
                var lambdaType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
                var lambda = Expression.Lambda(lambdaType, propertyAccess, parameter);

                string methodName;
                bool descending = sort.Direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

                if (isFirst)
                {
                    methodName = descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
                }
                else
                {
                    methodName = descending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy);
                }

                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName
                                && m.IsGenericMethodDefinition
                                && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.PropertyType);

                source = (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
                isFirst = false;
            }

            return source;
        }

        public IQueryable<T> ProcessQuery(IQueryable<T> query, GridRquest gridRequest)
        {
            if (gridRequest.Sort is not null)
                query = ApplySort(query, gridRequest.Sort);
            if (gridRequest.Pagination is not null)
                query = ApplyPagination(query, gridRequest.Pagination);

            return query;
        }
    }
}
