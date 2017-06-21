using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests.Helpers
{
    public class Given_A_List_Of_Sessions
    {
        private AlphabeticalSort _alphabeticalAlgorithm;
        private RandomSort _randomAlgorithm;
        private SessionSorter _sorter;
        private List<SessionDisplayModel> _sessions;

        [SetUp]
        public void BeforeEachTest()
        {
            _alphabeticalAlgorithm = Substitute.For<AlphabeticalSort>();
            _randomAlgorithm = Substitute.For<RandomSort>();
            _sorter = new SessionSorter(_alphabeticalAlgorithm, _randomAlgorithm);
            _sessions = new List<SessionDisplayModel>
            {
                new SessionDisplayModel(),
                new SessionDisplayModel(),
                new SessionDisplayModel()
            };
        }

        [Test]
        public void And_A_Conference_In_Voting_Then_Sorting_Should_Be_Random()
        {
            var conference = Substitute.For<IConference>();
            conference.CanVote().Returns(true);

            _sorter.SortSessions(conference, _sessions);

            _alphabeticalAlgorithm.DidNotReceive().Sort(Arg.Any<List<SessionDisplayModel>>());
            _randomAlgorithm.Received(1).Sort(Arg.Any<List<SessionDisplayModel>>());
        }

        [Test]
        public void And_A_Conference_Not_In_Voting_Then_Sorting_Should_Be_Alphabetical()
        {
            var conference = Substitute.For<IConference>();
            conference.CanVote().Returns(false);

            _sorter.SortSessions(conference, _sessions);

            _alphabeticalAlgorithm.Received(1).Sort(Arg.Any<List<SessionDisplayModel>>());
            _randomAlgorithm.DidNotReceive().Sort(Arg.Any<List<SessionDisplayModel>>());
        }
    }
}
