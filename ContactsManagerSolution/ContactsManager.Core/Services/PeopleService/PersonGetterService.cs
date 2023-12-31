﻿using CsvHelper;
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
    public class PersonGetterService : IPersonGetterService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ILogger<PersonGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonGetterService(IPeopleRepository peopleRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            _peopleRepository = peopleRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public virtual async Task<PersonResponse?> GetPersonById(Guid? PersonId)
        {
            if (PersonId == null) return null;

            Person? person = await _peopleRepository.GetPersonById(PersonId.Value);

            if (person == null) return null;

            return person.ToPersonResponse();


        }

        public virtual async Task<List<PersonResponse>> GetPersonList()
        {
            _logger.LogInformation("GetPersonList of personService reached");

            //To get informations about the foreign object based in the PK
            var personList = await _peopleRepository.GetPersonList();

            //Get through LINQ
            return personList.Select(temp => temp.ToPersonResponse()).ToList();

            //Get through Procedure
            //return _context.sp_GetAllPeople().Select(p => p.ToPersonResponse()).ToList();
        }

        public virtual async Task<List<PersonResponse>?> GetFilterdPeople(string? searchBy, string? searchString)
        {
            _logger.LogInformation("GetFilteredPeople of personService reached");

            List<Person>? people = new List<Person>();

            if (searchString == null)
            {
                people = await _peopleRepository.GetPersonList();
            }
            else
            {
                using (Operation.Time("Time for Filtered People from DB"))
                {
                    people = searchBy switch
                    {
                        nameof(PersonResponse.PersonName) =>
                            await _peopleRepository.GetFilterdPeople(temp =>
                                temp.PersonName!.Contains(searchString)),

                        nameof(PersonResponse.PersonEmail) =>
                            await _peopleRepository.GetFilterdPeople(temp =>
                                temp.PersonEmail!.Contains(searchString)),

                        nameof(PersonResponse.DateOfBirth) =>
                                await _peopleRepository.GetFilterdPeople(temp =>
                                    temp.DateOfBirth!.Value.ToString("dd MMMM yyyy").Contains(searchString)),

                        nameof(PersonResponse.PersonGender) =>
                            await _peopleRepository.GetFilterdPeople(temp =>
                                temp.PersonGender!.Contains(searchString)),

                        nameof(PersonResponse.CountryID) =>
                                await _peopleRepository.GetFilterdPeople(temp =>
                                    temp.Country!.CountryName!.Contains(searchString)),

                        nameof(PersonResponse.PersonAddress) =>
                                await _peopleRepository.GetFilterdPeople(temp =>
                                    temp.PersonAddress!.Contains(searchString)),

                        _ => await _peopleRepository.GetPersonList()
                    };
                }
            }

            _diagnosticContext.Set("People", people);

            return people.Select(temp => temp.ToPersonResponse()).ToList();
        }


        public virtual async Task<MemoryStream> GetPeopleCVS()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            //PersonName, Email, ...
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.PersonEmail));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.PersonAge));
            csvWriter.WriteField(nameof(PersonResponse.PersonGender));
            csvWriter.WriteField(nameof(PersonResponse.CountryName));
            csvWriter.WriteField(nameof(PersonResponse.PersonAddress));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            csvWriter.NextRecord();

            //List<Person> people = await _peopleRepository.GetPersonList();

            //List<PersonResponse> peopleReponses = people.Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> peopleResponses = await GetPersonList();

            foreach (var person in peopleResponses)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.PersonEmail);
                if (person.DateOfBirth.HasValue)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("dd-MM-yyyy"));
                }
                else
                {
                    csvWriter.WriteField("");
                }
                csvWriter.WriteField(person.PersonAge);
                csvWriter.WriteField(person.PersonGender);
                csvWriter.WriteField(person.CountryName);
                csvWriter.WriteField(person.PersonAddress);
                csvWriter.WriteField(person.ReceiveNewsLetters);

                csvWriter.NextRecord();
                csvWriter.Flush();
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        public virtual async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PeopleSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Person Email";
                worksheet.Cells["C1"].Value = "Date of Birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive news Letters";

                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                };

                int row = 2;

                List<PersonResponse> peopleResponses = await GetPersonList();

                foreach (PersonResponse person in peopleResponses)
                {
                    worksheet.Cells["A" + row].Value = person.PersonName;
                    worksheet.Cells["B" + row].Value = person.PersonEmail;
                    if (person.DateOfBirth.HasValue)
                    {
                        worksheet.Cells["C" + row].Value = person.DateOfBirth.Value.ToString("dd-MM-yyyy");
                    };
                    worksheet.Cells["D" + row].Value = person.PersonAge;
                    worksheet.Cells["E" + row].Value = person.PersonGender;
                    worksheet.Cells["F" + row].Value = person.CountryName;
                    worksheet.Cells["G" + row].Value = person.PersonAddress;
                    worksheet.Cells["H" + row].Value = person.ReceiveNewsLetters;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
