using System;

namespace DDDEastAnglia
{
    public sealed class VotingCookie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Expiry { get; set; }
    }
}
