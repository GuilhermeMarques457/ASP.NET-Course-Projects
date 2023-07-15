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

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(AddCssActionFilter))]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly IStockService _stockService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStockService stockService, IConfiguration configuration, ILogger<TradeController> logger)
        {
            _stockService = stockService;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _logger = logger;   
        }

        [Route("[action]")]
        [TypeFilter(typeof(AddDefaultStockSymbolActionFilter))]
        public async Task<IActionResult> Index(string? stockToTrade)
        {
            _logger.LogInformation("Index action reached");

            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stockToTrade);
            Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stockToTrade);


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

            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(Order);

            return RedirectToAction("SeeOrders");

        }

        [HttpPost]
        [TypeFilter(typeof(ValidateOrderActionFilter))]
        [Route("[action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest Order)
        {

            _logger.LogInformation("Client Selled stock");

            Order.DateAndTimeOfOrder = DateTime.Now;

            SellOrderResponse sellOrderResponse = await _stockService.CreateSellOrder(Order);

            return RedirectToAction("SeeOrders");



        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> SeeOrders()
        {
            _logger.LogInformation("Client wants to see orders");

            List<BuyOrderResponse> buyOrderList = await _stockService.GetBuyOrders();
            List<SellOrderResponse> sellOrderList = await _stockService.GetSellOrders();
            BuySellOrdersVM ordersList = new BuySellOrdersVM()
            {
                buyOrders = buyOrderList,
                sellOrders = sellOrderList
            };

            return View(ordersList);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Explore(string? searchStock, string? clickedStock)
        {

            _logger.LogInformation("Client wants to explore stocks");

            List<StockTrade> stockTrades = new List<StockTrade>();

            _tradingOptions.StockSymbols = _configuration.GetSection("TradingOptions:Top25PopularStocks")!.Value;

            List<string> stocksList = _tradingOptions.StockSymbols!.Split(",").ToList();
            List<string>? stocksSearched;

            if(searchStock != null && clickedStock != null)
            {
                ViewBag.CurrentSearch = searchStock;

                stocksSearched = stocksList.Where(s => s.Contains(searchStock)).ToList();

                foreach (var stock in stocksSearched)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };

                    stockTrades.Add(stockTrade);
                }

                ViewBag.ClickedStock = stockTrades.Where(s => s.StockSymbol == clickedStock).FirstOrDefault();
            }
            else if(searchStock != null)
            {
                stocksSearched = stocksList.Where(s => s.Contains(searchStock)).ToList();

                foreach (var stock in stocksSearched)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };

                    stockTrades.Add(stockTrade);
                }

                ViewBag.CurrentSearch = searchStock;

            }
            else if(clickedStock != null)
            {
                ViewBag.clickedStock = clickedStock;

                stockTrades.Clear();

                Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(clickedStock);
                Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(clickedStock);

                StockTrade stockTrade = new StockTrade()
                {
                    StockSymbol = clickedStock,
                    StockName = responseCompany!["name"].ToString(),
                    Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                    Image = responseCompany["logo"].ToString(),
                    Exchange = responseCompany["exchange"].ToString(),
                    Industry = responseCompany["finnhubIndustry"].ToString()

                };

                ViewBag.ClickedStock = stockTrade;

                stockTrades.Add(stockTrade);
            }
            else
            {
                foreach (var stock in stocksList)
                {
                    Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(stock);
                    Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(stock);

                    StockTrade stockTrade = new StockTrade()
                    {
                        StockSymbol = stock,
                        StockName = responseCompany!["name"].ToString(),
                        Price = Convert.ToDouble(responseDictionary!["c"].ToString()),
                        Image = responseCompany["logo"].ToString(),
                        Exchange = responseCompany["exchange"].ToString(),
                        Industry = responseCompany["finnhubIndustry"].ToString()

                    };
                    stockTrades.Add(stockTrade);
                }
            }
           

            return View(stockTrades);

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> TradePDF()
        {
            List<BuyOrderResponse> buyOrderList = await _stockService.GetBuyOrders();
            List<SellOrderResponse> sellOrderList = await _stockService.GetSellOrders();
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
