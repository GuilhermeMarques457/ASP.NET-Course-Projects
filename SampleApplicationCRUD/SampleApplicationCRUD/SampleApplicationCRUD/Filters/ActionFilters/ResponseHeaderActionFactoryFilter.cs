using Microsoft.AspNetCore.Mvc.Filters;
using OfficeOpenXml.Style;

namespace SampleApplicationCRUD.Filters.ActionFilters
{
    // This class is responsable to create and involke the filter object
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private string Key { get; set; }
        private string Value { get; set; }
        private int Order { get; set; }

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<ResponseHeaderActionFactoryFilter>();
            filter!.key = Key;
            filter!.value = Value;
            filter!.Order = Order;

            return filter;
        }
    }

    public class ResponseHeaderActionFactoryFilter : IAsyncActionFilter, IOrderedFilter
    {
        
        public string key;
        public string value;
        public int Order { get; set; }
        private readonly ILogger<ResponseHeaderActionFactoryFilter> _logger;
        

        public ResponseHeaderActionFactoryFilter(ILogger<ResponseHeaderActionFactoryFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Reached the filter factory before");
            context.HttpContext.Response.Headers[key] = value;
            //Before Execution
            await next(); //Use this method to call the next filter
            //After Execution

            _logger.LogInformation("Reached the filter factory after");
        }
    }
}
