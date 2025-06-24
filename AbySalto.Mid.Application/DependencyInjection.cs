using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AbySalto.Mid.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
