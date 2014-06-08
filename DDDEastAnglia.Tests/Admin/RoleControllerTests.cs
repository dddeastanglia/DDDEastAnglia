using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    class RoleControllerTests
    {
        [Test]
        public void Rename_Role_Removes_One_Role_And_Creates_A_New_Role_With_The_Same_Membership()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.GetAllRoles().Returns(new[] { "dummyrole1" });
            manager.GetUsersForRole("dummyrole1").Returns(new[] { "testuser1", "testuser2" });
            RoleModel model = new RoleModel { RoleName = "dummyrole1", NewRoleName = "dummyrole2" };
            controller.Rename(model);

            manager.Received().RenameRole("dummyrole1", "dummyrole2");
        }

        [Test]
        public void User_That_Is_Not_A_Member_Gets_Added()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(false);
            RoleModel model = new RoleModel
            {
                RoleName = "dummyrole",
                AvailableUsers = new SortedList<string, bool>()
            };
            model.AvailableUsers.Add("testuser", true);

            // Act
            controller.AddUsers(model);

            // Assert            
            manager.Received().AddUserToRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_A_Member_Does_Not_Get_Added_Again()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(true);
            RoleModel model = new RoleModel
            {
                RoleName = "dummyrole",
                AvailableUsers = new SortedList<string, bool>()
            };
            model.AvailableUsers.Add("testuser", true);

            // Act
            controller.AddUsers(model);

            // Assert            
            manager.DidNotReceive().AddUserToRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_A_Member_Gets_Removed()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(true);
            RoleModel model = new RoleModel
            {
                RoleName = "dummyrole",
                AvailableUsers = new SortedList<string, bool>()
            };
            model.AvailableUsers.Add("testuser", false);

            // Act
            controller.RemoveUsers(model);

            // Assert            
            manager.Received().RemoveUserFromRole("testuser", "dummyrole");
        }

        [Test]
        public void User_That_Is_Not_A_Member_Does_Not_Get_Removed()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.IsUserInRole("testuser", "dummyrole").Returns(false);
            RoleModel model = new RoleModel
            {
                RoleName = "dummyrole",
                AvailableUsers = new SortedList<string, bool>()
            };
            model.AvailableUsers.Add("testuser", false);

            // Act
            controller.RemoveUsers(model);

            // Assert            
            manager.DidNotReceive().RemoveUserFromRole("testuser", "dummyrole");
        }

        [Test]
        public void Confirmed_Delete_Deletes_A_Populated_Role()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
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
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
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
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
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
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
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
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.RoleExists("dummyrole").Returns(false);
            RoleModel model = new RoleModel { RoleName = "dummyrole" };

            // Act
            controller.Create(model);

            // Assert            
            manager.Received().CreateRole("dummyrole");
        }

        [Test]
        public void Dont_Add_A_New_Role_That_Already_Exists()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.RoleExists("dummyrole").Returns(true);
            RoleModel model = new RoleModel { RoleName = "dummyrole" };

            // Act
            controller.Create(model);

            // Assert            
            manager.DidNotReceive().CreateRole("dummyrole");
        }

        [Test]
        public void Index_View_Returns_List_Of_Roles()
        {
            // Arrange
            IRoleManager manager = CreateRoleManager();
            RoleController controller = CreateRoleController(manager);
            manager.GetAllRoles().Returns(new[] { "dummyrole1", "dummyrole2", "dummyrole3" });

            // Act
            var result = (ViewResult)controller.Index();

            // Assert            
            Assert.IsInstanceOf<List<string>>(result.Model);
            Assert.AreEqual(3, ((List<string>)result.Model).Count);

        }

        private static IRoleManager CreateRoleManager()
        {
            IRoleManager manager = Substitute.For<IRoleManager>();
            return manager;
        }

        private static RoleController CreateRoleController(IRoleManager manager)
        {
            IUserProfileRepository userRepo = Substitute.For<IUserProfileRepository>();
            userRepo.GetAllUserProfiles()
                .Returns(new List<UserProfile>
                {
                    new UserProfile {UserName = "testuser1"},
                    new UserProfile {UserName = "testuser2"},
                    new UserProfile {UserName = "testuser3"}
                });
            RoleController controller = new RoleController(manager, userRepo);
            return controller;
        }
    }
}