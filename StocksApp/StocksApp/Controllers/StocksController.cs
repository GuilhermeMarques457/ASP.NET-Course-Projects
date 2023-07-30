using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts;
using StocksApp.Filters.ActionFilter;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    public class StocksController : Controller
    {

        private readonly IFinnhubServiceStockGetter _finnhubServiceStockGetter;
        private readonly IFinnhubServiceCompanyProfileGetter _finnhubServiceCompanyProfileGetter;
        private readonly IFinnhubServiceStockPriceGetter _finnhubServiceStockPriceGetter;
        private readonly TradingOptions _tradingOptions;
        private readonly ILogger<TradeController> _logger;

        public StocksController(IFinnhubServiceStockGetter finnhubServiceStockGetter,
            IFinnhubServiceCompanyProfileGetter finnhubServiceCompanyProfileGetter,
            IFinnhubServiceStockPriceGetter finnhubServiceStockPriceGetter,
            IOptions<TradingOptions> tradingOptions,
            ILogger<TradeController> logger)
        {
            _finnhubServiceStockGetter = finnhubServiceStockGetter;
            _finnhubServiceCompanyProfileGetter = finnhubServiceCompanyProfileGetter;
            _finnhubServiceStockPriceGetter = finnhubServiceStockPriceGetter;

            _tradingOptions = tradingOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        [TypeFilter(typeof(AddCssActionFilter))]
        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Explore(string? searchStock, string? clickedStock)
        {

            _logger.LogInformation("Client wants to explore stocks");

            List<Dictionary<string, string>>? AllStocks = await _finnhubServiceStockGetter.GetStocks();
            List<StockTrade> stockTrades = new List<StockTrade>();

            List<StockVM> stocksToSee = new List<StockVM>();
            List<string> stocksListTop25 = _tradingOptions.Top25PopularStocks!.Split(",").ToList();

            if (stocksListTop25 != null && AllStocks != null)
            {
                AllStocks = AllStocks.Where(temp => stocksListTop25.Contains(Convert.ToString(temp["symbol"]))).ToList();

                stocksToSee = AllStocks.Select(temp => new StockVM()
                {
                    StockName = Convert.ToString(temp["description"]),
                    StockSymbol = Convert.ToString(temp["symbol"])
                }).ToList();
            }
            List<string>? stocksSearched;


            if (searchStock != null && clickedStock != null)
            {
                ViewBag.CurrentSearch = searchStock;

                stocksSearched = stocksListTop25!.Where(s => s.Contains(searchStock)).ToList();

                stocksToSee = AllStocks.Select(temp => new StockVM()
                {
                    StockName = Convert.ToString(temp["description"]),
                    StockSymbol = Convert.ToString(temp["symbol"])
                }).ToList();

                stocksToSee = stocksToSee.Where(s => s.StockSymbol!.Contains(searchStock)).ToList();

                Dictionary<string, object>? responseDictionary = await _finnhubServiceStockPriceGetter.GetStockPriceQuote(clickedStock);
                Dictionary<string, object>? responseCompany = await _finnhubServiceCompanyProfileGetter.GetCompanyProfile(clickedStock);

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
            }
            else if (searchStock != null)
            {
                stocksSearched = stocksListTop25.Where(s => s.Contains(searchStock)).ToList();

                stocksToSee = AllStocks.Select(temp => new StockVM()
                {
                    StockName = Convert.ToString(temp["description"]),
                    StockSymbol = Convert.ToString(temp["symbol"])
                }).ToList();

                stocksToSee = stocksToSee.Where(s => s.StockSymbol!.Contains(searchStock)).ToList();

                ViewBag.CurrentSearch = searchStock;

            }
            else if (clickedStock != null)
            {
                ViewBag.clickedStock = clickedStock;

                stockTrades.Clear();

                Dictionary<string, object>? responseDictionary = await _finnhubServiceStockPriceGetter.GetStockPriceQuote(clickedStock);
                Dictionary<string, object>? responseCompany = await _finnhubServiceCompanyProfileGetter.GetCompanyProfile(clickedStock);

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
            }

            return View(stocksToSee);

        }
    }
}
