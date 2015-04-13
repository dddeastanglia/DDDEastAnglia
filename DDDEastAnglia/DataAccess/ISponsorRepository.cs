using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorRepository
    {
        IEnumerable<Sponsor> GetAllSponsors();
        void AddSponsor(Sponsor sponsor);
        Sponsor GetSponsor(int sponsorId);
        void DeleteSponsor(int sponsorId);
    }
}
