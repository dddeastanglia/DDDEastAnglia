﻿using System;
using System.Web;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_The_Screen_And_Width_Are_Specified_When_Posting_The_Data_The_VoteController_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const int SessionIdToRemove = 2;

        private readonly HttpCookie _httpCookie = new HttpCookie(VotingCookie.CookieName, CookieId.ToString());
        private static readonly Guid CookieId = Guid.NewGuid();

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(_httpCookie);
        }

        [Test]
        public void Save_The_Screen_Resolution_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 1024, Height = 768});
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.ScreenResolution == "1024x768"));
        }

        [Test]
        public void Save_The_Height_If_The_Width_Is_Zero_With()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 0, Height = 768});
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.ScreenResolution == "0x768"));
        }

        [Test]
        public void Save_The_Width_If_The_Height_Is_Zero_With()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel { Width = 1024, Height = 0 });
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.ScreenResolution == "1024x0"));
        }

        [Test]
        public void Save_Nothing_If_The_Width_And_Height_Zero()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 0, Height = 0});
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.ScreenResolution == null));
        }
    }
}