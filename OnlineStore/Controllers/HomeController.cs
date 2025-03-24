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
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore;

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

            ApplicationUser user = new ApplicationUser() { Id = Guid.NewGuid(), Name = registerDto.Name, Email = registerDto.Email, PhoneNumber = registerDto.PhoneNumber, UserName = registerDto.Email };

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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, registerDto.IsAdmin ? "Admin" : "User")
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties() { IsPersistent = false });
                await signInManager.SignInAsync(user, isPersistent: false);
                await userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Product");
            }

            string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Validation 
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessages);
            }

            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                    return NoContent();

                await signInManager.SignInAsync(user, isPersistent: false);
                await userManager.UpdateAsync(user);

                var roles = await userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role,  roles.FirstOrDefault())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties() { IsPersistent = false });
                return RedirectToAction("Index", "Product");
            }
            return Problem("Invalid email or password");
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Product");
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ModifyPersonalAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await userManager.FindByIdAsync(userId);
            var registerDto = new RegisterDto() { Name=user.Name, Email=user.Email, PhoneNumber=user.PhoneNumber };

            return View(registerDto);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ModifyPersonalAccount(RegisterDto updatedUser)
        {
            // Validation 
            if (!ModelState.IsValid)
            {
                string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessages);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser user = new ApplicationUser();

            user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            try
            {
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.UserName = updatedUser.Email;

                var resultUpdateUser  = await userManager.UpdateAsync(user);
                if(!resultUpdateUser.Succeeded)
                {
                    string errorMessage = string.Join(" | ", resultUpdateUser.Errors.Select(e => e.Description));
                    return Problem(errorMessage);
                }

                var resultRemovePasword = await userManager.RemovePasswordAsync(user);
                if (!resultRemovePasword.Succeeded)
                {
                    string errorMessage = string.Join(" | ", resultRemovePasword.Errors.Select(e => e.Description));
                    return Problem(errorMessage);
                }

                var resultAddPassword = await userManager.AddPasswordAsync(user, updatedUser.Password);
                if (!resultAddPassword.Succeeded)
                {
                    string errorMessage = string.Join(" | ", resultAddPassword.Errors.Select(e => e.Description));
                    return Problem(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            await signInManager.RefreshSignInAsync(user);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, updatedUser.IsAdmin ? "Admin" : "User")
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties() { IsPersistent = false });

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users([FromQuery] string? name, [FromQuery] string? email)
        {
            ViewBag.name = name;
            ViewBag.email = email;
            var users = await userManager.Users.Where(x => (name != null ? x.Name.ToLower().Contains(name.ToLower()) : true) && (email != null ? x.Email.ToLower().Contains(email.ToLower()) : true)).ToListAsync();


            List<object> userList = new List<object>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    Name = user.Name,
                    user.Email,
                    user.PhoneNumber,
                    Role = roles.FirstOrDefault() 
                });
            }
            return View(userList);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Users"); 
            }

            string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }
    }
}
