using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserProfileRepository userProfileRepository;

        public SpeakerRepository(ISessionRepository sessionRepository, IUserProfileRepository userProfileRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            if (userProfileRepository == null)
            {
                throw new ArgumentNullException(nameof(userProfileRepository));
            }

            this.sessionRepository = sessionRepository;
            this.userProfileRepository = userProfileRepository;
        }

        public IEnumerable<SpeakerProfile> GetAllSpeakerProfiles()
        {
            var allSessions = sessionRepository.GetAllSessions();
            var sessionCountGroupedBySpeaker = allSessions.GroupBy(s => s.SpeakerUserName)
                                                          .Where(g => g.Any())
                                                          .ToDictionary(g => g.Key, g => g.Count());

            var usersWithSessions = new List<SpeakerProfile>();
            var allUserProfiles = userProfileRepository.GetAllUserProfiles();

            foreach (UserProfile userProfile in allUserProfiles)
            {
                int sessionCount;

                if (sessionCountGroupedBySpeaker.TryGetValue(userProfile.UserName, out sessionCount)
                        && sessionCount > 0)
                {
                    var speakerProfile = createSpeakerProfile(userProfile, sessionCount);
                    usersWithSessions.Add(speakerProfile);
                }
            }

            return usersWithSessions;
        }

        private SpeakerProfile createSpeakerProfile(UserProfile userProfile, int numberOfSubmittedSessions)
        {
            return new SpeakerProfile
            {
                UserId = userProfile.UserId,
                UserName = userProfile.UserName,
                Name = userProfile.Name,
                EmailAddress = userProfile.EmailAddress,
                NewSpeaker = userProfile.NewSpeaker,
                NumberOfSubmittedSessions = numberOfSubmittedSessions
            };
        }
    }
}
