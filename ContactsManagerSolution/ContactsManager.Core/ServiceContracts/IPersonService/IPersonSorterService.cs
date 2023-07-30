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
    public interface IPersonSorterService
    {
        List<PersonResponse>? GetSortedPeople(List<PersonResponse>? allPeople, string sortBy, SortOrderOptions options);

        /// <summary>
        /// Updates the specified person
        /// </summary>
        /// <param name="personUpdateRequest">Person details with Id to update</param>
        /// <returns>Returns person response object to update</returns>

    }
}
