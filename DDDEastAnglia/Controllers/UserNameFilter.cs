using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public sealed class UserNameFilter : ActionFilterAttribute
    {
        private readonly string userNameParameterName;

        public UserNameFilter(string userNameParameterName)
        {
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
