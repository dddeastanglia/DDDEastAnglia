using DDDEastAnglia.Models;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public class RandomSort :ISortAlgorithm
    {
        public virtual void Sort(List<SessionDisplayModel> sessions)
        {
            sessions.RandomShuffle();
        }
    }
}