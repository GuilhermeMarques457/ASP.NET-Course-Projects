using ContactsManager.Core.DTO.CountryDTO;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using System.Linq;

namespace Services.CountriesService
{
    public class CountriesGetterService : ICountriesGetterService
    {
        private readonly ICountriesRepository _countryRepository;

        public CountriesGetterService(ICountriesRepository countryRepository)
        {
            _countryRepository = countryRepository;
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

    }
}