using System;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess
{
    public interface IVotingCookieFactory
    {
        VotingCookie Create();
    }

    public class VotingCookieFactory : IVotingCookieFactory
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly ICalendarItemRepository calendarItemRepository;

        public VotingCookieFactory(IConferenceLoader conferenceLoader, ICalendarItemRepository calendarItemRepository)
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

        public VotingCookie Create()
        {
            var cookieName = GetCookieName();
            var cookieExpiry = GetCookieExpiryDate();
            return new VotingCookie(cookieName, cookieExpiry);
        }

        private string GetCookieName()
        {
            var conference = conferenceLoader.LoadConference();
            return string.Format("{0}.Voting", conference.ShortName);
        }

        private DateTime GetCookieExpiryDate()
        {
            var votingPeriod = calendarItemRepository.GetFromType(CalendarEntryType.Voting);
            var cookieExpiry = votingPeriod.EndDate.Value + TimeSpan.FromDays(1);
            return cookieExpiry.DateTime;
        }
    }
}
