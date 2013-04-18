using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using DDDEastAnglia.Domain;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess.Handlers.Voting.DeleteVote
{
    [TestFixture]
    public class Given_The_User_Did_Not_Vote_For_The_Session_The_DeleteVotingCommandHandler_Should
    {
        private const int SessionId = 1;
        private static readonly Guid CookieId = Guid.NewGuid();
        private DeleteVoteCommandHandler _handler; 
        private IVoteRepository _voteRepository;

        private IConferenceRepository _conferenceRepository;

        [SetUp]
        public void BeforeEachTest()
        {
            _voteRepository = Substitute.For<IVoteRepository>();
            _voteRepository.HasVotedFor(Arg.Is(SessionId), Arg.Is(CookieId)).Returns(true);

            _conferenceRepository = Substitute.For<IConferenceRepository>();
            var conference = new Conference(SessionId, "", "");
            conference.ReadyForVoting();
            _conferenceRepository.ForSession(Arg.Is(1)).Returns(conference);

            _handler = new DeleteVoteCommandHandler(_voteRepository, _conferenceRepository);
        }

        [Test]
        public void Correctly_Delete_The_Vote()
        {
            _handler.Handle(new DeleteVoteCommand
                {
                    CookieId = CookieId,
                    SessionId = SessionId
                });

            _voteRepository.Received().Delete(Arg.Is(SessionId), Arg.Is(CookieId));
        }
    }
}