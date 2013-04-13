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
    public class Given_That_The_UserAgent_And_Referer_Are_Set_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        private const string UserAgent = "A Browser";
        private const string Referer = "http://www.referer.com";
        private static readonly int[] CurrentSessionIds = new[] { SessionIdToRemove };
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
        }

        protected override void SetRequestInformationProviderExpectations(IRequestInformationProvider requestInformationProvider)
        {
            base.SetRequestInformationProviderExpectations(requestInformationProvider);
            requestInformationProvider.UserAgent.Returns(UserAgent);
            requestInformationProvider.Referrer.Returns(Referer);
        }

        [Test]
        public void Save_The_UserAgent_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.UserAgent == UserAgent));
        }

        [Test]
        public void Save_The_Referer_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.Referrer == Referer));
        }
    }
}