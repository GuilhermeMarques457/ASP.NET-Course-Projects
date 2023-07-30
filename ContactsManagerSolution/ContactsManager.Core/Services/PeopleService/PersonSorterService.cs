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
    public class PersonSorterService : IPersonSorterService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PersonSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonSorterService(IPeopleRepository peopleRepository, ILogger<PersonSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public List<PersonResponse>? GetSortedPeople(List<PersonResponse>? allPeople, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPeople of personService reached");

            if (string.IsNullOrEmpty(sortBy))
                return allPeople;

            List<PersonResponse>? sortedPeople = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonEmail), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonEmail), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.PersonEmail, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonGender), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.PersonGender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonGender), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.PersonGender).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.DateOfBirth!.Value.ToString("dd MMMM yyyy"), StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonAge), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.PersonAge).ToList(),

                (nameof(PersonResponse.PersonAge), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.PersonAge).ToList(),

                (nameof(PersonResponse.PersonAddress), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonAddress), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.PersonAddress, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                    => allPeople?.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                => allPeople?.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                   => allPeople?.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPeople
            };

            return sortedPeople;
        }

    }
}
