using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public interface ISessionSorter
    {
        void SortSessions(IConference conference, List<SessionDisplayModel> sessions);
    }
}