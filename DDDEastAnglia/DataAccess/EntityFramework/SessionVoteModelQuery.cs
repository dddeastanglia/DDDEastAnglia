using System;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class SessionVoteModelQuery : ISessionVoteModelQuery
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceRepository conferenceRepository;

        public SessionVoteModelQuery(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            this.voteRepository = voteRepository;
            this.conferenceRepository = conferenceRepository;
        }

        public SessionVoteModel Get(int sessionId, Guid cookieId)
        {
            var dataConference = conferenceRepository.ForSession(sessionId);
            var conference = new ConferenceBuilder(new CalendarEntryBuilder()).Build(dataConference);

            return new SessionVoteModel
                {
                    CanVote = conference.CanVote(),
                    HasBeenVotedForByUser = voteRepository.HasVotedFor(sessionId, cookieId),
                    SessionId = sessionId
                };
        }
    }
}
