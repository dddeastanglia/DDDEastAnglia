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

        private readonly UserProfile myUserProfile = new UserProfile
            {
                UserId = MyUserId,
                UserName = MyUserName
            };


        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(SessionIdToRemove).Returns(true);
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
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.UserId == myUserProfile.UserId));
        }
    }
}