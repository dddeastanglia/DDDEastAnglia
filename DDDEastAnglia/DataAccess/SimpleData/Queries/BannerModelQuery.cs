using System;
using System.Linq;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;
using DDDEastAnglia.Models.Query;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class BannerModelQuery : IBannerModelQuery
    {
        private readonly IBuild<Conference, Domain.Conference> conferenceBuilder;
        private readonly IConferenceRepository conferenceRepository;
        private readonly ICalendarItemRepository calendarItemRepository;

        public BannerModelQuery(IBuild<Conference, Domain.Conference> conferenceBuilder, IConferenceRepository conferenceRepository, ICalendarItemRepository calendarItemRepository)
        {
            if (conferenceBuilder == null)
            {
                throw new ArgumentNullException("conferenceBuilder");
            }

            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }
            
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }
            
            this.conferenceBuilder = conferenceBuilder;
            this.conferenceRepository = conferenceRepository;
            this.calendarItemRepository = calendarItemRepository;
        }

        public BannerModel Get(string conferenceShortName)
        {
            var conference = conferenceRepository.GetByEventShortName(conferenceShortName);
                
            if (conference == null)
            {
                return new BannerModel();
            }
                
            var domainConference = conferenceBuilder.Build(conference);
                
            if (domainConference == null)
            {
                return new BannerModel();
            }
                
            DateTimeOffset submissionCloses = DateTimeOffset.Now.AddDays(-1);
            DateTimeOffset votingCloses = DateTimeOffset.Now.AddDays(-1);

            var allDates = calendarItemRepository.GetAll().ToDictionary(c => c.EntryType, c => c);
            var submission = allDates[CalendarEntryType.SessionSubmission];
                    
            if (submission != null && submission.EndDate.HasValue)
            {
                submissionCloses = submission.EndDate.Value;
            }
                    
            var voting = allDates[CalendarEntryType.Voting];

            if (voting != null && voting.EndDate.HasValue)
            {
                votingCloses = voting.EndDate.Value;
            }

            return new BannerModel
            {
                IsOpenForSubmission = domainConference.CanSubmit(),
                IsOpenForVoting = domainConference.CanVote(),
                SessionSubmissionCloses = submissionCloses.ToString("R"),
                VotingCloses = votingCloses.ToString("R")
            };
        }
    }
}
