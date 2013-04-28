using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISessionVoteModelQuery
    {
        SessionVoteModel Get(int sessionId, Guid cookieId);
    }
    
    public class SessionVoteModelQuery : ISessionVoteModelQuery
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IConferenceRepository _conferenceRepository;

        public SessionVoteModelQuery(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            _voteRepository = voteRepository;
            _conferenceRepository = conferenceRepository;
        }

        public SessionVoteModel Get(int sessionId, Guid cookieId)
        {
            return new SessionVoteModel
                {
                    CanVote = _conferenceRepository.ForSession(sessionId).CanVote(),
                    HasBeenVotedForByUser = _voteRepository.HasVotedFor(sessionId, cookieId),
                    SessionId = sessionId
                };
        }
    }
}