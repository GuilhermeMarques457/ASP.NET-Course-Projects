using ServiceContracts.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulting country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Retuns the country object after adding it</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

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



        /// <summary>
        /// Upload a file from excel to convert the information and save it in the database
        /// </summary>
        /// <param name="formFile">The file with countries inside</param>
        /// <returns>Numbers of countries saved</returns>
        Task<int> UploadFromExcelFile(IFormFile formFile);
    }
}