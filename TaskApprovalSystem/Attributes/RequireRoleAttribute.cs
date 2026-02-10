namespace TaskApprovalSystem.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TaskApprovalSystem.Models;
public class RequireRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRoles[] _allowedRoles;

    public RequireRoleAttribute(params UserRoles[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        if (roleClaim == null ||
            !Enum.TryParse<UserRoles>(roleClaim, out var userRole) ||
            !_allowedRoles.Contains(userRole))
        {
            context.Result = new ForbidResult();
        }
    }
}