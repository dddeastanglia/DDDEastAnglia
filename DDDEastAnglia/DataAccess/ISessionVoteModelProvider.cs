using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISessionVoteModelProvider
    {
        SessionVoteModel Get(int sessionId, Guid cookieId);
    }
    
    public class SessionVoteModelProvider : ISessionVoteModelProvider
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IConferenceRepository _conferenceRepository;

        public SessionVoteModelProvider(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
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