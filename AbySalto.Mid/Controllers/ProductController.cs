using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Application.ModelBinders;
using AbySalto.Mid.Application.Models;
using AbySalto.Mid.Application.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AbySalto.Mid.Application.Interfaces;

namespace AbySalto.Mid.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet(nameof(GetAllProducts))]
        public async Task<ProductsResponse?> GetAllProducts()
        {
            return await this.productService.GetAllProducts();
        }

        [HttpGet(nameof(GetProduct))]
        public async Task<Product?> GetProduct([Id] Id id)
        {
            return await this.productService.GetProduct(id.RawId);
        }

        [Authorize]
        [HttpGet(nameof(GetFavourites))]
        public async Task<IEnumerable<Product>?> GetFavourites()
        {
            return await this.productService.GetFavourites();
        }

        [Authorize]
        [HttpPost(nameof(AddToFavourites))]
        public void AddToFavourites([Id] Id id)
        {
            this.productService.AddToFavourite(id.RawId);
        }

        [Authorize]
        [HttpPost(nameof(RemoveFromFavourite))]
        public void RemoveFromFavourite([Id] Id id)
        {
            this.productService.RemoveFromFavourite(id.RawId);
        }
    }
}
