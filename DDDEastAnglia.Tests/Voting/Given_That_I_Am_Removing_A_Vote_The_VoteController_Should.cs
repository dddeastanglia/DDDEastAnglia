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
    public class Given_That_I_Am_Removing_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(KnownSessionId).Returns(true);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(KnownSessionId)).Returns(true);
        }

        [Test]
        public void Delete_The_Appropriate_Vote()
        {
            Controller.RemoveVote(KnownSessionId);

            CurrentUserVoteRepository.Received().Delete(Arg.Is<int>(KnownSessionId));
        }

    }
}