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
        private VotingCookie cookieWithNoVotes;

        protected override void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            cookieWithNoVotes = new VotingCookie(VotingCookie.CookieName);
            repository.Get(Arg.Is(cookieWithNoVotes.Name)).Returns(cookieWithNoVotes);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(KnownSessionId)).Returns(true);
        }

        [Test]
        public void Record_The_EventID()
        {
            Controller.RegisterVote(KnownSessionId);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.Event == "DDDEA2013"));
        }

        [Test]
        public void Record_The_SessionId()
        {
            Controller.RegisterVote(KnownSessionId);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.SessionId == KnownSessionId));
        }

        [Test]
        public void Record_The_Cookie_Guid()
        {
            Controller.RegisterVote(KnownSessionId);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.CookieId.Equals(cookieWithNoVotes.Id)));
        }

        [Test]
        public void Record_The_Time_Of_The_Vote()
        {
            Controller.RegisterVote(1);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.TimeRecorded == SimulatedNow));
        }

        [Test]
        public void Record_That_The_Vote_Is_Actually_A_Vote()
        {
            Controller.RegisterVote(1);

            VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.IsVote));
        }
    }
}