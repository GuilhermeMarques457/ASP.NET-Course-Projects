using Entities;

namespace RepositoryContracts
{
    public interface IStockRepository
    {
        Task<BuyOrder> BuyOrder(BuyOrder buyOrder);
        Task<SellOrder> SellOrder(SellOrder sellOrder);
        Task<List<SellOrder>> GetSellOrders();
        Task<List<BuyOrder>> GetBuyOrders();
    }
}