using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class ConferenceIsInPreviewFilterAttribute : ActionFilterAttribute
    {
        private readonly IConferenceLoader conferenceLoader;

        public ConferenceIsInPreviewFilterAttribute(IConferenceLoader conferenceLoader)
        {
            this.conferenceLoader = conferenceLoader;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var conference = conferenceLoader.LoadConference();

            if (conference.IsPreview())
            {
                filterContext.Result = new RedirectResult("~/Preview");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
