using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar
{
    public class CalendarItemToSingleTimeEntryConverter : IBuild<CalendarItem, SingleTimeEntry>
    {
        public SingleTimeEntry Build(CalendarItem item)
        {
            if (item.EndDate.HasValue)
            {
                return null;
            }
            return new SingleTimeEntry(item.CalendarItemId, item.EntryType, item.Description, item.IsPublic, item.Authorised, item.StartDate);
        }
    }
}