using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class RegisterVoteCommandHandler : BaseHandler<RegisterVoteCommand>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IConferenceRepository _conferenceRepository;

        public RegisterVoteCommandHandler(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            _voteRepository = voteRepository;
            _conferenceRepository = conferenceRepository;
        }

        public override void Handle(RegisterVoteCommand message)
        {
            var conference = _conferenceRepository.ForSession(message.SessionId);
            if (conference == null || !conference.CanVote())
            {
                return;
            }
            if (_voteRepository.HasVotedFor(message.SessionId, message.CookieId))
            {
                return;
            }
            _voteRepository.Save(new Vote
                {
                    CookieId = message.CookieId,
                    IPAddress = message.IPAddress,
                    Referrer = message.Referrer,
                    ScreenResolution = message.ScreenResolution,
                    SessionId = message.SessionId,
                    TimeRecorded = message.TimeRecorded,
                    UserAgent = message.UserAgent,
                    UserId = message.UserId,
                    WebSessionId = message.WebSessionId
                });
        }
    }
}