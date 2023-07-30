using CsvHelper;
using Entities;
using ServiceContracts.Enums;
using Services.Helpers;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;
using ServiceContracts.IPersonService;

namespace Services.PeopleService
{
    public class PersonDeleterService : IPersonDeleterService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PersonDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleterService(IPeopleRepository peopleRepository, ILogger<PersonDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }



        public async Task<bool> DeletePerson(Guid? PersonId)
        {
            if (PersonId == null)
            {
                throw new ArgumentNullException(nameof(PersonId));
            }

            Person? person = await _peopleRepository.GetPersonById(PersonId);

            if (person == null)
            {
                return false;
            }

            return await _peopleRepository.DeletePerson(person.PersonID);

        }
    }
}
