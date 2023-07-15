using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Rotativa.AspNetCore.Options;
using System.IO;
using CRUDExample.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ActionFilters;
using SampleApplicationCRUD.Filters.ResultFilters;
using SampleApplicationCRUD.Filters.ResourceFilters;
using SampleApplicationCRUD.Filters.AuthorizationFilters;
using SampleApplicationCRUD.Filters.ExceptionFilters;
using SampleApplicationCRUD.Filters;

namespace SampleApplicationCRUD.Controllers
{
    //If I Apply this filter, it will be used for all action methods
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments =
        new object[] { 
            "X-MyCustomKeyPersonController",
            "MyCustomValuePersonController",
            3 }
        , Order = 3)]
    // Filter to log exception
    //[TypeFilter(typeof(HandleExceptionFilter))]
    [TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonService personService, ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personService = personService;
            _countriesService = countriesService;
            _logger = logger;   
            
        }
        
        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsIndexActionFilter), Order = 4)]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments =
            new object[] {
                "X-MyCustomKeyIndex",
                "MyCustomValueIndex",
                1 }
            , Order = 1)]
        [ResponseHeaderFilterFactory("X-MyCustomKeyIndex-Factory", "MyCustomValueIndexFactory", 1)]
        [ServiceFilter(typeof(PersonsListResultFilter))]
        //[SkipFilter]
        public async Task<IActionResult> Index(
            string? searchBy,
            string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC
        )
        {
            _logger.LogInformation("Index action of personsControler reached");
            _logger.LogDebug("searchBy: " + searchBy + ", searchString: " + searchString + ", sortOrder: " + sortOrder + ", sortyBy: " + sortBy);


            List<PersonResponse>? persons = new List<PersonResponse>();
            persons = await _personService.GetFilterdPeople(searchBy, searchString);
            
            //Sorting
            List<PersonResponse>? sortedPersons = _personService.GetSortedPeople(persons, sortBy, sortOrder);

            return View(sortedPersons);
        }

        [Route("[action]")]
        [HttpGet]
        [ResponseHeaderFilterFactory("X-MyCustomKeyIndex", "MyCustomValueIndex", 1)]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(
                temp => new SelectListItem() 
                { 
                    Text = temp.CountryName, Value = temp.CountryId.ToString() 
                });


            return View(new PersonAddRequest());
        }

        [HttpPost]
        [Route("[action]")]
        [TypeFilter(typeof(PersonsPostAndPutActionFilter))]
        // I'm calling the filter but i'm saying isDisabled is equal to false so it does not shot circuiting
        [TypeFilter(typeof(DisableActionMethodResourceFilter), Arguments = 
            new object[]
            {
                false
            })]
        public async Task<IActionResult> Create(PersonAddRequest person)
        {
            PersonResponse personRespose = await _personService.AddPerson(person);

            return RedirectToAction("Index", "Persons");
        }

        [Route("[action]/{id:guid}")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid? id)
        {
            PersonResponse? personResponse = await _personService.GetPersonById(id);

            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(
                temp => new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryId.ToString()
                });


            if (personUpdateRequest == null) 
            {
                return RedirectToAction("index");
            }

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{id:guid}")]
        [TypeFilter(typeof(PersonsPostAndPutActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {
            PersonResponse? matchingPerson = await _personService.GetPersonById(person.PersonId);

            if(matchingPerson == null )
            {
                return RedirectToAction("index");
            }

            PersonResponse personRespose = await _personService.UpdatePerson(person);

            return RedirectToAction("index", "persons");
        }

        [Route("[action]/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            PersonResponse? personResponse = await _personService.GetPersonById(id);

            if (personResponse == null)
            {
                return RedirectToAction("index");
            }

            return View(personResponse);
        }

        [Route("[action]/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonUpdateRequest? person)
        {
            PersonResponse? matchingPerson = await _personService.GetPersonById(person?.PersonId);

            if (matchingPerson == null)
            {
                return RedirectToAction("index");
            }

            bool deleted = await _personService.DeletePerson(person?.PersonId);

            if(deleted) 
            {
                return RedirectToAction("index", "persons");
            }

            return View();

        }

        [Route("[action]")]
        public async Task<IActionResult> PeoplePDF()
        {
            List<PersonResponse> people = await _personService.GetPersonList();

            return new ViewAsPdf("PeoplePDF", people, ViewData)
            {
                PageMargins = new Margins()
                {
                    Top = 20,
                    Left = 20,
                    Right = 20,
                    Bottom = 20,
                },
                PageOrientation = Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleCSV()
        {
            MemoryStream? memoryStream = await _personService.GetPeopleCVS();
            return File(memoryStream, "application/octet-stream", "people.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PeopleExcel()
        {
            MemoryStream? memoryStream = await _personService.GetPeopleExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "people.xlsx");
        }
    }
}
