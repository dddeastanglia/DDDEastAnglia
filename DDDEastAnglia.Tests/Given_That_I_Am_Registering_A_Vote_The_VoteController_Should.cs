using System;
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
    public class Given_That_I_Am_Registering_A_Vote_The_VoteController_Should
    {
        private const int KnownSessionId = 1;
        private IVotingCookieRepository cookieRepository;
        private IVoteRepository voteRepository;
        private ISessionRepository sessionRepository;
        private IEventRepository eventRepository;
        private VotingCookie cookieWithNoVotes;
        private VoteController controller;
        private ITimeProvider timeProvider;
        private DateTime simulatedNow;

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

            timeProvider = Substitute.For<ITimeProvider>();
            timeProvider.UtcNow.Returns(simulatedNow);

            controller = new VoteController(cookieRepository, voteRepository, sessionRepository, eventRepository, timeProvider);
        }

        [Test]
        public void Record_The_EventID()
        {
            controller.RegisterVote(KnownSessionId);

            voteRepository.Received().Save(Arg.Is<Vote>(vote => vote.Event == "DDDEA2013"));
        }

        [Test]
        public void Record_The_SessionId()
        {
            controller.RegisterVote(KnownSessionId);

            voteRepository.Received().Save(Arg.Is<Vote>(vote => vote.SessionId == KnownSessionId));
        }

        [Test]
        public void Record_The_Cookie_Guid()
        {
            controller.RegisterVote(KnownSessionId);

            voteRepository.Received().Save(Arg.Is<Vote>(vote => vote.CookieId.Equals(cookieWithNoVotes.Id)));
        }

        [Test]
        public void Record_The_Time_Of_The_Vote()
        {
            controller.RegisterVote(1);

            voteRepository.Received().Save(Arg.Is<Vote>(vote => vote.TimeRecorded == simulatedNow));
        }

        [Test]
        public void Record_That_The_Vote_Is_Actually_A_Vote()
        {
            controller.RegisterVote(1);

            voteRepository.Received().Save(Arg.Is<Vote>(vote => vote.IsVote));
        }
    }
}