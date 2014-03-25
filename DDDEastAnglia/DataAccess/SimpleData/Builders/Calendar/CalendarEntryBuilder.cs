using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.SimpleData.Builders.Calendar
{
    public class CalendarEntryBuilder : IBuild<CalendarItem, CalendarEntry>
    {
        public CalendarEntry Build(CalendarItem item)
        {
            return CreateSingleTimeEntry(item) ?? CreateTimeRangeEntry(item) ?? new NullCalendarEntry();
        }

        private CalendarEntry CreateSingleTimeEntry(CalendarItem item)
        {
            if (item.EndDate.HasValue)
            {
                return null;
            }
            return new SingleTimeEntry(item.CalendarItemId, item.EntryType, item.Description, item.IsPublic, item.Authorised, item.StartDate);
        }

        private CalendarEntry CreateTimeRangeEntry(CalendarItem item)
        {
            if (!item.EndDate.HasValue)
            {
                return null;
            }
            return new TimeRangeEntry(item.CalendarItemId,
                                      item.EntryType,
                                      item.Description,
                                      item.IsPublic,
                                      item.Authorised,
                                      item.StartDate,
                                      item.EndDate.Value);
        }
    }
}