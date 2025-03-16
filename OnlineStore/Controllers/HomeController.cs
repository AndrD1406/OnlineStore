using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Dtos;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.DataAccess.Models;
using OnlineStore.Models;
using System.Web;
using System.Net;
using OnlineStore.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userMng,
            SignInManager<ApplicationUser> signInMng, RoleManager<ApplicationRole> roleMng)
        {
            _logger = logger;
            userManager = userMng;
            signInManager = signInMng;
            roleManager = roleMng;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Validation 
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessages);
            }
            //return RedirectToAction(actionName: "Index", controllerName: "Product", null);

            // Create user
            ApplicationUser user = new ApplicationUser() { Id=Guid.NewGuid(), Name=registerDto.Name, Email=registerDto.Email, PhoneNumber=registerDto.PhoneNumber};

            user.UserName = registerDto.Email;


            IdentityResult result = null;
            try
            {
                result = await userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    if (registerDto.IsAdmin)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, registerDto.Email), new Claim(ClaimTypes.Name, registerDto.Email, ClaimTypes.Role, registerDto.IsAdmin ? "Admin" : "User") };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties()
            {
                IsPersistent = true,

            });

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                await userManager.UpdateAsync(user);
            }

            string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
