using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class RegisterVoteCommandHandler : BaseHandler<RegisterVoteCommand>
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceLoader conferenceLoader;

        public RegisterVoteCommandHandler(IVoteRepository voteRepository, IConferenceLoader conferenceLoader)
        {
            this.voteRepository = voteRepository;
            this.conferenceLoader = conferenceLoader;
        }

        public override void Handle(RegisterVoteCommand message)
        {
            var conference = conferenceLoader.LoadConference(message.SessionId);

            if (conference == null || !conference.CanVote())
            {
                return;
            }
            
            if (voteRepository.HasVotedFor(message.SessionId, message.CookieId))
            {
                return;
            }

            voteRepository.AddVote(new Vote
            {
                CookieId = message.CookieId,
                IPAddress = message.IPAddress,
                Referrer = message.Referrer,
                ScreenResolution = message.ScreenResolution,
                SessionId = message.SessionId,
                TimeRecorded = message.TimeRecorded,
                UserAgent = message.UserAgent,
                UserId = message.UserId,
                WebSessionId = message.WebSessionId,
                PositionInList = message.PositionInList
            });
        }
    }
}
