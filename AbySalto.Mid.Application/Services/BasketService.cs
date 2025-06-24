using AbySalto.Mid.Application.Extensions;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Application.Models.Basket;
using AbySalto.Mid.Domain.Entites;
using Microsoft.Extensions.Caching.Memory;

namespace AbySalto.Mid.Application.Services
{
    public class BasketService: IBasketService
    {
        private readonly IRepository<Basket> basketRepository;
        private readonly IUserIdentity userIdentity;
        private readonly IProductService productService;
        private readonly IMemoryCache cache;

        public BasketService(
            IRepository<Basket> basketRepository,
            IUserIdentity userIdentity,
            IProductService productService,
            IMemoryCache cache)
        {
            this.basketRepository = basketRepository;
            this.userIdentity = userIdentity;
            this.productService = productService;
            this.cache = cache;
        }

        public async Task<IEnumerable<ProductWithCount>> GetBasket()
        {
            var productIds = this.basketRepository.Query()
                .Where(x => x.UserId == this.userIdentity.Id.RawId)
                .Select(x => x.ProductIds)
                .FirstOrDefault()?
                .IdsStringToList();

            if (productIds == null || !productIds.Any())
                return Enumerable.Empty<ProductWithCount>();

            var productCounts = productIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var products = await this.productService.GetProductsByIdList(productCounts.Keys.Distinct());

            return products.Select(p => new ProductWithCount
            {
                Product = p,
                Count = productCounts.TryGetValue(p.Id.RawId, out var count) ? count : 0
            });
        }

        public async Task AddToBasket(int id)
        {
            var product = await this.productService.GetProduct(id);

            if(product == null)
                throw new Exception("Product is not available");

            if (product.Stock == 0)
                throw new Exception("Product is out of stock");

            product.Stock--;

            this.cache.Set($"product-{product.Id.RawId}", product);

            var basket = GetOrCreateBasket(this.userIdentity.Id.RawId);

            var productIdsList = basket.ProductIds.IdsStringToList();
            productIdsList.Add(product.Id.RawId);
            basket.ProductIds = string.Join(',', productIdsList);

            basketRepository.Commit();
        }

        private Basket GetOrCreateBasket(int userId)
        {
            var basket = this.basketRepository.Query()
                .FirstOrDefault(x => x.UserId == userId); 

            if (basket != null)
                return basket;

            basket = new Basket
            {
                UserId = userId,
                ProductIds = string.Empty
            };

            this.basketRepository.Insert(basket);
            return basket;
        }

        public async Task RemoveFromBasket(int id)
        {
            var product = await this.productService.GetProduct(id);

            if (product == null)
                throw new Exception("Product is not available");

            if (product.Stock == 0)
                throw new Exception("Product is out of stock");

            product.Stock++;

            this.cache.Set($"product-{product.Id.RawId}", product);

            var basket = this.basketRepository.Query()
                .Where(x => x.UserId == this.userIdentity.Id.RawId)
                .FirstOrDefault();

            if (basket == null)
            {
                throw new Exception("Can not remove item from non existing basket");
            }
            else
            {
                var productIdsList = basket.ProductIds.IdsStringToList();
                productIdsList.Remove(product.Id.RawId);
                basket.ProductIds = string.Join(',', productIdsList);
            }

            basketRepository.Commit();
        }
    }
}
