using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SelectedSessionsLoader : ISessionLoader
    {
        private readonly IDDDEAContext context;

        public SelectedSessionsLoader(IDDDEAContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            this.context = context;
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            
            return context.Sessions.Where(s => s.SpeakerUserName == profile.UserName
                                                && SelectedSessions.SessionIds.Contains(s.SessionId));
        }
    }
}
