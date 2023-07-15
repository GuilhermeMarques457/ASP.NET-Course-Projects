using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts;
using StocksApp.Filters.ActionFilter;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;

        public HomeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            ViewBag.css = "home.css";

            if (_tradingOptions.DefaultStockSymbol == null)
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }
            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);
            Dictionary<string, object>? responseCompany = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);


            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
                CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
                HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
                LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
                OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
            };

            return View(stock);
        }
    }
}
