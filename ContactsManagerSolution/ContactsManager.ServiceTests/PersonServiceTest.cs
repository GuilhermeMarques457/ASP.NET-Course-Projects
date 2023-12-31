﻿using AutoFixture;
using ContactsManager.Core.DTO.PersonDTO;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog;
using ServiceContracts.Enums;
using ServiceContracts.IPersonService;
using Services.PeopleService;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonAdderService _peopleAdderService;
        private readonly IPersonGetterService _peopleGetterService;
        private readonly IPersonUpdaterService _peopleUpdaterService;
        private readonly IPersonDeleterService _peopleDeleterService;
        private readonly IPersonSorterService _peopleSorterService;

        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;
        private readonly IPeopleRepository _peopleRepository;

        //Object to write the tests in the console
        private readonly ITestOutputHelper _testOutputHelper;

        //Object to create fake objects
        private readonly IFixture _fixture;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;

            //Mocking the repository through repository injection
            _peopleRepositoryMock = new Mock<IPeopleRepository>();
            //We're getting the repository implementation from the mock object
            _peopleRepository = _peopleRepositoryMock.Object;
            var diagnosticContextMock = new Mock<IDiagnosticContext>();

            var loggerMockAdd = new Mock<ILogger<PersonAdderService>>();
            var loggerMockGet = new Mock<ILogger<PersonGetterService>>();
            var loggerMockDel = new Mock<ILogger<PersonDeleterService>>();
            var loggerMockUpd = new Mock<ILogger<PersonUpdaterService>>();
            var loggerMockSort = new Mock<ILogger<PersonSorterService>>();


            //Mocking the dbContext is no longer required
            //DbContextMock<AspNetDbContext> dbContextMock = new DbContextMock<AspNetDbContext>(
            //    new DbContextOptionsBuilder<AspNetDbContext>().Options
            //);

            //var dbContext = dbContextMock.Object;

            //dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            //dbContextMock.CreateDbSetMock(temp => temp.Persons, peopleInitialData);

            _peopleGetterService = new PersonGetterService(_peopleRepository, loggerMockGet.Object, diagnosticContextMock.Object);
            _peopleAdderService = new PersonAdderService(_peopleRepository, loggerMockAdd.Object, diagnosticContextMock.Object);
            _peopleDeleterService = new PersonDeleterService(_peopleRepository, loggerMockDel.Object, diagnosticContextMock.Object);
            _peopleUpdaterService = new PersonUpdaterService(_peopleRepository, loggerMockUpd.Object, diagnosticContextMock.Object);
            _peopleSorterService = new PersonSorterService(_peopleRepository, loggerMockSort.Object, diagnosticContextMock.Object);

            
        }

        #region AddPerson

        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            PersonAddRequest? addRequest = null;

            Func<Task> action = (async () =>
            {
               await _peopleAdderService.AddPerson(addRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task AddPerson_NullNamePerson_ToBeArgumentException()
        {
            PersonAddRequest? addRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            Person person = addRequest.ToPerson();

            _peopleRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            Func<Task> action = (async () =>
            {
                await _peopleAdderService.AddPerson(addRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            //Create will just create the properties based on the type,
            //Validation like email doesn't matter
            //PersonAddRequest? addRequest = _fixture.Create<PersonAddRequest>();

            //Build override the default values with the method "with"
            PersonAddRequest? addRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            Person person = addRequest.ToPerson();
            PersonResponse personResponseExpected = person.ToPersonResponse();

            //If we supply any argument value to the AddPerson method
            //It should return the same value
            _peopleRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            PersonResponse personResponseAdd = await _peopleAdderService.AddPerson(addRequest);
            personResponseExpected.PersonId = personResponseAdd.PersonId;
           

            personResponseAdd.PersonId.Should().NotBeEmpty();
            personResponseAdd.Should().BeEquivalentTo(personResponseExpected);

        }

        #endregion

        #region GetPersonById
        [Fact]
        public async Task GetPersonById_NullPersonId_ToBeNull()
        {
            Guid? guid = null;

            PersonResponse? personResponse = await _peopleGetterService.GetPersonById(guid);

            //Assert.Null(personResponse);
            personResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonById_ValidIdPerson_ToBeSuccessful()
        {
            //CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>().Create();

            //CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse personResponseExpected = person.ToPersonResponse();

            _peopleRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse? personResponseById = await _peopleGetterService.GetPersonById(person.PersonID);

            personResponseById.Should().BeEquivalentTo(personResponseExpected);

            //Assert.Equal(personResponse, personResponseById);
        }
        #endregion

        #region GetAllPeople

        [Fact]
        public async Task GetPersonList_EmptyList_ToBeEmpty()
        {
            _peopleRepositoryMock.Setup(temp => temp.GetPersonList())
                .ReturnsAsync(new List<Person>());

            List<PersonResponse> personResponses = await _peopleGetterService.GetPersonList();

            personResponses.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPersonList_WithFewPeople_ToBeSuccessful()
        {
            //Arrange

            Person personRequest1 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest2 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest3 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<Person> personRequests = new List<Person>()
            {
                personRequest1,
                personRequest2,
                personRequest3
            };

            List<PersonResponse> personResponseListExpected = personRequests
                .Select(temp => temp.ToPersonResponse()).ToList();

            _peopleRepositoryMock.Setup(temp => temp.GetPersonList())
                .ReturnsAsync(personRequests);

            //Act
            List<PersonResponse> personsListFromGet = await _peopleGetterService.GetPersonList();


            personsListFromGet.Should().BeEquivalentTo(personResponseListExpected);

        }
        #endregion

        #region GetFilteredPeople
        [Fact]
        public async Task GetFilteredPeople_EmptySearch_ToBeSuccessful()
        {
            //Arrange

            Person personRequest1 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest2 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest3 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<Person> personRequests = new List<Person>()
            {
                personRequest1,
                personRequest2,
                personRequest3
            };

            List<PersonResponse> personResponseListExpected = personRequests
                .Select(temp => temp.ToPersonResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in personResponseListExpected)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            _peopleRepositoryMock.Setup(temp => temp.GetFilterdPeople(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(personRequests);

            //Act
            List<PersonResponse>? personsListFromSearch = await _peopleGetterService.GetFilterdPeople(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in personsListFromSearch!)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    persons_list_from_search.Should().Contain(person_response_from_add);
            //}

            personsListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
        }

        //First we add people and then we search a string
        //And then it should return based on that string
        [Fact]
        public async Task GetFilteredPeople_SearchByPersonName_ToBeSucessful()
        {
            Person personRequest1 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest2 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest3 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<Person> personRequests = new List<Person>()
            {
                personRequest1,
                personRequest2,
                personRequest3
            };

            List<PersonResponse> personResponseListExpected = personRequests
                .Select(temp => temp.ToPersonResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            //print person_response_list
            foreach (PersonResponse person_response_list in personResponseListExpected)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            _peopleRepositoryMock.Setup(temp => temp.GetFilterdPeople(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(personRequests);

            //Act
            List<PersonResponse>? personsListFromSearch = await _peopleGetterService.GetFilterdPeople(nameof(Person.PersonName), "so");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_list in personsListFromSearch!)
            {
                _testOutputHelper.WriteLine(person_response_list.ToString());
            }

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    persons_list_from_search.Should().Contain(person_response_from_add);
            //}

            personsListFromSearch.Should().BeEquivalentTo(personResponseListExpected);
        }
        #endregion

        #region GetSortedPeople
        [Fact]
        public async Task GetSortedPeople_ToBeSucessful()
        {

            //Arrange
            Person personRequest1 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest2 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            Person personRequest3 = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            List<Person> personRequests = new List<Person>()
            {
                personRequest1,
                personRequest2,
                personRequest3
            };

            List<PersonResponse> personResponseListExpected = personRequests
                .Select(temp => temp.ToPersonResponse()).ToList();

            _peopleRepositoryMock.Setup(temp => temp.GetPersonList())
                .ReturnsAsync(personRequests);

            //Act

            List<PersonResponse> allPeople = await _peopleGetterService.GetPersonList();

            List<PersonResponse>? personsListFromSort = _peopleSorterService
                .GetSortedPeople(allPeople, nameof(Person.PersonName), SortOrderOptions.DESC);
          

            //Assert
            //for (int i = 0; i <person_response_list_from_add.Count; i++)
            //{
            //    person_response_list_from_add[i].Should().Be(persons_list_from_sort[i]);
            //}

            personsListFromSort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            Func<Task> action = (async() =>
            {
                await _peopleUpdaterService.UpdatePerson(personUpdateRequest);
            });

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ArgumentException()
        {
            PersonUpdateRequest? personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.PersonId, Guid.NewGuid)
                .Create();

            Func<Task> action = (async() =>
             {
                 await _peopleUpdaterService.UpdatePerson(personUpdateRequest);
             });

             await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameNull_ToBeArgumentException()
        {

            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.PersonGender, "Male")
                .Create();

            PersonResponse personResponse = person.ToPersonResponse();
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            Func<Task> action = (async () =>
            {
                await _peopleUpdaterService.UpdatePerson(personUpdateRequest);
            });

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task UpdatePerson_ProperDetails_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.PersonGender, "Male")
                .Create();

            PersonResponse personResponseExpected = person.ToPersonResponse();
            PersonUpdateRequest personUpdateRequest = personResponseExpected.ToPersonUpdateRequest();

            _peopleRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            _peopleRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            PersonResponse personResponseFromUpdate = await _peopleUpdaterService.UpdatePerson(personUpdateRequest);

            personResponseFromUpdate.Should().Be(personResponseExpected);

        }

        #endregion

        #region DeletePerson
  
        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {
            Person? person = _fixture.Build<Person>()
                .With(temp => temp.PersonEmail, "someone@gmal.com")
                .With(temp => temp.PersonID, Guid.NewGuid)
                .With(temp => temp.Country, null as Country)
                .Create();


            _peopleRepositoryMock.Setup(temp => temp.DeletePerson(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _peopleRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);


            bool isDeleted = await _peopleDeleterService.DeletePerson(person.PersonID);

            isDeleted.Should().BeTrue();

            //Assert.True(isDeleted);
        }

        //If supplied id doesn't existis, it returns false
        [Fact]
        public async Task DeletePerson_InValidPersonID_ToBeFailed()
        {
            bool isDeleted = await _peopleDeleterService.DeletePerson(Guid.NewGuid());

            isDeleted.Should().BeFalse();
            //Assert.False(isDeleted);
        }

        #endregion
    }
}
