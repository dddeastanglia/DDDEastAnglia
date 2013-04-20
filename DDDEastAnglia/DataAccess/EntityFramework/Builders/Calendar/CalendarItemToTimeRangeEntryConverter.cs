using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar
{
    public class CalendarItemToTimeRangeEntryConverter : IBuild<CalendarItem, TimeRangeEntry>
    {
        public TimeRangeEntry Build(CalendarItem item)
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