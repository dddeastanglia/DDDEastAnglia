using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_The_UserAgent_And_Referer_Are_Set_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        private const string UserAgent = "A Browser";
        private const string Referer = "http://www.referer.com";

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(SessionIdToVoteFor)
                .Returns(false);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(SessionIdToVoteFor)).Returns(true);
        }

        protected override void SetRequestInformationProviderExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetRequestInformationProviderExpectations(controllerInformationProvider);
            controllerInformationProvider.UserAgent.Returns(UserAgent);
            controllerInformationProvider.Referrer.Returns(Referer);
        }

        [Test]
        public void Save_The_UserAgent_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.UserAgent == UserAgent));
        }

        [Test]
        public void Save_The_Referer_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.Referrer == Referer));
        }
    }
}