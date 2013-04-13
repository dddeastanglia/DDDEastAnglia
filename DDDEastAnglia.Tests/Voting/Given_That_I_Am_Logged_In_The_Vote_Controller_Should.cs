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
    public class Given_That_I_Am_Logged_In_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        private const int MyUserId = 100;
        private const string MyUserName = "Bob";
        private static readonly int[] CurrentSessionIds = new[] { SessionIdToRemove };
        private VotingCookie cookieWithOneVote;

        private readonly UserProfile myUserProfile = new UserProfile
            {
                UserId = MyUserId,
                UserName = MyUserName
            };


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
            requestInformationProvider.IsLoggedIn().Returns(true);
            requestInformationProvider.GetCurrentUser().Returns(myUserProfile);
        }

        [Test]
        public void Save_My_UserId_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.UserId == myUserProfile.UserId));
        }
    }
}