using System;
using DDDEastAnglia.DataAccess;
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
        private const string UserAgent = "A Browser";
        private const string Referer = "http://www.referer.com";
        private static readonly int[] CurrentSessionIds = new[] { SessionIdToRemove };
        private VotingCookie cookieWithOneVote;

        protected override void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            cookieWithOneVote = new VotingCookie(Guid.NewGuid(), VotingCookie.CookieName, CurrentSessionIds, new DateTime(2013, 4, 30));
            repository.Get(Arg.Is(cookieWithOneVote.Name))
                .Returns(cookieWithOneVote);
        }

        protected override void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
            base.SetSessionRepositoryExpectations(sessionRepository);
            sessionRepository.Exists(Arg.Is(SessionIdToVoteFor)).Returns(true);
        }

        protected override void SetRequestInformationProviderExpectations(IRequestInformationProvider requestInformationProvider)
        {
            base.SetRequestInformationProviderExpectations(requestInformationProvider);
            requestInformationProvider.UserAgent.Returns(UserAgent);
            requestInformationProvider.Referrer.Returns(Referer);
        }

        //[Test]
        //public void Save_The_Screen_Resolution_With_The_Vote()
        //{
        //    Controller.RegisterVote(SessionIdToVoteFor, 1024, 768);
        //    VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "1024x768"));
        //}

        //[Test]
        //public void Save_The_Height_If_The_Width_Is_Zero_With()
        //{
        //    Controller.RegisterVote(SessionIdToVoteFor, 0, 768);
        //    VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "0x768"));
        //}

        //[Test]
        //public void Save_The_Width_If_The_Height_Is_Zero_With()
        //{
        //    Controller.RegisterVote(SessionIdToVoteFor, 1024, 0);
        //    VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "1024x0"));
        //}

        //[Test]
        //public void Save_Nothing_If_The_Width_And_Height_Zero()
        //{
        //    Controller.RegisterVote(SessionIdToVoteFor, 0, 0);
        //    VoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == null));
        //}
    }
}