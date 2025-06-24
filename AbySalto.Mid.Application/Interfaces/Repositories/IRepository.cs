namespace AbySalto.Mid.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        IQueryable<T> GetById(int id);
        void Insert(T entity);
        bool Commit();
    }
}
