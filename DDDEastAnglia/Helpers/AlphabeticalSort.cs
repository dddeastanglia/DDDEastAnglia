using DDDEastAnglia.Controllers;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public class AlphabeticalSort : ISortAlgorithm
    {
        public virtual void Sort(List<Models.SessionDisplayModel> sessions)
        {
            sessions.Sort(new SessionDisplayModelComparer());
        }
    }
}