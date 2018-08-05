using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Agenda
{
    public sealed class AgendaSessionsLoader
    {
        private readonly ISessionLoader sessionLoader;
        private readonly ISpeakerRepository speakerRepository;

        public AgendaSessionsLoader(ISessionLoader sessionLoader, ISpeakerRepository speakerRepository)
        {
            if (sessionLoader == null)
            {
                throw new ArgumentNullException("sessionLoader");
            }

            if (speakerRepository == null)
            {
                throw new ArgumentNullException("speakerRepository");
            }


            this.sessionLoader = sessionLoader;
            this.speakerRepository = speakerRepository;
        }

        public IEnumerable<AgendaSession> GetSelectedSessions()
        {
            var allSessions = sessionLoader.LoadSessions();
            var speakerIdMap = GetSpeakerIdMap();

            return allSessions.Select(s =>
            {
                var speakerProfile = speakerIdMap[s.SpeakerUserName];

                return new AgendaSession
                {
                    SessionId = s.SessionId,
                    SessionTitle = s.Title,
                    SpeakerUserId = speakerProfile.UserId,
                    SpeakerName = speakerProfile.Name
                };
            });
        }

        private Dictionary<string, SpeakerProfile> GetSpeakerIdMap()
        {
            var speakerProfiles = speakerRepository.GetAllSpeakerProfiles();
            return speakerProfiles.ToDictionary(p => p.UserName, p => p);
        }
    }
}
