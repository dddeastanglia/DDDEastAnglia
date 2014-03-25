using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class DeleteVoteCommandHandler : BaseHandler<DeleteVoteCommand>
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceLoader conferenceLoader;

        public DeleteVoteCommandHandler(IVoteRepository voteRepository, IConferenceLoader conferenceLoader)
        {
            this.voteRepository = voteRepository;
            this.conferenceLoader = conferenceLoader;
        }

        public override void Handle(DeleteVoteCommand message)
        {
            var conference = conferenceLoader.LoadConference(message.SessionId);

            if (conference == null || !conference.CanVote())
            {
                return;
            }
            
            if (!voteRepository.HasVotedFor(message.SessionId, message.CookieId))
            {
                return;
            }
            
            voteRepository.Delete(message.SessionId, message.CookieId);
        }
    }
}