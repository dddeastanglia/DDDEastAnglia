using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface ILoginMethodLoader
    {
        IEnumerable<LoginMethodViewModel> GetLoginMethods(int userIdentity);
    }

    public class LoginMethodLoader : ILoginMethodLoader
    {
        private readonly IAccountLoginMethodQuery accountLoginMethodQuery;
        private readonly IBuild<LoginMethod, LoginMethodViewModel> loginMethodViewModelBuilder;

        public LoginMethodLoader(IAccountLoginMethodQuery accountLoginMethodQuery, IBuild<LoginMethod, LoginMethodViewModel> loginMethodViewModelBuilder)
        {
            if (accountLoginMethodQuery == null)
            {
                throw new ArgumentNullException(nameof(accountLoginMethodQuery));
            }

            if (loginMethodViewModelBuilder == null)
            {
                throw new ArgumentNullException(nameof(loginMethodViewModelBuilder));
            }

            this.accountLoginMethodQuery = accountLoginMethodQuery;
            this.loginMethodViewModelBuilder = loginMethodViewModelBuilder;
        }

        public IEnumerable<LoginMethodViewModel> GetLoginMethods(int userIdentity)
        {
            var loginMethods = accountLoginMethodQuery.GetLoginMethods(userIdentity);
            return loginMethods.Select(loginMethodViewModelBuilder.Build);
        }
    }
}
