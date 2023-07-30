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
using ContactsManager.Core.DTO.PersonDTO;

namespace Services.PeopleService
{
    public class PersonAdderService : IPersonAdderService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PersonAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPeopleRepository peopleRepository, ILogger<PersonAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            switch (personAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation made by me
            ValidationHelper.ModelValidation(personAddRequest);

            if (personAddRequest.ReceiveNewsLetters == null)
            {
                personAddRequest.ReceiveNewsLetters = false;
            }

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            await _peopleRepository.AddPerson(person);

            //Insert Procedure
            //_context.sp_InsertPerson(person);

            return person.ToPersonResponse();
        }


    }
}
