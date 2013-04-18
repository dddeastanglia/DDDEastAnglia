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
    public class Given_That_I_Am_Logged_In_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int MyUserId = 100;
        private const string MyUserName = "Bob";

        private readonly HttpCookie _httpCookie = new HttpCookie(VotingCookie.CookieName, CookieId.ToString());
        private static readonly Guid CookieId = Guid.NewGuid();

        private readonly UserProfile _myUserProfile = new UserProfile
            {
                UserId = MyUserId,
                UserName = MyUserName
            };


        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(_httpCookie);
            controllerInformationProvider.IsLoggedIn().Returns(true);
            controllerInformationProvider.GetCurrentUser().Returns(_myUserProfile);
        }

        [Test]
        public void Save_My_UserId_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.UserId == _myUserProfile.UserId));
        }
    }
}