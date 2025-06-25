using System.Net.Http.Json;
using AbySalto.Mid.Application.Extensions;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Interfaces.Auth;
using AbySalto.Mid.Application.Interfaces.Repositories;
using AbySalto.Mid.Application.Models.Product;
using AbySalto.Mid.Domain.Entites;
using Microsoft.Extensions.Caching.Memory;

namespace AbySalto.Mid.Application.Services
{
    public class ProductService: IProductService
    {
        private readonly IHttpClientFactory factory;
        private readonly IMemoryCache cache;
        private readonly IRepository<FavouriteProducts> favouriteProductsRepository;
        private readonly IUserIdentity userIdentity;

        public ProductService(
            IHttpClientFactory factory, 
            IMemoryCache cache, 
            IRepository<FavouriteProducts> favouriteProductsRepository,
            IUserIdentity userIdentity)
        {
            this.factory = factory;
            this.cache = cache;
            this.favouriteProductsRepository = favouriteProductsRepository;
            this.userIdentity = userIdentity;
        }

        public async Task<ProductsResponse?> GetAllProducts()
        {
            return await GetFromCacheOrServer<ProductsResponse>(key: "all-product", endpoint: "products?limit=0");
        }

        public async Task<ProductDetails?> GetProduct(int id)
        {
            return await GetFromCacheOrServer<ProductDetails>(key: $"product-{id}", endpoint: $"products/{id}");
        }

        public async Task<IEnumerable<Product>> GetProductsByIdList(IEnumerable<int> ids)
        {
            var allProducts = (await GetAllProducts()).Products;

            return allProducts.Where(x => ids.Contains(x.Id.RawId));
        }

        private async Task<T?> GetFromCacheOrServer<T>(string key, string endpoint)
        {
            var entryValueFactory = async (ICacheEntry entry) =>
            {
                using (var dummyJsonHttp = this.factory.CreateClient("DummyJsonClient"))
                {
                    return await dummyJsonHttp.GetFromJsonAsync<T>(endpoint);
                }
            };

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
            };

            return await this.cache.GetOrCreateAsync<T>(key, entryValueFactory, cacheOptions);
        }

        public async Task<IEnumerable<Product>?> GetFavourites()
        {
            var entryValueFactory = async (ICacheEntry entry) =>
            {
                var userId = this.userIdentity.Id.RawId;
                var favouriteProductIds = this.favouriteProductsRepository.Query()
                        .Where(x => x.UserId == userId)
                        .Select(x => x.FavouriteProductIds)
                        .FirstOrDefault();

                var favouriteIds = favouriteProductIds.IdsStringToList();

                return await GetProductsByIdList(favouriteIds);
            };

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = DateTime.Today.GetRestOfDayTimeSpan()
            };

            return await this.cache.GetOrCreateAsync(
                $"favourites-{this.userIdentity.Id.RawId}", entryValueFactory, cacheOptions);
        }

        public void AddToFavourite(int id)
        {
            UpdateFavourite(id, add: true);
        }

        public void RemoveFromFavourite(int id)
        {
            UpdateFavourite(id, add: false);
        }

        private void UpdateFavourite(int id, bool add)
        {
            var userId = this.userIdentity.Id.RawId;
            var userFavourites = this.favouriteProductsRepository.Query()
                .FirstOrDefault(x => x.UserId == userId);

            if (userFavourites == null)
            {
                if (add)
                {
                    this.favouriteProductsRepository.Insert(new FavouriteProducts
                    {
                        UserId = userId,
                        FavouriteProductIds = id.ToString()
                    });
                    this.favouriteProductsRepository.Commit();
                }
                return;
            }

            var favouriteIds = userFavourites.FavouriteProductIds.IdsStringToList();

            var contains = favouriteIds.Contains(id);

            if (add && !contains)
            {
                favouriteIds.Add(id);
            }
            else if (!add && contains)
            {
                favouriteIds.Remove(id);
            }
            else
            {
                return; 
            }

            userFavourites.FavouriteProductIds = string.Join(",", favouriteIds);
            this.favouriteProductsRepository.Commit();
        }
    }
}
