using System;

namespace DDDEastAnglia.DataAccess
{
    public class CachingVotingCookieFactory : IVotingCookieFactory
    {
        private readonly IVotingCookieFactory votingCookieFactory;
        private VotingCookie votingCookie;

        public CachingVotingCookieFactory(IVotingCookieFactory votingCookieFactory)
        {
            if (votingCookieFactory == null)
            {
                throw new ArgumentNullException("votingCookieFactory");
            }
            
            this.votingCookieFactory = votingCookieFactory;
        }

        public VotingCookie Create()
        {
            return votingCookie ?? (votingCookie = votingCookieFactory.Create());
        }
    }
}
