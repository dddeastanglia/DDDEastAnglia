using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess
{
    [TestFixture]
    public sealed class SpeakerRepositoryTests
    {
        [Test]
        public void GetAllSpeakerProfiles_DoesNotReturnAUserWhoHasNotSubmittedASession()
        {
            var sessionRespository = new SessionRepositoryBuilder().Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").Build();
            var speakerRepository = new SpeakerRepository(sessionRespository, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers, Is.Empty);
        }

        [Test]
        public void GetAllSpeakerProfiles_ReturnsAUserWhoHasSubmittedASession()
        {
            var sessionRespository = new SessionRepositoryBuilder().WithSessionSubmittedBy("fred").Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").Build();
            var speakerRepository = new SpeakerRepository(sessionRespository, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers.Select(s => s.UserName), Is.EqualTo(new[] {"fred"}));
        }

        [Test]
        public void GetAllSpeakerProfiles_ReturnsOnlyUsersWhoHaveSubmittedSessions()
        {
            var sessionRespository = new SessionRepositoryBuilder().WithSessionSubmittedBy("fred").Build();
            var userProfileRepository = new UserProfileRepositoryBuilder().WithUser("fred").WithUser("bob").Build();
            var speakerRepository = new SpeakerRepository(sessionRespository, userProfileRepository);

            var allSpeakers = speakerRepository.GetAllSpeakerProfiles();

            Assert.That(allSpeakers.Select(s => s.UserName), Is.EqualTo(new[] {"fred"}));
        }

        private class SessionRepositoryBuilder
        {
            private readonly List<Session> sessions = new List<Session>();

            public SessionRepositoryBuilder WithSessionSubmittedBy(string username)
            {
                sessions.Add(new Session {SpeakerUserName = username});
                return this;
            }

            public ISessionRepository Build()
            {
                var sessionRespository = Substitute.For<ISessionRepository>();
                sessionRespository.GetAllSessions().Returns(sessions);
                return sessionRespository;
            }
        }

        private class UserProfileRepositoryBuilder
        {
            private readonly List<UserProfile> userProfiles = new List<UserProfile>();

            public UserProfileRepositoryBuilder WithUser(string username)
            {
                userProfiles.Add(new UserProfile {UserName = username});
                return this;
            }

            public IUserProfileRepository Build()
            {
                var userProfileRepsoitory = Substitute.For<IUserProfileRepository>();
                userProfileRepsoitory.GetAllUserProfiles().Returns(userProfiles);
                return userProfileRepsoitory;
            }
        }
    }
}
