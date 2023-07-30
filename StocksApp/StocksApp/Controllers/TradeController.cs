using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts.DTO;
using ServiceContracts;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using StocksApp.Models;
using StocksApp.Filters.ActionFilter;
using ServiceContracts.StockService;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(AddCssActionFilter))]
    public class TradeController : Controller
    {
        private readonly IFinnhubServiceStockGetter _finnhubServiceStockGetter;
        private readonly IFinnhubServiceCompanyProfileGetter _finnhubServiceCompanyProfileGetter;
        private readonly IFinnhubServiceStockPriceGetter _finnhubServiceStockPriceGetter;

        private readonly IStockServiceBuyGetter _stockServiceBuyGetter;
        private readonly IStockServiceSellGetter _stockServiceSellGetter;
        private readonly IStockServiceBuyCreater _stockServiceBuyCreater;
        private readonly IStockServiceSellCreater _stockServiceSellCreater;

        private readonly ILogger<TradeController> _logger;
        private readonly TradingOptions _tradingOptions;

        public TradeController(IFinnhubServiceStockGetter finnhubService,
            IOptions<TradingOptions> tradingOptions,
            IStockServiceBuyGetter stockServiceBuyGetter,
            IStockServiceSellGetter stockServiceSellGetter,
            IStockServiceBuyCreater stockServiceBuyCreater,
            IStockServiceSellCreater stockServiceSellCreater,
            IFinnhubServiceStockGetter finnhubServiceStockGetter,
            IFinnhubServiceCompanyProfileGetter finnhubServiceCompanyProfileGetter,
            IFinnhubServiceStockPriceGetter finnhubServiceStockPriceGetter,
            ILogger<TradeController> logger)
        {
            _stockServiceBuyCreater = stockServiceBuyCreater;
            _stockServiceBuyGetter = stockServiceBuyGetter;
            _stockServiceSellCreater = stockServiceSellCreater;
            _stockServiceSellGetter = stockServiceSellGetter;

            _finnhubServiceStockGetter = finnhubServiceStockGetter;
            _finnhubServiceCompanyProfileGetter = finnhubServiceCompanyProfileGetter;
            _finnhubServiceStockPriceGetter = finnhubServiceStockPriceGetter;

            _tradingOptions = tradingOptions.Value;
            _logger = logger;   
        }

        
        [Route("[action]/{stockToTrade}")]
        [TypeFilter(typeof(AddDefaultStockSymbolActionFilter))]
        public async Task<IActionResult> Index(string? stockToTrade)
        {
            _logger.LogInformation("Index action reached");

            if(TempData["Errors"] != null)
            {
                ViewBag.Errors = TempData["Errors"];
            }

            Dictionary<string, object>? responseDictionary = await _finnhubServiceStockPriceGetter.GetStockPriceQuote(stockToTrade);
            Dictionary<string, object>? responseCompany = await _finnhubServiceCompanyProfileGetter.GetCompanyProfile(stockToTrade);

            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
            };

            ViewBag.Stock = stock;

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = stockToTrade,
                StockName = responseCompany!["name"].ToString(),
                Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                Image = responseCompany["logo"].ToString(),
            };

            ViewBag.StockTrade = stockTrade;

            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateOrderActionFilter))]
        [Route("[action]")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest Order)
        {

            _logger.LogInformation("Client Buyed stock");

            Order.DateAndTimeOfOrder = DateTime.Now;

            BuyOrderResponse buyOrderResponse = await _stockServiceBuyCreater.CreateBuyOrder(Order);

            return RedirectToAction("SeeOrders");

        }

        [HttpPost]
        [TypeFilter(typeof(ValidateOrderActionFilter))]
        [Route("[action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest Order)
        {

            _logger.LogInformation("Client Selled stock");

            Order.DateAndTimeOfOrder = DateTime.Now;

            SellOrderResponse sellOrderResponse = await _stockServiceSellCreater.CreateSellOrder(Order);

            return RedirectToAction("SeeOrders");



        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> SeeOrders()
        {
            _logger.LogInformation("Client wants to see orders");

            List<BuyOrderResponse> buyOrderList = await _stockServiceBuyGetter.GetBuyOrders();
            List<SellOrderResponse> sellOrderList = await _stockServiceSellGetter.GetSellOrders();
            BuySellOrdersVM ordersList = new BuySellOrdersVM()
            {
                buyOrders = buyOrderList,
                sellOrders = sellOrderList
            };

            return View(ordersList);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> TradePDF()
        {
            List<BuyOrderResponse> buyOrderList = await _stockServiceBuyGetter.GetBuyOrders();
            List<SellOrderResponse> sellOrderList = await _stockServiceSellGetter.GetSellOrders();
            List<BuyOrderResponse> buyOrderByDate = buyOrderList.OrderByDescending(o => o.DateAndTimeOfOrder).ToList();
            List<SellOrderResponse> sellOrderByDate = sellOrderList.OrderByDescending(o => o.DateAndTimeOfOrder).ToList();

            BuySellOrdersVM ordersList = new BuySellOrdersVM()
            {
                buyOrders = buyOrderByDate,
                sellOrders = sellOrderByDate
            };

            return new ViewAsPdf("TradePDF", ordersList, ViewData)
            {
                PageMargins = new Margins()
                {
                    Top = 30,
                    Left = 20,
                    Right = 20,
                    Bottom = 30,
                },
                PageOrientation = Orientation.Landscape,

            };

        }
    }
}
