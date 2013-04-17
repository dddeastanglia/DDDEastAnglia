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
    public class Given_That_I_Have_Voted_For_Several_Sessions_The_VoteController_Should : VotingTestBase
    {
        private const int FirstSessionVotedFor = 1;
        private const int SecondSessionVotedFor = 2;
        private const int SessionNotVotedFor = 3;
        private const int UnknownSessionId = 10;
        private static readonly int[] OnlySessionOne = new[] {FirstSessionVotedFor};
        private static readonly int[] SessionIdsVotedFor = new[] { FirstSessionVotedFor, SecondSessionVotedFor };
        private static readonly int[] SessionsExpectedAfterReceivingVote = new[] { FirstSessionVotedFor, SecondSessionVotedFor, SessionNotVotedFor };
        private VotingCookie cookieWithTwoVotes;

        protected override void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            cookieWithTwoVotes = new VotingCookie(Guid.NewGuid(), VotingCookie.CookieName, SessionIdsVotedFor, new DateTime(2013, 4, 30));
            repository.Get(Arg.Is(cookieWithTwoVotes.Name))
                .Returns(cookieWithTwoVotes);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(FirstSessionVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(SecondSessionVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(SessionNotVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(UnknownSessionId)).Returns(false);
        }

        [Test]
        public void Record_A_Vote_For_A_Session_That_I_Have_Not_Voted_For()
        {
            Controller.RegisterVote(SessionNotVotedFor);

            CookieRepository.Received()
                      .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, SessionsExpectedAfterReceivingVote)));
            VoteRepository.Received()
                      .Save(Arg.Is<Vote>(vote => vote.IsVoteFor("DDDEA2013", SessionNotVotedFor)));
        }

        [Test]
        public void Not_Record_A_Vote_If_The_User_Has_Already_Voted_For_The_Session()
        {
            Controller.RegisterVote(SecondSessionVotedFor);

            CookieRepository.DidNotReceive()
                      .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, SessionsExpectedAfterReceivingVote)));
            VoteRepository.DidNotReceive()
                      .Save(Arg.Is<Vote>(vote => vote.IsVoteFor("DDDEA2013", SessionNotVotedFor)));
        }

        [Test]
        public void Remove_A_Session_From_The_Cookie_If_It_Had_Been_Voted_For()
        {
            Controller.RemoveVote(SecondSessionVotedFor);

            CookieRepository.Received()
                .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, OnlySessionOne)));
        }

        [Test]
        public void Record_That_A_Vote_Has_Been_Removed()
        {
            Controller.RemoveVote(SecondSessionVotedFor);

            VoteRepository.Received()
                .Delete(Arg.Is<int>(SecondSessionVotedFor), Arg.Is<Guid>(cookieWithTwoVotes.Id));
        }
    }
}