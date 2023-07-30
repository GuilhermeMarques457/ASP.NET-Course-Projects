using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.IPersonService
{
    public interface IPersonDeleterService
    {
        Task<bool> DeletePerson(Guid? PersonId);

    }
}
