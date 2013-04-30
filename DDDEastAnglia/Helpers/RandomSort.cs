using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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