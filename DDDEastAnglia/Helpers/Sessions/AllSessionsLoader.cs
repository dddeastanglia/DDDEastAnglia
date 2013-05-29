using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class AllSessionsLoader : ISessionLoader
    {
        private readonly IDDDEAContext db;

        public AllSessionsLoader(IDDDEAContext db)
        {
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }
            
            this.db = db;
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            
            return db.Sessions.Where(s => s.SpeakerUserName == profile.UserName);
        }
    }
}
