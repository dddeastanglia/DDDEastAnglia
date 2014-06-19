using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISpeakerRepository
    {
        IEnumerable<SpeakerProfile> GetAllSpeakerProfiles();
    }
}
