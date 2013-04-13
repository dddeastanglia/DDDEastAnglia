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
        private static readonly int[] CurrentSessionIds = new[] {SessionIdToRemove};
        private VotingCookie cookieWithOneVote;

        protected override void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            cookieWithOneVote = new VotingCookie(Guid.NewGuid(), VotingCookie.CookieName, CurrentSessionIds, new DateTime(2013, 4, 30));
            repository.Get(Arg.Is(cookieWithOneVote.Name))
                .Returns(cookieWithOneVote);
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

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.IPAddress == LocalIpAddress));
        }

        [Test]
        public void Record_The_IPAddress_With_When_Removing_A_Vote()
        {
            Controller.RemoveVote(SessionIdToRemove);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.IPAddress == LocalIpAddress));
        }
    }
}