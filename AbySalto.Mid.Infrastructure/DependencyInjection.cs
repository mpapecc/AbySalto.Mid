using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Domain.Entites;
using AbySalto.Mid.Infrastructure.Auth;
using AbySalto.Mid.Infrastructure.ConfigurationOptions;
using AbySalto.Mid.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AbySalto.Mid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DummyJsonOptions>(configuration.GetSection(nameof(DummyJsonOptions)));
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserIdentity,UserIdentity>(x =>
            {
                var httpContext = x.GetRequiredService<IHttpContextAccessor>();
                var cache = x.GetRequiredService<IMemoryCache>();
                var userRepository = x.GetRequiredService<IRepository<User>>();
                var userIdClaim = httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId");

                return userIdClaim != null ? new UserIdentity(int.Parse(userIdClaim.Value), userRepository, cache) : new UserIdentity();
            });

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
