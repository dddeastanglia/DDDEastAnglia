using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        
        // GET: /Admin/User/
        public ActionResult Index()
        {
            var users = db.UserProfiles.ToList()
                                       .Select(CreateUserModel)
                                       .OrderBy(u => u.UserName).ToList();

            var sessionCountsPerUser = db.Sessions.GroupBy(s => s.SpeakerUserName)
                                         .ToDictionary(g => g.Key, g => g.Count());

            foreach (var user in users)
            {
                int sessionCount;
                sessionCountsPerUser.TryGetValue(user.UserName, out sessionCount);
                user.SubmittedSessionCount = sessionCount;
            }

            return View(users);
        }

        private static UserModel CreateUserModel(UserProfile profile)
        {
            return new UserModel
                {
                    UserId = profile.UserId,
                    UserName = profile.UserName,
                    Name = profile.Name,
                    EmailAddress = profile.EmailAddress,
                    MobilePhone = profile.MobilePhone,
                    WebsiteUrl = profile.WebsiteUrl,
                    TwitterHandle = profile.TwitterHandle,
                    Bio = profile.Bio,
                    GravatarUrl = profile.GravitarUrl()
                };
        }
    }
}
