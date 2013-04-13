using System.Linq;
using System.Web;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpContextUserProvider : IUserProvider
    {
        private DDDEAContext context = new DDDEAContext();

        public bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name);
        }

        public UserProfile GetCurrentUser()
        {
            return context.UserProfiles.FirstOrDefault(profile => profile.UserName == HttpContext.Current.User.Identity.Name);
        }
    }
}