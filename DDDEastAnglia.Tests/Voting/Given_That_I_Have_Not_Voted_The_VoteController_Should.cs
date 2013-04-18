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
    public class Given_That_I_Have_Not_Voted_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;
        private const int UnknownSessionId = 10;
        private IControllerInformationProvider _controllerInformationProvider;
        private static readonly Guid _cookieGuid = Guid.NewGuid();
        private static readonly HttpCookie _cookie = new HttpCookie(VotingCookie.CookieName, _cookieGuid.ToString());

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            _controllerInformationProvider = controllerInformationProvider;
            _controllerInformationProvider.GetCookie(Arg.Is<string>(s => s== VotingCookie.CookieName)).Returns(_cookie);
        }


        [Test]
        public void Register_A_Vote_For_A_Session()
        {
           Controller.RegisterVote(KnownSessionId);

           MessageBus.Received()
                      .Send(Arg.Is<RegisterVoteCommand>(command => command.SessionId == KnownSessionId));
        }

        [Test]
        public void Set_A_Cookie_When_Trying_To_Remove_A_Session()
        {
            Controller.RemoveVote(KnownSessionId);
            _controllerInformationProvider.Received()
                    .SaveCookie(Arg.Is<HttpCookie>(cookie => cookie.Value == _cookieGuid.ToString()));
        }

        [Test]
        public void Set_An_Empty_Cookie_When_Trying_To_Add_An_Unknown_Session()
        {
            Controller.RegisterVote(UnknownSessionId);
            _controllerInformationProvider.Received()
                    .SaveCookie(Arg.Is<HttpCookie>(cookie => cookie.Value == _cookieGuid.ToString()));
        }

        [Test]
        public void Save_The_Vote_To_The_Database()
        {
            Controller.RegisterVote(KnownSessionId);
            MessageBus.Received()
                          .Send(Arg.Is<RegisterVoteCommand>(command => command.SessionId == KnownSessionId));
        }

    }
}
