using System.Collections.Generic;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class ClosedFilterProvider : FilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return GetFilters<AllowedWhenConferenceIsClosedAttribute>(controllerContext, actionDescriptor);
        }

        protected override object CreateFilter(IConferenceLoader conferenceLoader)
        {
            return new ConferenceIsClosedFilterAttribute(conferenceLoader);
        }
    }
}
