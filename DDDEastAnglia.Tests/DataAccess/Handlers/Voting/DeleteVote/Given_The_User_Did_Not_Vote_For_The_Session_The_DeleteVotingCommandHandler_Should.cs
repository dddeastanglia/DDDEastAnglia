﻿using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using NSubstitute;
using NUnit.Framework;
using Conference = DDDEastAnglia.Domain.Conference;

namespace DDDEastAnglia.Tests.DataAccess.Handlers.Voting.DeleteVote
{
    [TestFixture]
    public class Given_The_User_Did_Not_Vote_For_The_Session_The_DeleteVotingCommandHandler_Should
    {
        private const int SessionId = 1;
        private static readonly Guid CookieId = Guid.NewGuid();
        private DeleteVoteCommandHandler _handler; 
        private IVoteRepository _voteRepository;
        private IConferenceLoader _conferenceLoader;

        [SetUp]
        public void BeforeEachTest()
        {
            _voteRepository = Substitute.For<IVoteRepository>();
            _voteRepository.HasVotedFor(Arg.Is(SessionId), Arg.Is(CookieId)).Returns(true);

            _conferenceLoader = Substitute.For<IConferenceLoader>();
            var conference = new Conference(1, "", "");
            conference.AddToCalendar(ConferenceHelper.GetOpenVotingPeriod());
            _conferenceLoader.LoadConference(Arg.Is(1)).Returns(conference);

            _handler = new DeleteVoteCommandHandler(_voteRepository, _conferenceLoader);
        }

        [Test]
        public void Correctly_Delete_The_Vote()
        {
            _handler.Handle(new DeleteVoteCommand
                {
                    CookieId = CookieId,
                    SessionId = SessionId
                });

            _voteRepository.Received().Delete(Arg.Is(SessionId), Arg.Is(CookieId));
        }
    }
}