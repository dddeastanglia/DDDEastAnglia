using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia
{
    public interface IVotingCookie
    {
        string CookieName { get; }
        DateTime DefaultExpiry { get; }
    }

    public class VotingCookie : IVotingCookie
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly ICalendarItemRepository calendarItemRepository;

        public VotingCookie(IConferenceLoader conferenceLoader, ICalendarItemRepository calendarItemRepository)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }
            
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            this.conferenceLoader = conferenceLoader;
            this.calendarItemRepository = calendarItemRepository;
        }

        public string CookieName
        {
            get
            {
                var conference = conferenceLoader.LoadConference();
                return string.Format("{0}.Voting", conference.ShortName);
            }
        }

        public DateTime DefaultExpiry
        {
            get
            {
                var votingPeriod = calendarItemRepository.GetFromType(CalendarEntryType.Voting);
                var cookieExpiry = votingPeriod.EndDate.Value + TimeSpan.FromDays(1);
                return cookieExpiry.DateTime;
            }
        }
    }
}
