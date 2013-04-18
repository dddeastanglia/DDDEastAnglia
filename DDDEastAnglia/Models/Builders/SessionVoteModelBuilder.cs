using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Models.Builders
{
    public class SessionVoteModelBuilder : IBuild<SessionVoteModel>
    {
        private readonly IControllerInformationProvider _controllerInformationProvider;
        private readonly IVoteRepository _voteRepository;
        private readonly IConferenceRepository _conferenceRepository;

        public SessionVoteModelBuilder(IControllerInformationProvider controllerInformationProvider, IVoteRepository voteRepository, IConferenceRepository conferenceRepository)
        {
            _controllerInformationProvider = controllerInformationProvider;
            _voteRepository = voteRepository;
            _conferenceRepository = conferenceRepository;
        }

        public SessionVoteModel Get(int id)
        {
            return new SessionVoteModel
                {
                    SessionId = id,
                    HasBeenVotedForByUser = _voteRepository.HasVotedFor(id, _controllerInformationProvider.GetCookie(VotingCookie.CookieName).GetId()),
                    CanVote = _conferenceRepository.ForSession(id).CanVote()
                };
        }
    }
}