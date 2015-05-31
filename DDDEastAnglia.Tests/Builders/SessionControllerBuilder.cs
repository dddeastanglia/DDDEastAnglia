using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email;
using NSubstitute;

namespace DDDEastAnglia.Tests.Builders
{
    internal class SessionControllerBuilder
    {
        private readonly IConferenceLoader conferenceLoader = Substitute.For<IConferenceLoader>();
        private readonly IUserProfileRepository userProfileRepository = Substitute.For<IUserProfileRepository>();
        private readonly ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        private readonly ISessionSorter sessionSorter = Substitute.For<ISessionSorter>();
        private IPostman postman = Substitute.For<IPostman>();
        private UserProfile user;

        public SessionControllerBuilder WithPostman(IPostman newPostman)
        {
            postman = newPostman;
            return this;
        }

        public SessionControllerBuilder WhenSubmissionsAreOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanSubmit().Returns(true);
            conferenceLoader.LoadConference().Returns(conference);

            return this;
        }

        public SessionControllerBuilder ForUser(UserProfile newUser)
        {
            user = newUser;
            userProfileRepository.GetUserProfileByUserName(newUser.UserName).Returns(newUser);

            return this;
        }

        public SessionControllerBuilder Submitting(Session session)
        {
            sessionRepository.AddSession(session).Returns(session);
            return this;
        }

        public SessionControllerBuilder Updating(Session session)
        {
            sessionRepository.Get(session.SessionId).Returns(session);
            return this;
        }

        public SessionController Build()
        {
            var sessionController = new SessionController(
                conferenceLoader,
                userProfileRepository,
                sessionRepository,
                sessionSorter,
                new EmailMessengerFactory(postman));

            sessionController.SetupWithAuthenticatedUser(user);

            return sessionController;
        }
    }
}