using System;
using System.Web;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_The_IPAddress_Is_Available_The_VoteController_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;
        private const string LocalIpAddress = "127.0.0.1";
        private readonly HttpCookie _httpCookie = new HttpCookie(VotingCookie.CookieName, CookieId.ToString());
        private static readonly Guid CookieId = Guid.NewGuid();

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(_httpCookie);
            controllerInformationProvider.GetIPAddress().Returns(LocalIpAddress);
        }

        [Test]
        public void Record_The_IPAddress_With_When_Creating_A_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);

            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.IPAddress == LocalIpAddress));
        }
    }
}