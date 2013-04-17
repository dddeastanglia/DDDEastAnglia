using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Am_Registering_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(KnownSessionId).Returns(false);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(KnownSessionId)).Returns(true);
        }


        [Test]
        public void Record_The_SessionId()
        {
            Controller.RegisterVote(KnownSessionId);

            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.SessionId == KnownSessionId));
        }

        [Test]
        public void Record_The_Time_Of_The_Vote()
        {
            Controller.RegisterVote(1);

            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.TimeRecorded == SimulatedNow));
        }

    }
}