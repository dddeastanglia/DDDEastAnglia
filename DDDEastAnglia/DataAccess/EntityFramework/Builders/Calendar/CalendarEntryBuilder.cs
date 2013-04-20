using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar
{
    public class CalendarEntryBuilder : IBuild<CalendarItem, CalendarEntry>
    {
        private readonly IBuild<CalendarItem, SingleTimeEntry> _singleTimeEntryBuilder;
        private readonly IBuild<CalendarItem, TimeRangeEntry> _timeRangeBuilder;

        public CalendarEntryBuilder(IBuild<CalendarItem, SingleTimeEntry> singleTimeEntryBuilder,
                                    IBuild<CalendarItem, TimeRangeEntry> timeRangeBuilder)
        {
            _singleTimeEntryBuilder = singleTimeEntryBuilder;
            _timeRangeBuilder = timeRangeBuilder;
        }

        public CalendarEntry Build(CalendarItem item)
        {
            return _singleTimeEntryBuilder.Build(item) ?? _timeRangeBuilder.Build(item) ?? (CalendarEntry)new NullCalendarEntry();
        }
    }
}