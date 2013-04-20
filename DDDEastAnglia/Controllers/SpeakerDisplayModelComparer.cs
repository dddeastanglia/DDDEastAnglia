using System;
using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SpeakerDisplayModelComparer : IComparer<SpeakerDisplayModel>
    {
        public int Compare(SpeakerDisplayModel x, SpeakerDisplayModel y)
        {
            if (x.IsCurrentUser && !y.IsCurrentUser)
            {
                return -1;
            }

            if (!x.IsCurrentUser && y.IsCurrentUser)
            {
                return 1;
            }

            return string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
