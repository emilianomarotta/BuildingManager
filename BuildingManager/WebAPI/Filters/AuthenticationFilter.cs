using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IBusinessLogic;
using Domain;

namespace WebApi.Filters;

public class AuthenticationFilter : Attribute, IAuthorizationFilter

{
    private string _roles;
    public AuthenticationFilter(string roles)
    {
        _roles = roles;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Authorization"];

        if (String.IsNullOrEmpty(token))
        {
            context.Result = new JsonResult("Empty authorization header") { StatusCode = 401 };
        }
        else if (!Guid.TryParse(token, out Guid parsedToken))
        {
            context.Result = new JsonResult("Invalid token format") { StatusCode = 400 };
        }
        else
        {
            var currentUser = GetSessionLogicService(context).GetCurrentUser(parsedToken);

            if (currentUser == null)
            {
                context.Result = new JsonResult("Token not exist") { StatusCode = 401 };
            }
            string userRole = GetRoleForUser(currentUser);
            if (!_roles.Contains(userRole))
            {
                context.Result = new JsonResult("Unauthorized") { StatusCode = 403 };
            }
        }
    }

    ISessionLogic GetSessionLogicService(AuthorizationFilterContext context)
    {
        var sessionManagerObject = context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
        var sessionService = sessionManagerObject as ISessionLogic;

        return sessionService;
    }
    private string GetRoleForUser(User user)
    {
        if (user is Administrator)
        {
            return "Administrator";
        }
        if (user is Staff)
        {
            return "Staff";
        }
        if (user is CompanyAdmin)
        {
            return "CompanyAdmin";
        }
        return "Manager";
    }
}