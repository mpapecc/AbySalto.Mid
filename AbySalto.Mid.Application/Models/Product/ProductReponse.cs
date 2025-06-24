using AbySalto.Mid.Domain.Entites;

namespace AbySalto.Mid.Application.Models.Product
{
    public class ProductsResponse
    {
        public IList<Product> Products { get; set; }
    }

    public class Product
    {
        public Id Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
