using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApplicationCRUD.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("OnResultExecuting: {FilterName}.{MethodName}", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("dd-MM-dd HH:mm");

            await next();

            _logger.LogInformation("OnResultExecuted: {FilterName}.{MethodName}", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));

            
        }
    }
}
