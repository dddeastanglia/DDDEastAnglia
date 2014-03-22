using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataAccess.MessageBus;

namespace DDDEastAnglia.DataAccess.Handlers.Voting
{
    public class RegisterVoteCommandHandler : BaseHandler<RegisterVoteCommand>
    {
        private readonly IVoteRepository voteRepository;
        private readonly IConferenceRepository conferenceRepository;

        public RegisterVoteCommandHandler(IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            this.voteRepository = voteRepository;
            this.conferenceRepository = conferenceRepository;
        }

        public override void Handle(RegisterVoteCommand message)
        {
            var dataConference = conferenceRepository.ForSession(message.SessionId);
            var conference = new ConferenceBuilder(new CalendarEntryBuilder()).Build(dataConference);

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
                    WebSessionId = message.WebSessionId
                });
        }
    }
}
