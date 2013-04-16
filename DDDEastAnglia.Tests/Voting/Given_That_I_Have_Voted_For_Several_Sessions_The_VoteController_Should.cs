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
        private static readonly int[] SessionsExpectedAfterReceivingVote = new[] { FirstSessionVotedFor, SecondSessionVotedFor, SessionNotVotedFor };
        private VotingCookie cookieWithTwoVotes;

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(FirstSessionVotedFor).Returns(true);
            repository.HasVotedFor(SecondSessionVotedFor).Returns(true);
            repository.HasVotedFor(SessionNotVotedFor).Returns(false);
            repository.HasVotedFor(UnknownSessionId).Returns(false);
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

            CurrentUserVoteRepository.Received()
                      .Save(Arg.Is<Vote>(vote => vote.SessionId == SessionNotVotedFor));
        }

        [Test]
        public void Not_Record_A_Vote_If_The_User_Has_Already_Voted_For_The_Session()
        {
            Controller.RegisterVote(SecondSessionVotedFor);

            CurrentUserVoteRepository.DidNotReceive()
                      .Save(Arg.Is<Vote>(vote => vote.SessionId == SecondSessionVotedFor));
        }

        [Test]
        public void Remove_A_Session_From_The_Cookie_If_It_Had_Been_Voted_For()
        {
            Controller.RemoveVote(SecondSessionVotedFor);

            CurrentUserVoteRepository.Received()
                .Delete(Arg.Is<int>(SecondSessionVotedFor));
        }
    }
}