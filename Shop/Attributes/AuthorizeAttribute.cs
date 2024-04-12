using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Models;


namespace Shop.Attributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {

        public AuthorizeAttribute() : base(typeof(AuthorizeWithRedirectFilter))
        {
        }

        public class AuthorizeWithRedirectFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    int? userId = context.HttpContext.Session.GetInt32("UserId");

                    if (!userId.HasValue)
                    {
                        context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    }
                }
            }
        }
    }
}
