using Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleApplicationCRUD.Controllers;
using ServiceContracts;
using ServiceContracts.DTO;
using System;

namespace SampleApplicationCRUD.Filters.ActionFilters
{
    public class PersonsPostAndPutActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;
        public PersonsPostAndPutActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesService
                        .GetAllCountries();

                    personsController.ViewBag.Countries = countries
                        .Select(temp => new SelectListItem
                        {
                            Text = temp.CountryName,
                            Value = temp.CountryId.ToString()
                        });

                    personsController.ViewBag.Errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList();

                    //Getting the personRequest object of the binded parameter
                    var person = context.ActionArguments["person"];

                    // Short circuiting (stoping the normal flow, i mean prevent the action and next filters to execute, because there are errors)
                    // To do it result must not be null
                    context.Result = personsController.View(person);

                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

            //Don't call the next (short-circuiting)
            //await next();
        }
    }
}
