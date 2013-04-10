using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Models
{
    public class VotingCookie
    {
        public const string CookieName = "DDDEA2013.Voting";
        private static readonly DateTime DefaultExpiry = new DateTime(2013, 4, 30);
        private readonly List<int> _sessionsVotedFor = new List<int>();

        public VotingCookie(string name)
            : this(name, DefaultExpiry)
        {
            
        }

        public VotingCookie(string name, DateTime expires)
        {
            Name = name;
            Expires = expires;
        }

        public VotingCookie(string name, IEnumerable<int> sessionsVotedFor, DateTime expires) 
            : this(name, expires)
        {
            _sessionsVotedFor.AddRange(sessionsVotedFor);
        }

        public string Name { get; set; }
        public DateTime Expires { get; set; }
        public IEnumerable<int> SessionsVotedFor { get { return _sessionsVotedFor.AsReadOnly(); } }


        public bool Contains(int sessionId)
        {
            return _sessionsVotedFor.Contains(sessionId);
        }

        public void Add(int sessionId)
        {
            if (_sessionsVotedFor.Contains(sessionId))
            {
                return;
            }
            _sessionsVotedFor.Add(sessionId);
        }

        public void Remove(int sessionId)
        {
            if (!_sessionsVotedFor.Contains(sessionId))
            {
                return;
            }
            _sessionsVotedFor.Remove(sessionId);
        }
    }
}