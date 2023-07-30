using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO.CountryDTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulting country entity
    /// </summary>
    public interface ICountriesGetterService
    {

        /// <summary>
        /// Returns all countries as a list
        /// </summary>
        /// <returns>Returns all countries as a list</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given id
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Country object as countryResponse object</returns>
        Task<CountryResponse?> GetCountryById(Guid? countryID);

    }
}