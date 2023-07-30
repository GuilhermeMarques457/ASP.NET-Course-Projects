using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApplicationCRUD.Filters.ResultFilters
{
    public class PersonsAlwaysRunResultFilter : IAsyncAlwaysRunResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //Everthing can be done to this always filter

            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }

            await next();
        }
    }
}
