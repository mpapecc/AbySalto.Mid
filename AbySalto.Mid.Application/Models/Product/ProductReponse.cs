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
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
    }

    public class ProductDetails: Product
    {
        public int Stock { get; set; }
        public int MinimumOrderQuantity { get; set; }
    }
}
