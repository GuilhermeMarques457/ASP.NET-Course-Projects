using ContactsManager.Core.DTO.PersonDTO;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.IPersonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PeopleService
{
    public class PersonGetterServiceChild : PersonGetterService
    {

        public PersonGetterServiceChild(IPeopleRepository personRepository, ILogger<PersonGetterService> logger, IDiagnosticContext diagnosticContext) : base(personRepository, logger, diagnosticContext)
        {
            
        }

        public async override Task<MemoryStream> GetPeopleExcel()
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
    }
}
