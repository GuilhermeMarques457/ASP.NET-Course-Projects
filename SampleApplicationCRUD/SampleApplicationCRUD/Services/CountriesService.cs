using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Linq;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countryRepository;

        public CountriesService(ICountriesRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            switch (countryAddRequest)
            {
                case null:
                    throw new ArgumentNullException(nameof(countryAddRequest));
            }

            switch (countryAddRequest.CountryName)
            {
                case null:
                    throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await _countryRepository.GetCountryByName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists");
            };

            Country country = countryAddRequest.ToCountry();
         
            country.CountryID = Guid.NewGuid(); 

            await _countryRepository.AddCountry(country);

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = await _countryRepository.GetAllCountries();
            List<CountryResponse> countryResponses = countries.Select(country => country.ToCountryResponse()).ToList();

            return countryResponses;
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryID)
        {
            if (countryID == null) 
            {
                return null;
            }
            else
            {
                Country? country = await _countryRepository.GetCountryById(countryID.Value);

                if(country == null)
                {
                    return null;
                }

                return country.ToCountryResponse();
            }
            

            
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