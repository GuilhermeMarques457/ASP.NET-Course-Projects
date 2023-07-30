using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO.CountryDTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulting country entity
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Add a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Retuns the country object after adding it</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);        
    }
}