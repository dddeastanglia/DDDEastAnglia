using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorSorter
    {
        IEnumerable<Sponsor> Sort(IEnumerable<Sponsor> sponsors);
    }
}
