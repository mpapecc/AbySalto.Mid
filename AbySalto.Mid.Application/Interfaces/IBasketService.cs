using AbySalto.Mid.Application.Models.Basket;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IBasketService
    {
        Task<IEnumerable<ProductWithCount>> GetBasket();
        Task AddToBasket(int id);
        Task RemoveFromBasket(int id);
    }
}