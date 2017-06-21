using System;
using System.Linq;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;
using DDDEastAnglia.Models.Query;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class BannerModelQuery : IBannerModelQuery
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly ICalendarItemRepository calendarItemRepository;

        public BannerModelQuery(IConferenceLoader conferenceLoader, ICalendarItemRepository calendarItemRepository)
        {
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            this.conferenceLoader = conferenceLoader;
            this.calendarItemRepository = calendarItemRepository;
        }

        public BannerModel Get()
        {
            var conference = conferenceLoader.LoadConference();

            if (conference == null)
            {
                return new BannerModel();
            }

            DateTimeOffset submissionCloses = DateTimeOffset.Now.AddDays(-1);
            DateTimeOffset votingCloses = DateTimeOffset.Now.AddDays(-1);

            var allDates = calendarItemRepository.GetAll().ToDictionary(c => c.EntryType, c => c);
            var submission = allDates[CalendarEntryType.SessionSubmission];

            if (submission?.EndDate != null)
            {
                submissionCloses = submission.EndDate.Value;
            }

            var voting = allDates[CalendarEntryType.Voting];

            if (voting?.EndDate != null)
            {
                votingCloses = voting.EndDate.Value;
            }

            return new BannerModel
            {
                IsOpenForSubmission = conference.CanSubmit(),
                IsOpenForVoting = conference.CanVote(),
                SessionSubmissionCloses = submissionCloses.ToString("R"),
                VotingCloses = votingCloses.ToString("R")
            };
        }
    }
}
