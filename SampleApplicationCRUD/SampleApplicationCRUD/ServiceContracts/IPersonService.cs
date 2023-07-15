using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        Task<List<PersonResponse>> GetPersonList();

        Task<PersonResponse?> GetPersonById(Guid? PersonId);

     
        /// <summary>
        /// Returns all people objects that matches eith given search field and search string
        /// </summary>
        /// <param name="searchBy">SearchBy is the property that I want to search,</param>
        /// <param name="searchString">SearchString is the value that I want to search</param>
        /// <returns></returns>
        Task<List<PersonResponse>?> GetFilterdPeople(string? searchBy, string? searchString);

        /// <summary>
        /// Returns a list of people based in the sortby
        /// and the option that is supplied (asc, desc)
        /// </summary>
        /// <param name="allPeople">it's the list that will be sorted</param>
        /// <param name="sortBy">it's the proporty that will be sorted</param>
        /// <param name="options">it's the order, descending or ascending</param>
        /// <returns>A sorted list as PersonResponse</returns>
        List<PersonResponse>? GetSortedPeople(List<PersonResponse>? allPeople, string sortBy, SortOrderOptions options);

        /// <summary>
        /// Updates the specified person
        /// </summary>
        /// <param name="personUpdateRequest">Person details with Id to update</param>
        /// <returns>Returns person response object to update</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person based on given Id
        /// </summary>
        /// <param name="PersonId"></param>
        /// <returns>true if the delete is successfull
        /// otherwise false</returns>
        Task<bool> DeletePerson(Guid? PersonId);

        /// <summary>
        /// Method to deal with the people information
        /// </summary>
        /// <returns>Returns the people information as CVS</returns>
        Task<MemoryStream> GetPeopleCVS();

        /// <summary>
        /// Method to take peple details and transform it in a excel table
        /// </summary>
        /// <returns>Return the people information as an Excel table</returns>
        Task<MemoryStream> GetPeopleExcel();

        
    }
}
