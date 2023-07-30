using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplicationCRUD.Controllers
{
  
    public class HomeController : Controller
    {
        [Route("/Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            // We can access the feature anywhere
            // It has aditional information about our application
            IExceptionHandlerPathFeature? exeption = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if(exeption != null && exeption.Error != null)
            {
                ViewBag.ErrorMessage = exeption.Error.Message;
            }
            return View();
        }
    }
}
