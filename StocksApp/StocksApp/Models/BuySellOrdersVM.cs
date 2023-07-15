using ServiceContracts.DTO;

namespace StocksApp.Models
{
    public class BuySellOrdersVM
    {
        public List<BuyOrderResponse> buyOrders = new List<BuyOrderResponse>();
        public List<SellOrderResponse> sellOrders = new List<SellOrderResponse>();
    }
}
