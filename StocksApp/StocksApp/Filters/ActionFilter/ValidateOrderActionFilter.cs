using Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StocksApp.Filters.ActionFilter
{

    public class ValidateOrderActionFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            TradeController controller = (TradeController)context.Controller;

            if (!controller.ModelState.IsValid)
            {
                controller.ViewBag.Errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList();

                controller.TempData["Errors"] = controller.ViewBag.Errors;


                if (context.ActionArguments["Order"] is BuyOrderRequest order &&
                        typeof(BuyOrderRequest).IsAssignableFrom(order.GetType()))
                {
                    BuyOrderRequest orderRequest = (BuyOrderRequest)context.ActionArguments["Order"];

                    context.Result = controller.RedirectToAction("Index", "Trade", new { stockToTrade = orderRequest!.StockSymbol });

                }
                else
                {
                    SellOrderRequest orderRequest = (SellOrderRequest)context.ActionArguments["Order"];

                    context.Result = controller.RedirectToAction("Index", "Trade", new { stockToTrade = orderRequest!.StockSymbol });
                }

                
            }
            else
            {
                await next();
            }
       

           
        }
    }
}
