using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData;
using DDDEastAnglia.DataAccess.SimpleData.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Builders.Calendar;

namespace DDDEastAnglia.Filters
{
    public class PreviewFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            // child actions bypass the filter because they are being called from another action
            if (controllerContext.IsChildAction)
            {
                return Enumerable.Empty<Filter>();
            }

            var controllerType = controllerContext.Controller.GetType();
            var allowedInPreviewDefinedOnController = HasExcludeFromFilterAttributeDefined(controllerType);
            var allowedInPreviewDefinedOnAction = HasExcludeFromFilterAttributeDefined(controllerType.GetMethod(actionDescriptor.ActionName));

            // we don't need to apply the filter if there is an attribute specifically allowing the action to bypass the filter
            if (allowedInPreviewDefinedOnController || allowedInPreviewDefinedOnAction)
            {
                return Enumerable.Empty<Filter>();
            }

            var conferenceLoader = new ConferenceLoader(new ConferenceRepository(), new ConferenceBuilder(new CalendarItemRepository(), new CalendarEntryBuilder()));
            var filter = new ConferenceIsInPreviewFilterAttribute(conferenceLoader);
            return new[] { new Filter(filter, FilterScope.Global, null) };
        }

        private bool HasExcludeFromFilterAttributeDefined(dynamic source)
        {
            AllowedWhenConferenceIsInPreviewAttribute[] customAttributes = source.GetCustomAttributes(typeof(AllowedWhenConferenceIsInPreviewAttribute), true);
            return customAttributes.Any();
        }
    }
}
