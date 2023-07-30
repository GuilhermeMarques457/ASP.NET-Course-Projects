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
    public class PersonUpdaterService : IPersonUpdaterService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PersonUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdaterService(IPeopleRepository peopleRepository, ILogger<PersonUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            //BECAREFULL WITH THIS
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(PersonUpdateRequest));

            if (personUpdateRequest.PersonName == null)
                throw new ArgumentException(nameof(PersonUpdateRequest));

            ValidationHelper.ModelValidation(personUpdateRequest);

            if (personUpdateRequest.ReceiveNewsLetters == null)
            {
                personUpdateRequest.ReceiveNewsLetters = false;
            }

            Person? matchingPerson = await _peopleRepository.GetPersonById(personUpdateRequest.PersonId);

            if (matchingPerson == null) throw new InvalidPersonIdException("Given Id doesn't match with existing person");

            //Only for this object the savechanges will affect the data in the database

            Person updatedPerson = await _peopleRepository.UpdatePerson(personUpdateRequest.ToPerson());

            return updatedPerson.ToPersonResponse();
        }

    }
}
