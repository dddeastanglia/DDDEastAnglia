using System.Data;
using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20130629)]
    public class InitialDatabase : Migration
    {
        public override void Up()
        {
            CreateMembershipTable();
            CreateRolesTable();
            CreateUsersInRolesTable();
            CreateOAuthMembershipTable();

            CreateCalendarItemsTable();
            CreateConferencesTable();
            CreateSessionsTable();
            CreateUserProfileTable();
            CreateVotesTable();

            CreateForeignKeys();
        }

        private void CreateMembershipTable()
        {
            Create.Table("webpages_Membership")
                        .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("CreateDate").AsDateTime().Nullable()
                        .WithColumn("ConfirmationToken").AsString(128).Nullable()
                        .WithColumn("IsConfirmed").AsBoolean().Nullable().WithDefaultValue(false)
                        .WithColumn("LastPasswordFailureDate").AsDateTime().Nullable()
                        .WithColumn("PasswordFailuresSinceLastSuccess").AsInt32().NotNullable().WithDefaultValue(0)
                        .WithColumn("Password").AsString(128).NotNullable()
                        .WithColumn("PasswordChangedDate").AsDateTime().Nullable()
                        .WithColumn("PasswordSalt").AsString(128).NotNullable()
                        .WithColumn("PasswordVerificationToken").AsString(128).Nullable()
                        .WithColumn("PasswordVerificationTokenExpirationDate").AsDateTime().Nullable();
        }

        private void CreateRolesTable()
        {
            Create.Table("webpages_Roles")
                        .WithColumn("RoleId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("RoleName").AsString(256).NotNullable();

            Create.Index()
                        .OnTable("webpages_Roles")
                        .OnColumn("RoleName").Unique();
        }

        private void CreateUsersInRolesTable()
        {
            Create.Table("webpages_UsersInRoles")
                        .WithColumn("UserId").AsInt32().NotNullable()
                        .WithColumn("RoleId").AsInt32().NotNullable();
        }

        private void CreateOAuthMembershipTable()
        {
            Create.Table("webpages_OAuthMembership")
                        .WithColumn("Provider").AsString(30).NotNullable()
                        .WithColumn("ProviderUserId").AsString(100).NotNullable()
                        .WithColumn("UserId").AsInt32().NotNullable();

            Create.PrimaryKey()
                        .OnTable("webpages_OAuthMembership")
                        .Columns(new[] { "Provider", "ProviderUserId" });
        }

        private void CreateCalendarItemsTable()
        {
            Create.Table("CalendarItems")
                        .WithColumn("CalendarItemId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("ConferenceId").AsInt32().NotNullable()
                        .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(false)
                        .WithColumn("Authorised").AsBoolean().NotNullable().WithDefaultValue(false)
                        .WithColumn("Description").AsString(int.MaxValue).NotNullable()
                        .WithColumn("StartDate").AsCustom("DateTimeOffset").NotNullable()
                        .WithColumn("EndDate").AsCustom("DateTimeOffset").Nullable()
                        .WithColumn("EntryType").AsInt32().NotNullable()
                        .WithColumn("EntryTypeString").AsString(int.MaxValue).NotNullable();

            Create.Index()
                        .OnTable("CalendarItems")
                        .OnColumn("ConferenceId");
        }

        private void CreateConferencesTable()
        {
            Create.Table("Conferences")
                        .WithColumn("ConferenceId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("Name").AsString(int.MaxValue).NotNullable()
                        .WithColumn("ShortName").AsString(int.MaxValue).NotNullable();
        }

        private void CreateSessionsTable()
        {
            Create.Table("Sessions")
                        .WithColumn("SessionId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("Title").AsString(int.MaxValue).NotNullable()
                        .WithColumn("Abstract").AsString(int.MaxValue).NotNullable()
                        .WithColumn("SpeakerUserName").AsString(int.MaxValue).NotNullable()
                        .WithColumn("Votes").AsInt32().NotNullable().WithDefaultValue(0)
                        .WithColumn("ConferenceId").AsInt32().NotNullable();

            Create.Index()
                        .OnTable("Sessions")
                        .OnColumn("ConferenceId");
        }

        private void CreateUserProfileTable()
        {
            Create.Table("UserProfile")
                        .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("UserName").AsString(int.MaxValue).NotNullable()
                        .WithColumn("Name").AsString(int.MaxValue).NotNullable()
                        .WithColumn("EmailAddress").AsString(int.MaxValue).NotNullable()
                        .WithColumn("WebsiteUrl").AsString(int.MaxValue).Nullable()
                        .WithColumn("TwitterHandle").AsString(int.MaxValue).Nullable()
                        .WithColumn("Bio").AsString(int.MaxValue).Nullable()
                        .WithColumn("MobilePhone").AsString(int.MaxValue).Nullable()
                        .WithColumn("NewSpeaker").AsBoolean().Nullable().WithDefaultValue(false);
        }

        private void CreateVotesTable()
        {
            Create.Table("Votes")
                        .WithColumn("VoteId").AsInt32().NotNullable().PrimaryKey().Identity()
                        .WithColumn("SessionId").AsInt32().NotNullable()
                        .WithColumn("CookieId").AsGuid().NotNullable()
                        .WithColumn("TimeRecorded").AsDateTime().NotNullable()
                        .WithColumn("UserId").AsInt32().NotNullable()
                        .WithColumn("WebSessionId").AsString(int.MaxValue).Nullable()
                        .WithColumn("IPAddress").AsString(int.MaxValue).Nullable()
                        .WithColumn("UserAgent").AsString(int.MaxValue).Nullable()
                        .WithColumn("ScreenResolution").AsString(int.MaxValue).Nullable()
                        .WithColumn("Referrer").AsString(int.MaxValue).Nullable();
        }

        private void CreateForeignKeys()
        {
            Create.ForeignKey()
                        .FromTable("CalendarItems").ForeignColumn("ConferenceId")
                        .ToTable("Conferences").PrimaryColumn("ConferenceId")
                        .OnDelete(Rule.Cascade);

            Create.ForeignKey()
                        .FromTable("Sessions").ForeignColumn("ConferenceId")
                        .ToTable("Conferences").PrimaryColumn("ConferenceId");

            Create.ForeignKey()
                        .FromTable("Votes").ForeignColumn("UserId")
                        .ToTable("UserProfile").PrimaryColumn("UserId");

            Create.ForeignKey()
                        .FromTable("webpages_UsersInRoles").ForeignColumn("UserId")
                        .ToTable("UserProfile").PrimaryColumn("UserId");

            Create.ForeignKey()
                        .FromTable("webpages_UsersInRoles").ForeignColumn("RoleId")
                        .ToTable("webpages_Roles").PrimaryColumn("RoleId");
        }

        public override void Down()
        {
            Delete.Table("webpages_Membership");
            Delete.Table("webpages_Roles");
            Delete.Table("webpages_UsersInRoles");
            Delete.Table("webpages_OAuthMembership");

            Delete.Table("CalendarItems");
            Delete.Table("Conferences");
            Delete.Table("Sessions");
            Delete.Table("UserProfile");
            Delete.Table("Votes");
        }
    }
}
