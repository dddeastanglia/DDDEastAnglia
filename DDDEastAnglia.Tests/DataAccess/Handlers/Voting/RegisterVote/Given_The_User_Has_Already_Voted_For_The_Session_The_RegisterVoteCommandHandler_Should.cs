﻿using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using DDDEastAnglia.Domain;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess.Handlers.Voting.RegisterVote
{
    [TestFixture]
    public class Given_The_User_Has_Already_Voted_For_The_Session_The_RegisterVoteCommandHandler_Should
    {
        private const int SessionId = 1;
        private static readonly Guid CookieId = Guid.NewGuid();
        private readonly DateTime _simulatedNow = DateTime.Now;
        private RegisterVoteCommandHandler _handler;
        private IVoteRepository _voteRepository;
        private IConferenceRepository _conferenceRepository;

        [SetUp]
        public void BeforeEachTest()
        {
            _voteRepository = Substitute.For<IVoteRepository>();
            _voteRepository.HasVotedFor(Arg.Any<int>(), Arg.Any<Guid>()).Returns(true);

            _conferenceRepository = Substitute.For<IConferenceRepository>();
            var conference = new Conference(SessionId, "", "");
            conference.ReadyForVoting();
            _conferenceRepository.ForSession(Arg.Is(1)).Returns(conference);

            _handler = new RegisterVoteCommandHandler(_voteRepository, _conferenceRepository);
        }

        [Test]
        public void Not_Save_The_Vote()
        {
            _handler.Handle(new RegisterVoteCommand
                {
                    CookieId = CookieId,
                    SessionId = SessionId,
                    TimeRecorded = _simulatedNow
                });

            _voteRepository.DidNotReceiveWithAnyArgs().Save(null);
        }
    }
}