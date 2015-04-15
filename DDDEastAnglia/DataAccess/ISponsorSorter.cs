using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorSorter
    {
        IEnumerable<Sponsor> Sort(IEnumerable<Sponsor> sponsors);
    }
}
