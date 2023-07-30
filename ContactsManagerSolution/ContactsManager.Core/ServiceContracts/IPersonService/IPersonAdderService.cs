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
    public interface IPersonAdderService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);




    }
}
