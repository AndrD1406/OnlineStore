using OnlineStore.BusinessLogic.Dtos;
using OnlineStore.DataAccess.Models;
using System.Security.Claims;

namespace OnlineStore.BusinessLogic.Services
{
	public interface IJwtService
	{
		AuthenticationResponse CreateJwtToken(ApplicationUser user);
		ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
	}
}
