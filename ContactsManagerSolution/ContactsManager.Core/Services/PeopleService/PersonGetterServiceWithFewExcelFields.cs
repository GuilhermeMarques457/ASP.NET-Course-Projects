using ContactsManager.Core.DTO.PersonDTO;
using OfficeOpenXml;
using ServiceContracts.IPersonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PeopleService
{
    public class PersonGetterServiceWithFewExcelFields : IPersonGetterService
    {
        private readonly PersonGetterService _personGetterService;

        public PersonGetterServiceWithFewExcelFields(PersonGetterService personGetterService)
        {
             _personGetterService = personGetterService;
        }
        public async Task<List<PersonResponse>?> GetFilterdPeople(string? searchBy, string? searchString)
        {
            return await _personGetterService.GetFilterdPeople(searchBy, searchString);
        }

        public async Task<MemoryStream> GetPeopleCVS()
        {
            return await _personGetterService.GetPeopleCVS();
        }

        public async Task<MemoryStream> GetPeopleExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PeopleSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Age";
                worksheet.Cells["C1"].Value = "Gender";
               

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
                    worksheet.Cells["B" + row].Value = person.PersonAge;
                    worksheet.Cells["C" + row].Value = person.PersonGender;
                    

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<PersonResponse?> GetPersonById(Guid? PersonId)
        {
            return await _personGetterService.GetPersonById(PersonId);
        }

        public async Task<List<PersonResponse>> GetPersonList()
        {
            return await _personGetterService.GetPersonList();
        }
    }
}
