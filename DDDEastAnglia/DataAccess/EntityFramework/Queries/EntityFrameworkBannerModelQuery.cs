using System;
using System.Linq;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;
using DDDEastAnglia.Models.Query;

namespace DDDEastAnglia.DataAccess.EntityFramework.Queries
{
    public class EntityFrameworkBannerModelQuery : IBannerModelQuery
    {
        private readonly IBuild<Conference, Domain.Conference> _conferenceBuilder;

        public EntityFrameworkBannerModelQuery(IBuild<Conference, Domain.Conference> conferenceBuilder)
        {
            _conferenceBuilder = conferenceBuilder;
        }


        public BannerModel Get(string conferenceShortName)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                var conference =
                    dddeaContext.Conferences.Include("CalendarItems")
                            .SingleOrDefault(item => item.ShortName == conferenceShortName);
                if (conference == null)
                {
                    return new BannerModel();
                }
                var domainConference = _conferenceBuilder.Build(conference);
                if (domainConference == null)
                {
                    return new BannerModel();
                }
                DateTimeOffset submissionCloses = DateTimeOffset.Now.AddDays(-1);
                DateTimeOffset votingCloses = DateTimeOffset.Now.AddDays(-1);
                if (conference.CalendarItems != null)
                {
                    var submission =
                        conference.CalendarItems.SingleOrDefault(
                            item => item.EntryType == CalendarEntryType.SessionSubmission);
                    if (submission != null && submission.EndDate.HasValue)
                    {
                        submissionCloses = submission.EndDate.Value;
                    }
                    var voting =
                        conference.CalendarItems.SingleOrDefault(
                            item => item.EntryType == CalendarEntryType.Voting);
                    if (voting != null && voting.EndDate.HasValue)
                    {
                        votingCloses = voting.EndDate.Value;
                    }
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
}