using Microsoft.AspNetCore.Mvc.Filters;
using StocksApp.Controllers;

namespace StocksApp.Filters.ActionFilter
{
    public class AddDefaultStockSymbolActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            TradeController controller = (TradeController)context.Controller;

            if(context.ActionArguments.TryGetValue("stockToTrade", out var stockToTrade))
            {
               
            }
            else
            {
                context.ActionArguments["stockToTrade"] = "MSFT";
            }
            

            await next();
        }
    }
}
