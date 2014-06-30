using System;

namespace DDDEastAnglia
{
    public sealed class VotingCookie
    {
        public string Name { get { return name; } }
        private readonly string name;

        public DateTime Expiry { get { return expiry; } }
        private readonly DateTime expiry;

        public VotingCookie(string name, DateTime expiry)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            this.name = name;
            this.expiry = expiry;
        }
    }
}
