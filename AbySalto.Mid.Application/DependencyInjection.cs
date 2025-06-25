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
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped(typeof(IGridProcessor<>), typeof(GridProcessor<>));
            return services;
        }
    }
}
