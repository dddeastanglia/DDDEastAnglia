using DDDEastAnglia.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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