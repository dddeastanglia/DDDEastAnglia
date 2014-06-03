using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData;
using DDDEastAnglia.DataAccess.SimpleData.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Builders.Calendar;

namespace DDDEastAnglia.Filters
{
    public abstract class FilterProvider : IFilterProvider
    {
        public abstract IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
        protected abstract object CreateFilter(IConferenceLoader conferenceLoader);

        public IEnumerable<Filter> GetFilters<T>(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            // child actions bypass the filter because they are being called from another action
            if (controllerContext.IsChildAction)
            {
                return Enumerable.Empty<Filter>();
            }

            var controllerType = controllerContext.Controller.GetType();
            var allowedInPreviewDefinedOnController = HasExcludeFromFilterAttributeDefined<T>(controllerType);
            var allowedInPreviewDefinedOnAction = HasExcludeFromFilterAttributeDefined<T>(controllerType.GetMethod(actionDescriptor.ActionName));

            // we don't need to apply the filter if there is an attribute specifically allowing the action to bypass the filter
            if (allowedInPreviewDefinedOnController || allowedInPreviewDefinedOnAction)
            {
                return Enumerable.Empty<Filter>();
            }

            var conferenceLoader = new ConferenceLoader(new ConferenceRepository(), new ConferenceBuilder(new CalendarItemRepository(), new CalendarEntryBuilder()));
            var filter = CreateFilter(conferenceLoader);
            return new[] { new Filter(filter, FilterScope.Global, null) };
        }

        private bool HasExcludeFromFilterAttributeDefined<T>(dynamic source)
        {
            T[] customAttributes = source.GetCustomAttributes(typeof(T), true);
            return customAttributes.Any();
        }
    }
}
