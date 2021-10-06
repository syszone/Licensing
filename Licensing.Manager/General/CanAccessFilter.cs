using Licensing.Manager.Controllers;
using Licensing.Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Licensing.Manager.General
{
    public class CanAccessFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                context.Result = new BadRequestObjectResult("Invalid!");

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var _usermanager = (UserManager<ApplicationUser>)context.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
                var user = await _usermanager.FindByIdAsync(userId);
                if (user != null && user.IsActive == true)
                {
                    var listMenu = await HomeController.GetMenuItem();
                    var menu = listMenu.Where(r => r.URL == context.HttpContext.Request.Path.Value).FirstOrDefault();
                    if (menu != null && menu.HasAccess == true)
                    {
                        await next();
                    }
                    else
                    {
                        context.Result = CreateRedirect();
                    }
                }
                else
                {
                    var _signinmanager = (SignInManager<ApplicationUser>)context.HttpContext.RequestServices.GetService(typeof(SignInManager<ApplicationUser>));
                    await _signinmanager.SignOutAsync();
                    context.Result = new RedirectToRouteResult("/Identity/Account/Login");
                }
            }

        }

        private IActionResult CreateRedirect() => new RedirectToRouteResult(new RouteValueDictionary {
                { "controller", "Home" },
                { "action", "Unauth" }
        });

    }
}
