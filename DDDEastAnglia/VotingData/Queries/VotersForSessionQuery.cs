using System;
using System.Data;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class VotersForSessionQuery : IQuery<SessionVoterModel>
    {
        private readonly GravatarUrl gravatar;
        private readonly int sessionId;

        public VotersForSessionQuery(GravatarUrl gravatar, int sessionId)
        {
            if (gravatar == null)
            {
                throw new ArgumentNullException("gravatar");
            }

            this.gravatar = gravatar;
            this.sessionId = sessionId;
        }

        public string Sql => $@"SELECT V.CookieId, U.Name, U.EmailAddress
FROM Votes V
LEFT OUTER JOIN UserProfiles U
ON V.UserId = U.UserId
WHERE V.sessionid = {sessionId}";

        public IQueryResultObjectFactory<SessionVoterModel> ObjectFactory => new SessionVotersModelFactory(gravatar);

        private class SessionVotersModelFactory : IQueryResultObjectFactory<SessionVoterModel>
        {
            private readonly GravatarUrl gravatar;

            public SessionVotersModelFactory(GravatarUrl gravatar)
            {
                if (gravatar == null)
                {
                    throw new ArgumentNullException("gravatar");
                }

                this.gravatar = gravatar;
            }

            public SessionVoterModel Create(IDataReader reader)
            {
                Guid cookieId = reader.GetGuid(reader.GetOrdinal("CookieId"));
                int userNameField = reader.GetOrdinal("Name");
                string userName = reader.IsDBNull(userNameField) ? null : reader.GetString(userNameField);
                int emailAddressField = reader.GetOrdinal("EmailAddress");
                string emailAddress = reader.IsDBNull(emailAddressField) ? null : reader.GetString(emailAddressField);

                string userIdentifier = userName ?? cookieId.ToString();
                string gravatarUrl = emailAddress == null
                                        ? gravatar.GetUrl(cookieId.ToString(), useIdenticon: true)
                                        : gravatar.GetUrl(emailAddress);

                return new SessionVoterModel
                {
                    IsAnonymous = userName == null,
                    UserIdentifier = userIdentifier,
                    GravatarUrl = gravatarUrl
                };
            }
        }
    }
}
