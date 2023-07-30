using ContactsManager.Core.DTO.CountryDTO;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using System.Linq;

namespace Services.CountriesService
{
    public class CountriesUploaderService : ICountriesUploaderService
    {
        private readonly ICountriesRepository _countryRepository;

        public CountriesUploaderService(ICountriesRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<int> UploadFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();

            await formFile.CopyToAsync(memoryStream);

            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;

                for(int row = 2 ; row <= rowCount; row++)
                {
                    string? cellValue = workSheet.Cells[row, 1].Value.ToString();

                    if(!string.IsNullOrEmpty(cellValue)) 
                    { 
                        string countryName = cellValue;

                        if (await _countryRepository.GetCountryByName(countryName) == null)
                        {
                            CountryAddRequest countryAddRequest = new CountryAddRequest()
                            {
                                CountryName = countryName,
                            };

                            await _countryRepository.AddCountry(countryAddRequest.ToCountry());

                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}