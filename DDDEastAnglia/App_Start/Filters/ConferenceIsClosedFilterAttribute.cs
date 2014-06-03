using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class ConferenceIsClosedFilterAttribute : ActionFilterAttribute
    {
        private readonly IConferenceLoader conferenceLoader;

        public ConferenceIsClosedFilterAttribute(IConferenceLoader conferenceLoader)
        {
            this.conferenceLoader = conferenceLoader;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var conference = conferenceLoader.LoadConference();

            if (conference.IsClosed())
            {
                filterContext.Result = new RedirectResult("~/Closed");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
