using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO.AccountDTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleApplicationCRUD.Controllers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // Class that will Manipulate the model class (UserManager)
        // Class that is the model to be manipulated <ApplicationUser>
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            if(!ModelState.IsValid)
            {
                ViewBag.Erros = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                PersonName = registerDTO.PersonName,
                UserName = registerDTO.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO!.Password);

            if(result.Succeeded)
            {
                // Creating a User in "Admin" role
                if(registerDTO.UserType == UserTypeOptions.Admin)
                {
                    // Creating a Admin role in the DB if does not exist
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole() 
                        {
                            Name = UserTypeOptions.Admin.ToString(),
                        };

                        // Creating the role in the DB
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    // Creating User and registering it in the Admin role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                // Creating a User in "User" role
                else
                {
                    // Creating a User role in the DB if does not exist
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString(),
                        };

                        // Creating the role in the DB
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    // Creating User and registering it in the User role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                // True: Persistent cookie, false...
                await _signInManager.SignInAsync(user, isPersistent: registerDTO.RememberMe);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(registerDTO);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Erros = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(loginDTO);
            }

            // lockoutOnFailure: true
            // After three attempts block that account to try again
            SignInResult result = await _signInManager.PasswordSignInAsync(loginDTO!.Email, loginDTO!.Password, isPersistent: loginDTO.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if(user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }

                    return RedirectToAction(nameof(PersonsController.Index), "Persons");
                }
                
            }

            ModelState.AddModelError("Login", "Invalid email or password");

            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        public async Task<IActionResult> EmailValidator(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return Json(true);
            }

            return Json(false);

           
        }
    }
}
