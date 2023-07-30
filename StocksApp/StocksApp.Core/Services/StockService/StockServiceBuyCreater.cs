using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;

namespace Service
{
    public class StockServiceBuyCreater : IStockServiceBuyCreater
    {
        private readonly IStockRepository _stockRepository;


        public StockServiceBuyCreater(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
            {
                throw new ArgumentNullException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.Price == 0 || buyOrderRequest.Price >= 1000000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.Quantity == 0 || buyOrderRequest.Quantity >= 100000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.StockSymbol == null)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }
            if (buyOrderRequest.DateAndTimeOfOrder!.Value.Year <= 2000)
            {
                throw new ArgumentException(nameof(buyOrderRequest));
            }

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            buyOrder.BuyOrderID = Guid.NewGuid();

            await _stockRepository.BuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

    }
}
