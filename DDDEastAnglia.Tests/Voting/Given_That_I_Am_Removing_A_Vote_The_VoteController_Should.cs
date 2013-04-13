using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Am_Removing_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;
        private static readonly int[] SessionsVotedFor = new[] {KnownSessionId};
        private VotingCookie cookieWithNoVotes;

        protected override void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            cookieWithNoVotes = new VotingCookie(Guid.NewGuid(), VotingCookie.CookieName, SessionsVotedFor, new DateTime(2013, 4, 30));
            repository.Get(Arg.Is(cookieWithNoVotes.Name))
                            .Returns(cookieWithNoVotes);
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

            VoteRepository.Received().Delete(Arg.Is<int>(KnownSessionId), Arg.Is<Guid>(cookieWithNoVotes.Id));
        }

    }
}