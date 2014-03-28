using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess
{
    public interface ICalendarItemRepository
    {
        IEnumerable<CalendarItem> GetAll();
        CalendarItem GetFromType(CalendarEntryType voting);
    }
}