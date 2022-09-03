using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;

namespace newMantis.Infrastructure
{
    [CustomAuthFilter]
    public class CustomSuperUserFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if(string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("role")) || filterContext.HttpContext.Session.GetString("role") != "1")
            {
                filterContext.Result = filterContext.Result = new RedirectToRouteResult(  
                new RouteValueDictionary  
                {  
                     { "controller", "Home" },
                     { "action", "Index" }  
                });
            }
        }
    }
    
}