using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers.RoleController
{
    [TestFixture]
    class RoleController
    {
        [Test]
        public void Delete_An_Empty_Role_Without_Confirmation()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            RoleController controller = new RoleController(manager);

            // Act


            // Assert            
        }

    }
}