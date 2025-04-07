using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OnlineStore.BusinessLogic.Dtos;
using OnlineStore.Controllers;
using OnlineStore.DataAccess.Models;
using OnlineStore.Tests.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Tests.Controllers;

[TestFixture]
public class HomeControllerTests : IDisposable
{
    private HomeController _controller;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<RoleManager<ApplicationRole>> _roleManagerMock;

    [SetUp]
    public void Setup()
    {
        // Setup UserManager mock
        _userManagerMock = GetUserManagerMock<ApplicationUser>();

        // Setup SignInManager mock
        _signInManagerMock = GetSignInManagerMock(_userManagerMock.Object);

        // Setup RoleManager mock (using a dummy role store)
        var roleStore = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(roleStore.Object, null, null, null, null);

        _controller = new HomeController(_userManagerMock.Object, _signInManagerMock.Object, _roleManagerMock.Object);
    }

    // Helper to create a mock UserManager
    private static Mock<UserManager<TUser>> GetUserManagerMock<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    // Helper to create a mock SignInManager
    private static Mock<SignInManager<TUser>> GetSignInManagerMock<TUser>(UserManager<TUser> userManager) where TUser : class
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
        return new Mock<SignInManager<TUser>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
    }

    [Test]
    public void Index_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void Register_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Register();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task Register_Post_ValidModel_ReturnsRedirectToProductIndex()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Password = "Password1!",
            IsAdmin = false
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Build a service provider that registers logging, authentication, and MVC services.
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { });
        services.AddMvc(); // Registers IUrlHelperFactory among other MVC services.
        var serviceProvider = services.BuildServiceProvider();

        // Set up a default HttpContext with RequestServices containing the registered services.
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };

        // Optionally, assign a UrlHelper to the controller.
        _controller.ControllerContext.HttpContext = httpContext;
        _controller.Url = new UrlHelper(new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor()));

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Product");
    }

    [Test]
    public void Login_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task Login_Post_ValidCredentials_ReturnsRedirectToProductIndex()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "Password1!"
        };

        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _userManagerMock.Setup(um => um.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(new ApplicationUser { Id = Guid.NewGuid(), Email = loginDto.Email });
        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "User" });
        _signInManagerMock.Setup(sm => sm.SignInAsync(It.IsAny<ApplicationUser>(), false, null))
            .Returns(Task.CompletedTask);
        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Build a service provider with authentication and MVC services.
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => { });
        services.AddMvc(); // Registers IUrlHelperFactory, etc.
        var serviceProvider = services.BuildServiceProvider();

        // Create a DefaultHttpContext with the service provider.
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };

        // Set up the controller's ControllerContext and Url helper.
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor();
        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
        _controller.ControllerContext = new ControllerContext(actionContext);
        _controller.Url = new UrlHelper(actionContext);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Product");
    }

    [Test]
    public async Task LogOut_WhenCalled_ReturnsRedirectToProductIndex()
    {
        // Arrange: Build a service provider with cookie authentication and MVC services.
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Identity.Application";
        })
        .AddCookie("Identity.Application", options => { });
        services.AddMvc(); // Registers IUrlHelperFactory and other MVC services.
        var serviceProvider = services.BuildServiceProvider();

        // Create a DefaultHttpContext with the built service provider.
        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };

        // Create an ActionContext with HttpContext, RouteData, and a ControllerActionDescriptor.
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor();
        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        // Assign the ActionContext and explicitly set the Url property.
        _controller.ControllerContext = new ControllerContext(actionContext);
        _controller.Url = new UrlHelper(actionContext);

        // Act: Call the LogOut action.
        var result = await _controller.LogOut();

        // Assert: Verify the redirect action.
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Product");
    }

    [Test]
    public async Task ModifyPersonalAccount_Get_ReturnsViewWithRegisterDto()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser
        {
            Id = Guid.Parse(userId),
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890"
        };

        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        // Act
        var result = await _controller.ModifyPersonalAccount();

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult.Model.Should().BeOfType<RegisterDto>();
        var model = viewResult.Model as RegisterDto;
        model.Name.Should().Be(user.Name);
        model.Email.Should().Be(user.Email);
        model.PhoneNumber.Should().Be(user.PhoneNumber);
    }

    [Test]
    public async Task Users_Get_ReturnsViewWithUserList()
    {
        // Arrange
        var users = new List<ApplicationUser>
    {
        new ApplicationUser { Id = Guid.NewGuid(), Name = "User One", Email = "one@example.com", PhoneNumber = "111" },
        new ApplicationUser { Id = Guid.NewGuid(), Name = "User Two", Email = "two@example.com", PhoneNumber = "222" }
    };

        // Wrap the users list in a TestAsyncEnumerable so that async operations like ToListAsync work.
        var asyncUsers = new TestAsyncEnumerable<ApplicationUser>(users.AsQueryable());
        _userManagerMock.Setup(um => um.Users).Returns(asyncUsers);
        _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "User" });

        // Set up a fake ClaimsPrincipal with the Admin role (since [Authorize(Roles="Admin")] is applied).
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, "Admin")
    };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };

        // Act
        var result = await _controller.Users(null, null);

        // Assert
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult.Model.Should().BeAssignableTo<IEnumerable<object>>();
    }

    [Test]
    public async Task DeleteUser_ValidId_ReturnsRedirectToUsers()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = Guid.Parse(userId), Email = "test@example.com", Name = "Test User" };
        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Users");
    }

    [Test]
    public async Task DeleteUser_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var userId = "";
        // Act
        var result = await _controller.DeleteUser(userId);
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void CreateUser_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.CreateUser();
        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task CreateUser_Post_ValidModel_ReturnsRedirectToUsers()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Name = "New User",
            Email = "newuser@example.com",
            PhoneNumber = "333333",
            Password = "Password1!",
            IsAdmin = false
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.CreateUser(registerDto);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        redirectResult.Should().NotBeNull();
        redirectResult.ActionName.Should().Be("Users");
    }

    [TearDown]
    public void TearDown()
    {
        _controller?.Dispose();
    }

    public void Dispose()
    {
        TearDown();
    }
}
