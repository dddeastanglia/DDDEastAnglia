using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SubmittedSessionProfileFilter : IUserProfileFilter
    {
        private readonly IDDDEAContext context;

        public SubmittedSessionProfileFilter(IDDDEAContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            this.context = context;
        }

        public IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles)
        {
            return profiles.Where(profile => context.Sessions.Any(s => s.SpeakerUserName == profile.UserName));
        }
    }
}
