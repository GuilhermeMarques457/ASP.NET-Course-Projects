using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;

namespace Service
{
    public class StockServiceBuyGetter : IStockServiceBuyGetter
    {
        private readonly IStockRepository _stockRepository;


        public StockServiceBuyGetter(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stockRepository.GetBuyOrders();
            return buyOrders.ToList().Select(o => o.ToBuyOrderResponse()).ToList();
        }

    }
}
