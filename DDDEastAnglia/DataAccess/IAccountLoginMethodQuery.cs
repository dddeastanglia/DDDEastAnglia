using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IAccountLoginMethodQuery
    {
        IEnumerable<LoginMethod> GetLoginMethods(int userId);
    }
}
