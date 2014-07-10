using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IAccountLoginMethodQuery
    {
        LoginMethods GetLoginMethods(int userId);
    }
}
