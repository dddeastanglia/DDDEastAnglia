using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Tests.Voting
{
    public static class CookieHelper
    {
        public static bool IsCorrect(this VotingCookie cookie, string expectedCookieName, IEnumerable<int> sessionIdsToExpect)
        {
            // deliberately keeping these separate, easier to debug
            if (!cookie.Name.Equals(expectedCookieName))
            {
                return false;
            }
            if (!cookie.SessionsVotedFor.All(sessionIdsToExpect.Contains))
            {
                return false;
            }
            return true;
        }
    }
}