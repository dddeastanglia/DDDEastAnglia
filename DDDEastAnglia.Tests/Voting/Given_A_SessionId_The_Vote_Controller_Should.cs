using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_A_SessionId_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        protected const string DefaultSessionID = "THIS IS A SESSION ID";

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(SessionIdToVoteFor).Returns(false);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(SessionIdToVoteFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(SessionIdToRemove)).Returns(true);
        }

        protected override void SetRequestInformationProviderExpectations(IRequestInformationProvider requestInformationProvider)
        {
            base.SetRequestInformationProviderExpectations(requestInformationProvider);
            requestInformationProvider.SessionId.Returns(DefaultSessionID);
        }

        [Test]
        public void Save_The_SessionId_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.WebSessionId == DefaultSessionID));
        }
    }
}