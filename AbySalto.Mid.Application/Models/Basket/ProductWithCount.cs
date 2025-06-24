using ProductModel = AbySalto.Mid.Application.Models.Product.Product;

namespace AbySalto.Mid.Application.Models.Basket
{
    public class ProductWithCount
    {
        public ProductModel Product { get; set; }
        public int Count { get; set; }
    }
}
