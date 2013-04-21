using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class DeleteVoteCommandHandler : BaseHandler<DeleteVoteCommand>
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IConferenceRepository _conferenceRepository;

        public DeleteVoteCommandHandler(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            _voteRepository = voteRepository;
            _conferenceRepository = conferenceRepository;
        }

        public override void Handle(DeleteVoteCommand message)
        {
            var conference = _conferenceRepository.ForSession(message.SessionId);
            if (conference == null || !conference.CanVote())
            {
                return;
            }
            if (!_voteRepository.HasVotedFor(message.SessionId, message.CookieId))
            {
                return;
            }
            _voteRepository.Delete(message.SessionId, message.CookieId);
        }
    }
}