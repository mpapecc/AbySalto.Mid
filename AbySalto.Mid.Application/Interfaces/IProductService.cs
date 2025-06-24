using AbySalto.Mid.Application.Models.Product;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductsResponse?> GetAllProducts();
        Task<Product?> GetProduct(int id);
        Task<IEnumerable<Product>> GetProductsByIdList(IEnumerable<int> ids);
        Task<IEnumerable<Product>?> GetFavourites();
        void AddToFavourite(int id);
        void RemoveFromFavourite(int id);
    }
}