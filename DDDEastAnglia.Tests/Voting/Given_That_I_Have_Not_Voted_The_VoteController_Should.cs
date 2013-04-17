using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Have_Not_Voted_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;
        private const int UnknownSessionId = 10;

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(KnownSessionId)
                .Returns(false);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(KnownSessionId)).Returns(true);
            sessionRepository.Exists(Arg.Is(UnknownSessionId)).Returns(false);
        }

        [Test]
        public void Register_A_Vote_For_A_Session()
        {
           Controller.RegisterVote(KnownSessionId);

           CurrentUserVoteRepository.Received()
                      .Save(Arg.Is<Vote>(vote => vote.SessionId == KnownSessionId));
        }

        [Test]
        public void Not_Set_A_Cookie_When_Trying_To_Remove_A_Session()
        {
            Controller.RemoveVote(KnownSessionId);
            CurrentUserVoteRepository.DidNotReceive()
                    .Save(Arg.Any<Vote>());
        }

        [Test]
        public void Set_An_Empty_Cookie_When_Trying_To_Add_An_Unknown_Session()
        {
            Controller.RegisterVote(UnknownSessionId);
            CurrentUserVoteRepository.DidNotReceiveWithAnyArgs()
                    .Save(null);
        }

        [Test]
        public void Save_The_Vote_To_The_Database()
        {
            Controller.RegisterVote(KnownSessionId);
            CurrentUserVoteRepository.Received()
                          .Save(Arg.Is<Vote>(vote => vote.IsVoteFor(KnownSessionId)));
        }

        [Test]
        public void Fail_To_Register_A_Vote_In_The_Database_If_Session_Is_Not_Known()
        {
            Controller.RegisterVote(UnknownSessionId);
            CurrentUserVoteRepository.DidNotReceiveWithAnyArgs().Save(null);
        }
    }
}
