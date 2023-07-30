using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;

namespace Service
{
    public class StockServiceSellGetter : IStockServiceSellGetter
    {
        private readonly IStockRepository _stockRepository;


        public StockServiceSellGetter(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stockRepository.GetSellOrders();
            return sellOrders.ToList().Select(o => o.ToSellOrderResponse()).ToList();
        }


    }
}
