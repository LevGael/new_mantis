using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;

namespace newMantis.Infrastructure
{
    public class CustomAuthFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if(string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("email")))
            {
                filterContext.Result = filterContext.Result = new RedirectToRouteResult(  
                new RouteValueDictionary  
                {  
                     { "controller", "Connexion" },
                     { "action", "Index" }  
                });
            }
        }
    }
    
}