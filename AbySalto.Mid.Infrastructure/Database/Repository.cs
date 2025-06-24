using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Domain.Entites;

namespace AbySalto.Mid.Infrastructure.Database
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AbySaltoDb context;

        public Repository(AbySaltoDb context)
        {
            this.context = context;
        }

        public virtual IQueryable<T> GetById(int id)
        {
            return this.context.Set<T>().Where(x => x.Id == id);
        }

        public virtual void Insert(T entity)
        {
            this.context.Set<T>().Add(entity);
        }

        public virtual IQueryable<T> Query()
        {
            return this.context.Set<T>();
        }

        public virtual bool Commit()
        {
            return context.SaveChanges() > 0;
        }
    }
}
