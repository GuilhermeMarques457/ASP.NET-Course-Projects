using Entities;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        /// <summary>
        /// Add a country object to the list of countries
        /// </summary>
        /// <param name="country"></param>
        /// <returns>Retuns the country object after adding it</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries as a list
        /// </summary>
        /// <returns>Returns all countries as a list</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given id
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Country object as country object</returns>
        Task<Country?> GetCountryById(Guid? countryID);

        /// <summary>
        /// Returns a country based on the given country name
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByName(string countryName);

    }
}