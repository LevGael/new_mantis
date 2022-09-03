using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;

namespace newMantis.Infrastructure
{
    public class CustomFirstConnectionFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if(!string.IsNullOrEmpty(filterContext.HttpContext.Session.GetString("isfirst")))
            {
                filterContext.Result = filterContext.Result = new RedirectToRouteResult(  
                new RouteValueDictionary  
                {  
                     { "controller", "Connexion" },
                     { "action", "Premiere_connexion" }  
                });
            }
        }
    }
    
}