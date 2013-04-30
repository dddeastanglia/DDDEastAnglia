using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Helpers
{
    public interface ISessionSorter
    {
        void SortSessions(Conference conference, List<SessionDisplayModel> sessions);
    }
}