using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class ConferenceIsInPreviewFilter : ActionFilterAttribute
    {
        private readonly IConferenceLoader conferenceLoader;

        public ConferenceIsInPreviewFilter(IConferenceLoader conferenceLoader)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException(nameof(conferenceLoader));
            }

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
