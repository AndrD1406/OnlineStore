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

namespace OnlineStore.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJwtService jwtService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userMng,
            SignInManager<ApplicationUser> signInMng, RoleManager<ApplicationRole> roleMng, IJwtService _jwtService)
        {
            _logger = logger;
            jwtService = _jwtService;
            userManager = userMng;
            signInManager = signInMng;
            roleManager = roleMng;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var accessToken = Request.Cookies["AccessToken"];

            if (accessToken == null)
                return RedirectToAction(nameof(Register));
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

            if (result.Succeeded)
            {
                // sign-in
                // isPersister: false - must be deleted automatically when the browser is closed
                await signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;

                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await userManager.UpdateAsync(user);

                Response.Cookies.Append("AccessToken", authenticationResponse.Token);

                return RedirectToAction(nameof(Index), null, authenticationResponse?.Token);
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
