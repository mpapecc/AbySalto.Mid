using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbySalto.Mid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DummyJsonOptions>(configuration.GetSection(nameof(DummyJsonOptions)));
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            return services.AddDatabase(configuration).AddHttpClients().AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AbySaltoDb>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("DummyJsonClient", (serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<DummyJsonOptions>>().Value;
                client.BaseAddress = new Uri(settings.BaseAddress);
            });

            return services;
        }
    }
}
