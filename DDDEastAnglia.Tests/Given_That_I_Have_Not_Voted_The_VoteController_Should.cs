using System.Collections.Generic;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests
{
    [TestFixture]
    public class Given_That_I_Have_Not_Voted_The_VoteController_Should
    {
        private const int KnownSessionId = 1;
        private const int UnknownSessionId = 10;
        private static readonly int[] SessionIdsVotedFor = new [] { KnownSessionId };
        private static readonly int[] NoSessionIdsVotedFor = new int[0];
        private IVotingCookieRepository cookieRepository;
        private IVoteRepository voteRepository;
        private ISessionRepository sessionRepository;
        private IEventRepository eventRepository;
        private VotingCookie cookieWithNoVotes;
        private VoteController controller;

        [SetUp]
        public void BeforeEachTest()
        {
            cookieRepository = Substitute.For<IVotingCookieRepository>();
            cookieWithNoVotes = new VotingCookie(VotingCookie.CookieName);
            cookieRepository.Get(Arg.Is(cookieWithNoVotes.Name))
                .Returns(cookieWithNoVotes);

            voteRepository = Substitute.For<IVoteRepository>();

            eventRepository = Substitute.For<IEventRepository>();
            eventRepository.Get(Arg.Is("DDDEA2013")).Returns(EventHelper.BuildEvent(true, true));

            sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepository.Exists(Arg.Is(KnownSessionId)).Returns(true);
            sessionRepository.Exists(Arg.Is(UnknownSessionId)).Returns(false);

            controller = new VoteController(cookieRepository, voteRepository, sessionRepository, eventRepository);
        }

        [Test]
        public void Register_A_Vote_For_A_Session()
        {
           controller.RegisterVote(KnownSessionId);

           cookieRepository.Received()
                      .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, SessionIdsVotedFor)));
        }

        [Test]
        public void Not_Set_A_Cookie_When_Trying_To_Remove_A_Session()
        {
            controller.RemoveVote(KnownSessionId);
            cookieRepository.DidNotReceive()
                    .Save(Arg.Is<VotingCookie>(cookie => cookie.IsCorrect(VotingCookie.CookieName, NoSessionIdsVotedFor)));
        }

        [Test]
        public void Set_An_Empty_Cookie_When_Trying_To_Add_An_Unknown_Session()
        {
            controller.RegisterVote(UnknownSessionId);
            cookieRepository.DidNotReceiveWithAnyArgs()
                    .Save(null);
        }

        [Test]
        public void Save_The_Vote_To_The_Database()
        {
            controller.RegisterVote(KnownSessionId);
            voteRepository.Received()
                          .Save(Arg.Is<Vote>(vote => vote.IsVoteFor("DDDEA2013", KnownSessionId)));
        }

        [Test]
        public void Fail_To_Register_A_Vote_In_The_Database_If_Session_Is_Not_Known()
        {
            controller.RegisterVote(UnknownSessionId);
            voteRepository.DidNotReceiveWithAnyArgs().Save(null);
        }
    }
}
