using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorRepository
    {
        IEnumerable<Sponsor> GetAllSponsors();
    }
}
