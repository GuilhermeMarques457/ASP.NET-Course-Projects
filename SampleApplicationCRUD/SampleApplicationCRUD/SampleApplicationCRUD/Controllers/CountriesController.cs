using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace SampleApplicationCRUD.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("[action]")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an excel file properly";
                return View();
            }

            if(!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Please select a file of xlsx (excel) extension";
                return View();
            }

            int countriesInserted = await _countriesService.UploadFromExcelFile(excelFile);

            ViewBag.Message = $"{countriesInserted} Countries Uploaded";

            return View();
        }
    }

  
}
