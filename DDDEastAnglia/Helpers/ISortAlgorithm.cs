using DDDEastAnglia.Models;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public interface ISortAlgorithm
    {
        void Sort(List<SessionDisplayModel> sessions);
    }
}