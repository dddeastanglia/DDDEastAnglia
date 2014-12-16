using System.Collections.Generic;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Filters
{
    public class PreviewFilterProvider : FilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return GetFilters<AllowedWhenConferenceIsInPreviewAttribute>(controllerContext, actionDescriptor);
        }

        protected override object CreateFilter(IConferenceLoader conferenceLoader)
        {
            return new ConferenceIsInPreviewFilter(conferenceLoader);
        }
    }
}
