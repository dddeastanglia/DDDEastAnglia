using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Helpers
{
    public interface ISortAlgorithm
    {
        void Sort(List<SessionDisplayModel> sessions);
    }
}