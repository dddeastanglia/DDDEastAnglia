using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.EntityFramework.Builders
{
    public class ConferenceBuilder : IBuild<Conference, Domain.Conference>
    {
        private readonly IBuild<CalendarItem, CalendarEntry> _calendarEntryBuilder;

        public ConferenceBuilder(IBuild<CalendarItem, CalendarEntry> calendarEntryBuilder)
        {
            _calendarEntryBuilder = calendarEntryBuilder;
        }

        public Domain.Conference Build(Conference item)
        {
            if (item == null)
            {
                return null;
            }
            var conference = new Domain.Conference(item.ConferenceId, item.Name, item.ShortName);
            if (item.CalendarItems == null)
            {
                return conference;
            }
            foreach (var calendarItem in item.CalendarItems)
            {
                conference.AddToCalendar(_calendarEntryBuilder.Build(calendarItem));
            }
            return conference;
        }
    }
}