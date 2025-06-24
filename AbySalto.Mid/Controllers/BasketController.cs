using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.ModelBinders;
using AbySalto.Mid.Application.Models;
using AbySalto.Mid.Application.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BasketController : Controller
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [Authorize]
        [HttpGet(nameof(GetBasket))]
        public async Task<IEnumerable<ProductWithCount>> GetBasket()
        {
            return await this.basketService.GetBasket();
        }

        [Authorize]
        [HttpPost(nameof(AddToBasket))]
        public async Task AddToBasket([Id] Id productId)
        {
            await this.basketService.AddToBasket(productId.RawId);
        }

        [Authorize]
        [HttpPost(nameof(RemoveFromBasket))]
        public async Task RemoveFromBasket([Id] Id productId)
        {
            await this.basketService.RemoveFromBasket(productId.RawId);

        }
    }
}
