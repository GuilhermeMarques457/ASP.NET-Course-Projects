using Entities;
using ServiceContracts;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoFixture;
using RepositoryContracts;
using Xunit.Abstractions;
using FluentAssertions;
using Services.CountriesService;
using ContactsManager.Core.DTO.CountryDTO;

namespace Tests
{
    //I will let this tests with default assertions, not with fluent assertions

    public class CountriesServiceTest
    {
        private readonly ICountriesUploaderService _countriesUploaderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesAdderService _countriesAdderService;

        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public CountriesServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;

            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            _countriesAdderService = new CountriesAdderService(_countriesRepository);
            _countriesGetterService = new CountriesGetterService(_countriesRepository);
            _countriesUploaderService = new CountriesUploaderService(_countriesRepository);


        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullExeption
        [Fact]
        public async Task AddCountry_NullCountry_ArgumentNullException()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesAdderService.AddCountry(request);
            });
            
        }

        //When CountryName is null, it should throw ArgumentNullExeption
        [Fact]
        public async Task AddCountry_NullNameCountry_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesAdderService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate, it should throw ArgumentExeption
        [Fact]
        public async Task AddCountry_DuplicateCountryName_ToBeArgumentException()
        {
            //Arrange
            CountryAddRequest? request1 = _fixture.Build<CountryAddRequest>().Create();

            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, request1.CountryName)
                .Create();

            Country country = request1.ToCountry();

            _countriesRepositoryMock.Setup(temp => temp.GetCountryByName(It.IsAny<string>()))
                .ReturnsAsync(country);

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesAdderService.AddCountry(request1);
                await _countriesAdderService.AddCountry(request2);
            });
        }

        //When supplied country is correct, it should insert the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
                .Create();

            Country countryActual = request.ToCountry();
            CountryResponse countryResponseExpected = countryActual.ToCountryResponse();

            //Act
            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(countryActual);

            CountryResponse countryResponseActual = await _countriesAdderService.AddCountry(request);
            countryResponseExpected.CountryId = countryResponseActual.CountryId;

            //Assert
            countryResponseActual.CountryId.Should().NotBeEmpty();
            countryResponseActual.Should().BeEquivalentTo(countryResponseExpected);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public async Task GetAllCountries_EmptyList_ToBeEmpty()
        {

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(new List<Country>());

            //Act
            List<CountryResponse> actualCountryResponseList =
                await _countriesGetterService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponseList);
        }

        [Fact]
        public async Task GetAllCountries_WithFewCountries_ToBeSucessful()
        {
            //Arrange
            Country? request1 = _fixture.Build<Country>()
                .With(temp => temp.People, null as List<Person>)
                .Create();
            Country? request2 = _fixture.Build<Country>()
                .With(temp =>temp.People, null as List<Person>)
                .Create();

            List<Country> countries = new List<Country> 
            { 
                request1,
                request2 
            };


            List<CountryResponse> countryResponseListExpected = countries.Select(temp => temp.ToCountryResponse()).ToList();

            //Act

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            List<CountryResponse> actualCountryResponseList = await _countriesGetterService.GetAllCountries();

            actualCountryResponseList.Should().BeEquivalentTo(countryResponseListExpected);

            //Assert
           
        }

        #endregion

        #region GetCountryByID

        [Fact]
        public async Task GetCountryById_NullCountryID_ToBeFailed()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? countryResponse = await _countriesGetterService.GetCountryById(countryId);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task GetCountryById_ValidCountryId_ToBeSuccessful()
        {
            //Arrange
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>()
               .Create();

            Country countryActual = request.ToCountry();
            CountryResponse countryResponseExpected = countryActual.ToCountryResponse();

            //Act
            _countriesRepositoryMock.Setup(temp => temp.GetCountryById(It.IsAny<Guid>()))
                .ReturnsAsync(countryActual);

            CountryResponse? countryResponseActual = await _countriesGetterService.GetCountryById(countryResponseExpected.CountryId);

            //Assert
            countryResponseActual.Should().BeEquivalentTo(countryResponseExpected);
        }

        #endregion
    }
}
