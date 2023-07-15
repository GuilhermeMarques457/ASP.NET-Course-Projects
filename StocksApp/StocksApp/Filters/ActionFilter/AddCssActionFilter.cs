using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Controllers;

namespace StocksApp.Filters.ActionFilter
{
    public class AddCssActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = (TradeController)context.Controller;

            controller.ViewBag.css = "trade.css";

            await next();
        }
    }
}
