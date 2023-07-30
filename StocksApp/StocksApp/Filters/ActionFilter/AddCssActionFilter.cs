using Microsoft.AspNetCore.Mvc.Filters;
using Service;
using StocksApp.Controllers;

namespace StocksApp.Filters.ActionFilter
{
    public class AddCssActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is TradeController tradeController)
            {
                tradeController.ViewBag.css = "trade.css";
            }

            if (context.Controller is HomeController homeController)
            {
                homeController.ViewBag.css = "home.css";
            }

            if (context.Controller is StocksController stockController)
            {
                stockController.ViewBag.css = "stocks.css";
            }

            await next();
        }
    }
}
