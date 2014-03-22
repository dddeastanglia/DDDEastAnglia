using System.Collections.Generic;
using DDDEastAnglia.Areas.Admin.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers.RoleController
{
    [TestFixture]
    class RoleController
    {
        // User that is a member doesn't get added again
        // User that is not a member doesn't get removed
        // User that is a member gets removed
        [Test]
        public void User_That_Is_Not_A_Member_Gets_Added()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(false);
            ManageRoleModel model = new ManageRoleModel
            {
                RoleName = "dummyrole",
                roleUsers = new SortedList<string, RoleUserModel>()
            };
            model.roleUsers.Add("testuser", new RoleUserModel { IsMember = true, UserId = 999, Username = "testuser" });

            // Act
            controller.Manage(model);

            // Assert            
            manager.Received().AddUserToRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_A_Member_Does_Not_Get_Added_Again()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(true);
            ManageRoleModel model = new ManageRoleModel
            {
                RoleName = "dummyrole",
                roleUsers = new SortedList<string, RoleUserModel>()
            };
            model.roleUsers.Add("testuser", new RoleUserModel { IsMember = true, UserId = 999, Username = "testuser" });

            // Act
            controller.Manage(model);

            // Assert            
            manager.DidNotReceive().AddUserToRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_A_Member_Gets_Removed()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(true);
            ManageRoleModel model = new ManageRoleModel
            {
                RoleName = "dummyrole",
                roleUsers = new SortedList<string, RoleUserModel>()
            };
            model.roleUsers.Add("testuser", new RoleUserModel { IsMember = false, UserId = 999, Username = "testuser" });

            // Act
            controller.Manage(model);

            // Assert            
            manager.Received().RemoveUserFromRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_Not_A_Member_Does_Not_Get_Removed()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(false);
            ManageRoleModel model = new ManageRoleModel
            {
                RoleName = "dummyrole",
                roleUsers = new SortedList<string, RoleUserModel>()
            };
            model.roleUsers.Add("testuser", new RoleUserModel { IsMember = false, UserId = 999, Username = "testuser" });

            // Act
            controller.Manage(model);

            // Assert            
            manager.DidNotReceive().RemoveUserFromRole("testuser", "dummyrole");
        }

        [Test]
        public void Confirmed_Delete_Deletes_A_Populated_Role()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.GetUsersCount("dummyrole").Returns(5);

            // Act
            controller.DeleteConfirmed("dummyrole");

            // Assert            
            manager.Received().DeleteRole("dummyrole");
        }

        [Test]
        public void Confirmed_Delete_Deletes_An_Unpopulated_Role()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.GetUsersCount("dummyrole").Returns(0);

            // Act
            controller.DeleteConfirmed("dummyrole");

            // Assert            
            manager.Received().DeleteRole("dummyrole");
        }

        [Test]
        public void Dont_Delete_A_Populated_Role_Without_Confirmation()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.GetUsersCount("dummyrole").Returns(5);

            // Act
            controller.Delete("dummyrole");

            // Assert            
            manager.DidNotReceive().DeleteRole("dummyrole");
        }

        [Test]
        public void Delete_An_Empty_Role_Without_Confirmation()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.GetUsersCount("dummyrole").Returns(0);

            // Act
            controller.Delete("dummyrole");

            // Assert            
            manager.Received().DeleteRole("dummyrole");
        }

        [Test]
        public void Add_A_New_Role_That_Doesnt_Exist()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.RoleExists("dummyrole").Returns(false);
            CreateRoleModel model = new CreateRoleModel { RoleName = "dummyrole" };

            // Act
            controller.Create(model);

            // Assert            
            manager.Received().CreateRole("dummyrole");
        }

        [Test]
        public void Dont_Add_A_New_Role_That_Already_Exists()
        {
            // Arrange
            IRoleManager manager = Substitute.For<IRoleManager>();
            Areas.Admin.Controllers.RoleController controller = new Areas.Admin.Controllers.RoleController(manager);
            manager.RoleExists("dummyrole").Returns(true);
            CreateRoleModel model = new CreateRoleModel { RoleName = "dummyrole" };

            // Act
            controller.Create(model);

            // Assert            
            manager.DidNotReceive().CreateRole("dummyrole");
        }

    }
}