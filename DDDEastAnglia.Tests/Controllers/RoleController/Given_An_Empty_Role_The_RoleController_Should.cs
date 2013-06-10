using NUnit.Framework;
using NSubstitute;
using DDDEastAnglia;

namespace DDDEastAnglia.Tests.RoleController
{
    [TestFixture]
    class Given_An_Empty_Role_The_RoleController_Should
    {
        [Test]
        public void Delete_It_Without_Confirmation()
        {
            Given_An_Empty_Role();
        }

        private void Given_An_Empty_Role()
        {
            roleManager = Substitute.For<IRoleManager>();

        }

        private object roleManager;
    }
}