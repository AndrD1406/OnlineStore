using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Win32;

namespace OnlineStore.Attributes
{
    public class OnlineStoreAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Result = new ViewResult() { ViewName = "Home/Login" };
                //context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized", redirectUrl = "/Home/Register" });
            }
            else
            {
                context.Result = new ViewResult()
                {
                    ViewName = ""
                };
            }
        }
    }
}
