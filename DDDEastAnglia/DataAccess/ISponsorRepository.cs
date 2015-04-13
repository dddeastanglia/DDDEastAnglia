using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorRepository
    {
        IEnumerable<Sponsor> GetAllSponsors();
        Sponsor GetSponsor(int sponsorId);
        void AddSponsor(Sponsor sponsor);
        void DeleteSponsor(int sponsorId);
    }
}
