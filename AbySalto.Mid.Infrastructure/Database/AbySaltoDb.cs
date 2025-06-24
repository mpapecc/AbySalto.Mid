using AbySalto.Mid.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Database
{
    public class AbySaltoDb: DbContext
    {
        public AbySaltoDb(DbContextOptions<AbySaltoDb> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbySaltoDb).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FavouriteProducts> FavouriteProducts { get; set; }
        public DbSet<Basket> Baskets { get; set; }
    }
}
