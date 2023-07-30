using Entities;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;

namespace Service
{
    public class StockServiceSellCreater : IStockServiceSellCreater
    {
        private readonly IStockRepository _stockRepository;


        public StockServiceSellCreater(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.Price == 0 || sellOrderRequest.Price >= 1000000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.Quantity == 0 || sellOrderRequest.Quantity >= 100000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.StockSymbol == null)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }
            if (sellOrderRequest.DateAndTimeOfOrder!.Value.Year <= 2000)
            {
                throw new ArgumentException(nameof(sellOrderRequest));
            }

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            await _stockRepository.SellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }


    }
}
