using ContactsManager.Core.DTO.PersonDTO;
using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.IPersonService
{
    public interface IPersonGetterService
    {

        Task<List<PersonResponse>> GetPersonList();

        Task<PersonResponse?> GetPersonById(Guid? PersonId);


        /// <summary>
        /// Returns all people objects that matches eith given search field and search string
        /// </summary>
        /// <param name="searchBy">SearchBy is the property that I want to search,</param>
        /// <param name="searchString">SearchString is the value that I want to search</param>
        /// <returns></returns>
        Task<List<PersonResponse>?> GetFilterdPeople(string? searchBy, string? searchString);

        Task<MemoryStream> GetPeopleCVS();

        /// <summary>
        /// Method to take peple details and transform it in a excel table
        /// </summary>
        /// <returns>Return the people information as an Excel table</returns>
        Task<MemoryStream> GetPeopleExcel();


    }
}
