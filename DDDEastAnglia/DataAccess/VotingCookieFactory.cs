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
                throw new ArgumentNullException(nameof(conferenceLoader));
            }

            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException(nameof(calendarItemRepository));
            }

            this.conferenceLoader = conferenceLoader;
            this.calendarItemRepository = calendarItemRepository;
        }

        public VotingCookie Create()
        {
            var cookieName = GetCookieName();
            var cookieExpiry = GetCookieExpiryDate();
            return new VotingCookie {
                Name = cookieName,
                Expiry = cookieExpiry
            };
        }

        private string GetCookieName()
        {
            var conference = conferenceLoader.LoadConference();
            return $"{conference.ShortName}.Voting";
        }

        private DateTime GetCookieExpiryDate()
        {
            var votingPeriod = calendarItemRepository.GetFromType(CalendarEntryType.Voting);
            var cookieExpiry = votingPeriod.EndDate.Value + TimeSpan.FromDays(1);
            return cookieExpiry.DateTime;
        }
    }
}
