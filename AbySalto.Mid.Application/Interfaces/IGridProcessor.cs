using AbySalto.Mid.Application.Models.GridProcessors;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IGridProcessor<T> where T : class
    {
        IQueryable<T> ProcessQuery(IQueryable<T> query, GridRquest gridRequest);
        IQueryable<T> ApplyPagination(IQueryable<T> query, GridPagination gridPagination);
        IQueryable<T> ApplySort(IQueryable<T> query, IList<GridSort> gridPagination);
    }
}
