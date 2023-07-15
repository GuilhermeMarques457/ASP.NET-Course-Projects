using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SampleApplicationCRUD.Filters.ResultFilters;

namespace SampleApplicationCRUD.Filters.ResourceFilters
{
    public class DisableActionMethodResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<DisableActionMethodResourceFilter> _logger;
        private readonly bool _isDisabled;

        public DisableActionMethodResourceFilter(ILogger<DisableActionMethodResourceFilter> logger, bool isDisabled = true)
        {
            _logger = logger;
            _isDisabled = isDisabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("OnResourceExecuting: {FilterName}.{MethodName}", nameof(DisableActionMethodResourceFilter), nameof(OnResourceExecutionAsync));

            if(_isDisabled)
            {
                //context.Result = new NotFoundResult();
                context.Result = new StatusCodeResult(501);
            }
            else
            {
                await next();
            }

            _logger.LogInformation("OnResourceExecuted: {FilterName}.{MethodName}", nameof(DisableActionMethodResourceFilter), nameof(OnResourceExecutionAsync));
        }
    }
}
