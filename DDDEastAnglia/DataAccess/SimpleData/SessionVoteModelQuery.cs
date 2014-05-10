using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class SessionVoteModelQuery : ISessionVoteModelQuery
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceLoader conferenceLoader;

        public SessionVoteModelQuery(IVoteRepository voteRepository, IConferenceLoader conferenceLoader)
        {
            this.voteRepository = voteRepository;
            this.conferenceLoader = conferenceLoader;
        }

        public SessionVoteModel Get(int sessionId, Guid cookieId)
        {
            var conference = conferenceLoader.LoadConference(sessionId);

            return new SessionVoteModel
                {
                    CanVote = conference.CanVote(),
                    HasBeenVotedForByUser = voteRepository.HasVotedFor(sessionId, cookieId),
                    SessionId = sessionId
                };
        }
    }
}
