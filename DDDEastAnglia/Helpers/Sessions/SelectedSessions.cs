using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDEastAnglia.Helpers.Sessions
{
    public static class SelectedSessions
    {
        private static readonly List<Tuple<int, int>> SelectedSessionsData = new List<Tuple<int, int>>
        {
            // session 1
            Tuple.Create(2264, 80),
            Tuple.Create(2247, 33),
            Tuple.Create(2246, 8895),
            Tuple.Create(2206, 8870),

            // session 2
            Tuple.Create(2230, 8885),
            Tuple.Create(2269, 8796),
            Tuple.Create(2228, 8872),
            Tuple.Create(2232, 8887),

            // session 3
            Tuple.Create(2220, 30),
            Tuple.Create(2196, 8867),
            Tuple.Create(2256, 8901),
            Tuple.Create(2201, 8786),

            // session 4
            Tuple.Create(2267, 53),
            Tuple.Create(2189, 133),
            Tuple.Create(2190, 6506),
            Tuple.Create(2198, 8868),

            // session 5
            Tuple.Create(2207, 2314),
            Tuple.Create(2257, 8902),
            Tuple.Create(2240, 8891),
            Tuple.Create(2258, 8864),

            // session 6
            Tuple.Create(2219, 8876),
            Tuple.Create(2254, 8693),
            Tuple.Create(2194, 8801),
            Tuple.Create(2199, 8869)
        };

        public static IEnumerable<int> SessionIds => SelectedSessionsData.Select(s => s.Item1);

        public static IEnumerable<int> SpeakerIds => SelectedSessionsData.Select(s => s.Item2);
    }
}
