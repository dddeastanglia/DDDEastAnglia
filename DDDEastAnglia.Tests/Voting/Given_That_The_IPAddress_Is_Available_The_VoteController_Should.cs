using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_The_IPAddress_Is_Available_The_VoteController_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        private const string LocalIpAddress = "127.0.0.1";

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(SessionIdToRemove).Returns(true);
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
            requestInformationProvider.GetIPAddress().Returns(LocalIpAddress);
        }

        [Test]
        public void Record_The_IPAddress_With_When_Creating_A_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);

            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.IPAddress == LocalIpAddress));
        }
    }
}