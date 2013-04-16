using DDDEastAnglia.Controllers;
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

        protected override void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
        {
            base.SetCookieRepositoryExpectations(repository);
            repository.HasVotedFor(SessionIdToRemove).Returns(true);
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

        [Test]
        public void Save_The_Screen_Resolution_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 1024, Height = 768});
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "1024x768"));
        }

        [Test]
        public void Save_The_Height_If_The_Width_Is_Zero_With()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 0, Height = 768});
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "0x768"));
        }

        [Test]
        public void Save_The_Width_If_The_Height_Is_Zero_With()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel { Width = 1024, Height = 0 });
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == "1024x0"));
        }

        [Test]
        public void Save_Nothing_If_The_Width_And_Height_Zero()
        {
            Controller.RegisterVote(SessionIdToVoteFor, new VoteModel {Width = 0, Height = 0});
            CurrentUserVoteRepository.Received().Save(Arg.Is<Vote>(vote => vote.ScreenResolution == null));
        }
    }
}