﻿using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class ConferenceIsClosedFilter : ActionFilterAttribute
    {
        private readonly IConferenceLoader conferenceLoader;

        public ConferenceIsClosedFilter(IConferenceLoader conferenceLoader)
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

            if (conference.IsClosed())
            {
                filterContext.Result = new RedirectResult("~/Closed");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
