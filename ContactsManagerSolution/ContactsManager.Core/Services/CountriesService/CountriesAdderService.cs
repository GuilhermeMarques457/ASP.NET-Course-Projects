using ContactsManager.Core.DTO.CountryDTO;
using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using System.Linq;

namespace Services.CountriesService
{
    public class CountriesAdderService : ICountriesAdderService
    {
        private readonly ICountriesRepository _countryRepository;

        public CountriesAdderService(ICountriesRepository countryRepository)
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

    }
}