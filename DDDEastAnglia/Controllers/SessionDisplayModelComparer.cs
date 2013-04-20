using System;
using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SessionDisplayModelComparer : IComparer<SessionDisplayModel>
    {
        public int Compare(SessionDisplayModel x, SessionDisplayModel y)
        {
            if (x.IsUsersSession && !y.IsUsersSession)
            {
                return -1;
            }

            if (!x.IsUsersSession && y.IsUsersSession)
            {
                return 1;
            }

            return string.Compare(x.SessionTitle, y.SessionTitle, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
