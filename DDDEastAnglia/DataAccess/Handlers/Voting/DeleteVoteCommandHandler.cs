using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.DataAccess.SimpleData.Builders;
using DDDEastAnglia.DataAccess.SimpleData.Builders.Calendar;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class DeleteVoteCommandHandler : BaseHandler<DeleteVoteCommand>
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceRepository conferenceRepository;

        public DeleteVoteCommandHandler(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            this.voteRepository = voteRepository;
            this.conferenceRepository = conferenceRepository;
        }

        public override void Handle(DeleteVoteCommand message)
        {
            var dataConference = conferenceRepository.ForSession(message.SessionId);
            var conference = new ConferenceBuilder(new CalendarEntryBuilder()).Build(dataConference);

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