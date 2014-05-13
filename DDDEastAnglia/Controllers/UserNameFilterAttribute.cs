using System;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public sealed class UserNameFilterAttribute : ActionFilterAttribute
    {
        private readonly string userNameParameterName;

        public UserNameFilterAttribute(string userNameParameterName)
        {
            if (userNameParameterName == null)
            {
                throw new ArgumentNullException("userNameParameterName", "A parameter name must be specified");
            }

            if (string.IsNullOrWhiteSpace(userNameParameterName))
            {
                throw new ArgumentException("A parameter name must be specified", "userNameParameterName");
            }
            
            this.userNameParameterName = userNameParameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey(userNameParameterName))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.ActionParameters[userNameParameterName] = filterContext.HttpContext.User.Identity.Name;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
