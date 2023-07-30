
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulting country entity
    /// </summary>
    public interface ICountriesUploaderService
    {
        /// <summary>
        /// Upload a file from excel to convert the information and save it in the database
        /// </summary>
        /// <param name="formFile">The file with countries inside</param>
        /// <returns>Numbers of countries saved</returns>
        Task<int> UploadFromExcelFile(IFormFile formFile);
    }
}