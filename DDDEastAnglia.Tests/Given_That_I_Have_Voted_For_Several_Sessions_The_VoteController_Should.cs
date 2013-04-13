using System;
using System.Collections.Generic;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests
{
    [TestFixture]
    public class Given_That_I_Have_Voted_For_Several_Sessions_The_VoteController_Should
    {
        private const int FirstSessionVotedFor = 1;
        private const int SecondSessionVotedFor = 2;
        private const int SessionNotVotedFor = 3;
        private const int UnknownSessionId = 10;
        private static readonly int[] OnlySessionOne = new[] {FirstSessionVotedFor};
        private static readonly int[] SessionIdsVotedFor = new[] { FirstSessionVotedFor, SecondSessionVotedFor };
        private static readonly int[] SessionsExpectedAfterReceivingVote = new[] { FirstSessionVotedFor, SecondSessionVotedFor, SessionNotVotedFor };
        private static readonly int[] NoSessionIdsVotedFor = new int[0];
        private IVotingCookieRepository cookieRepository;
        private IVoteRepository voteRepository;
        private ISessionRepository sessionRepository;
        private IEventRepository eventRepository;
        private VotingCookie cookieWithTwoVotes;
        private VoteController controller;

        [SetUp]
        public void SetUp()
        {
            cookieRepository = Substitute.For<IVotingCookieRepository>();
            cookieWithTwoVotes = new VotingCookie(Guid.NewGuid(), VotingCookie.CookieName, SessionIdsVotedFor, new DateTime(2013, 4, 30));
            cookieRepository.Get(Arg.Is(cookieWithTwoVotes.Name))
                .Returns(cookieWithTwoVotes);

            voteRepository = Substitute.For<IVoteRepository>();

            eventRepository = Substitute.For<IEventRepository>();
            eventRepository.Get(Arg.Is("DDDEA2013")).Returns(EventHelper.BuildEvent(true, true));

            sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepository.Exists(Arg.Is(FirstSessionVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(SecondSessionVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(SessionNotVotedFor)).Returns(true);
            sessionRepository.Exists(Arg.Is(UnknownSessionId)).Returns(false);

            controller = new VoteController(cookieRepository, voteRepository, sessionRepository, eventRepository, new TimeProvider());
        }

        [Test]
        public void Record_A_Vote_For_A_Session_That_I_Have_Not_Voted_For()
        {
            controller.RegisterVote(SessionNotVotedFor);

            cookieRepository.Received()
                      .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, SessionsExpectedAfterReceivingVote)));
            voteRepository.Received()
                      .Save(Arg.Is<Vote>(vote => vote.IsVoteFor("DDDEA2013", SessionNotVotedFor)));
        }

        [Test]
        public void Not_Record_A_Vote_If_The_User_Has_Already_Voted_For_The_Session()
        {
            controller.RegisterVote(SecondSessionVotedFor);

            cookieRepository.DidNotReceive()
                      .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, SessionsExpectedAfterReceivingVote)));
            voteRepository.DidNotReceive()
                      .Save(Arg.Is<Vote>(vote => vote.IsVoteFor("DDDEA2013", SessionNotVotedFor)));
        }

        [Test]
        public void Remove_A_Session_From_The_Cookie_If_It_Had_Been_Voted_For()
        {
            controller.RemoveVote(SecondSessionVotedFor);

            cookieRepository.Received()
                .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, OnlySessionOne)));
        }

        [Test]
        public void Record_That_A_Vote_Has_Been_Removed()
        {
            controller.RemoveVote(SecondSessionVotedFor);

            voteRepository.Received()
                .Save(Arg.Is<Vote>(vote => vote.VoteHasBeenRemoved("DDDEA2013", SecondSessionVotedFor)));
        }
    }
}