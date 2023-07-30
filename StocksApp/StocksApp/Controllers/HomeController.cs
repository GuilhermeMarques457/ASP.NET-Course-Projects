using Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using ServiceContracts;
using StocksApp.Filters.ActionFilter;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(AddCssActionFilter))]
    public class HomeController : Controller
    {
        private readonly IFinnhubServiceStockGetter _finnhubServiceStockGetter;
        private readonly IFinnhubServiceCompanyProfileGetter _finnhubServiceCompanyProfileGetter;
        private readonly IFinnhubServiceStockPriceGetter _finnhubServiceStockPriceGetter;
        private readonly TradingOptions _tradingOptions;

        public HomeController(IFinnhubServiceStockGetter finnhubServiceStockGetter,
            IFinnhubServiceCompanyProfileGetter finnhubServiceCompanyProfileGetter,
            IFinnhubServiceStockPriceGetter finnhubServiceStockPriceGetter,
            IOptions<TradingOptions> tradingOptions)
        {
            _finnhubServiceStockGetter = finnhubServiceStockGetter;
            _finnhubServiceCompanyProfileGetter = finnhubServiceCompanyProfileGetter;
            _finnhubServiceStockPriceGetter = finnhubServiceStockPriceGetter;

            _tradingOptions = tradingOptions.Value;
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if(exception != null && exception.Error != null)
            {
                ViewBag.ErrorMessage = exception.Error.Message;
            }
            
            return View();
        }
    }
}
