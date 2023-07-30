using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.Extensions.Logging;
using Services;
using SampleApplicationCRUD.Controllers;
using ServiceContracts.IPersonService;
using Xunit;
using ContactsManager.Core.DTO.CountryDTO;
using ContactsManager.Core.DTO.PersonDTO;

namespace Tests
{
    public class PersonsControllerTest
    {
        private readonly IPersonAdderService _peopleAdderService;
        private readonly IPersonGetterService _peopleGetterService;
        private readonly IPersonUpdaterService _peopleUpdaterService;
        private readonly IPersonDeleterService _peopleDeleterService;
        private readonly IPersonSorterService _peopleSorterService;

        private readonly Mock<IPersonAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonGetterService> _personsGetterServiceMock;
        private readonly Mock<IPersonUpdaterService> _personsUpdaterServiceMock;
        private readonly Mock<IPersonDeleterService> _personsDeleterServiceMock;
        private readonly Mock<IPersonSorterService> _personsSorterServiceMock;


        private readonly ICountriesGetterService _countriesGetterService;        
        private readonly Mock<ICountriesGetterService> _countriesGetterServiceMock;

        private readonly ILogger<PersonsController> _logger;
        private readonly Fixture _fixture;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<PersonsController>>();
            _logger = _loggerMock.Object;

            _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
            _countriesGetterService = _countriesGetterServiceMock.Object;

            _personsAdderServiceMock = new Mock<IPersonAdderService>();
            _personsGetterServiceMock = new Mock<IPersonGetterService>();
            _personsDeleterServiceMock = new Mock<IPersonDeleterService>();
            _personsUpdaterServiceMock = new Mock<IPersonUpdaterService>();
            _personsSorterServiceMock = new Mock<IPersonSorterService>();


            _peopleGetterService = _personsGetterServiceMock.Object;
            _peopleAdderService = _personsAdderServiceMock.Object;
            _peopleDeleterService = _personsDeleterServiceMock.Object;
            _peopleUpdaterService = _personsUpdaterServiceMock.Object;
            _peopleSorterService = _personsSorterServiceMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> personResponsesList =
                _fixture.Create<List<PersonResponse>>();

            PersonsController peopleController = new PersonsController(
                _peopleAdderService,
                _peopleGetterService,
                _peopleDeleterService,
                _peopleSorterService,
                _peopleUpdaterService, 
                _countriesGetterService,
                _logger);

            //Mocking the usings methods in the action
            _personsGetterServiceMock.Setup(temp => temp.GetFilterdPeople(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponsesList);

            _personsSorterServiceMock.Setup(temp => temp.GetSortedPeople
                (It.IsAny<List<PersonResponse>>(),
                 It.IsAny<string>(),
                 It.IsAny<SortOrderOptions>()))
                .Returns(personResponsesList);

            //Act
            //Creating a result based on the parameters of the controller
            IActionResult result = await peopleController.Index(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<SortOrderOptions>()
            );

            //Assert
            //Seeing if the return of the view is type of ViewResult
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            //Acessing the model object passed into the view
            viewResult.ViewData.Model.Should().BeAssignableTo<List<PersonResponse>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponsesList);

        }

        #endregion

        #region AddPerson

        [Fact]
        public async Task AddPerson_IfNoModelErros_ToReturnRedirectToIndexView()
        {
            //Arrange
            PersonAddRequest personAddRequest =
                _fixture.Create<PersonAddRequest>();

            PersonResponse personResponseExpected =
                _fixture.Create<PersonResponse>();

            PersonsController peopleController = new PersonsController(
               _peopleAdderService,
               _peopleGetterService,
               _peopleDeleterService,
               _peopleSorterService,
               _peopleUpdaterService,
               _countriesGetterService,
               _logger);

            _personsAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponseExpected);

            //Act

            IActionResult result =
                await peopleController.Create(personAddRequest);

            PersonResponse personResponseActual = await _peopleAdderService.AddPerson(personAddRequest);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");

            personResponseActual.Should().BeEquivalentTo(personResponseExpected);

        }
        #endregion

        #region EditPerson

        [Fact]
        public async Task EditPerson_ValidGuid_ToReturnEditView()
        {
            //Arrange
            

            PersonResponse personResponse = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonGender, _fixture.Create<GenderOptions>().ToString())
                .Create();

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List <CountryResponse> countries =
                _fixture.Create<List<CountryResponse>>();

            PersonsController peopleController = new PersonsController(
                _peopleAdderService,
                _peopleGetterService,
                _peopleDeleterService,
                _peopleSorterService,
                _peopleUpdaterService,
                _countriesGetterService,
                _logger);

            _countriesGetterServiceMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            _personsGetterServiceMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(personResponse);

            //Act

            List<CountryResponse> countryResponsesActual = await _countriesGetterService.GetAllCountries();
            PersonResponse? personResponseActual = await _peopleGetterService.GetPersonById(personUpdateRequest.PersonId);

            IActionResult result =
                await peopleController.Edit(personUpdateRequest.PersonId);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(personResponseActual?.ToPersonUpdateRequest());

            personResponseActual.Should().BeEquivalentTo(personResponse);
            countryResponsesActual.Should().BeEquivalentTo(countries);

        }

        #endregion
    }
}
