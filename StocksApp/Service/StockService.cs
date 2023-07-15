using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;


        public StockService(IStockRepository stockRepository)
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

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _stockRepository.GetBuyOrders();
            return buyOrders.ToList().Select(o => o.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stockRepository.GetSellOrders();
            return sellOrders.ToList().Select(o => o.ToSellOrderResponse()).ToList();
        }


    }
}
