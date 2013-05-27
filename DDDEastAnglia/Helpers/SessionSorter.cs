using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public class SessionSorter : ISessionSorter
    {
        private readonly ISortAlgorithm _defaultSort;
        private readonly ISortAlgorithm _votingSort;

        public SessionSorter(ISortAlgorithm defaultSort, ISortAlgorithm votingSort)
        {
            _defaultSort = defaultSort;
            _votingSort = votingSort;
        }

        public void SortSessions(IConference conference, List<SessionDisplayModel> sessions)
        {
            if (conference.CanVote())
            {
                _votingSort.Sort(sessions);
            }
            else
            {
                _defaultSort.Sort(sessions);
            }
        }
    }
}