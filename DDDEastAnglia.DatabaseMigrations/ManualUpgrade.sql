-- Might have to rename some of the auto-named items in this script for it to work


DROP TABLE [dbo].[__MigrationHistory]
GO

-- CalendarItems
ALTER TABLE [dbo].[CalendarItems] ALTER COLUMN [EntryTypeString] [nvarchar] (max) NOT NULL
GO

ALTER TABLE [dbo].[CalendarItems] DROP CONSTRAINT [DF__CalendarI__IsPub__22AA2996]
GO

ALTER TABLE [dbo].[CalendarItems] ADD CONSTRAINT [DF_CalendarItems_IsPublic] DEFAULT 0 FOR [IsPublic]
GO

ALTER TABLE [dbo].[CalendarItems] DROP CONSTRAINT [DF__CalendarI__Autho__239E4DCF]
GO

ALTER TABLE [dbo].[CalendarItems] ADD CONSTRAINT [DF_CalendarItems_Authorised] DEFAULT 0 FOR [Authorised]
GO

sp_rename '[dbo].[CalendarItems].[PK_dbo.CalendarItems]', 'PK_CalendarItems', 'index'
GO

sp_rename '[dbo].[CalendarItems].[IX_ConferenceId]', 'IX_CalendarItems_ConferenceId', 'index'
GO

ALTER TABLE [dbo].[CalendarItems] DROP CONSTRAINT [FK_dbo.CalendarItems_dbo.Conferences_ConferenceId]
GO

ALTER TABLE [dbo].[CalendarItems] ADD CONSTRAINT [FK_CalendarItems_ConferenceId_Conferences_ConferenceId] FOREIGN KEY ([ConferenceId]) REFERENCES [dbo].[Conferences] ([ConferenceId])
GO


-- Conferences
sp_rename '[dbo].[Conferences].[PK_dbo.Conferences]', 'PK_Conferences', 'index'
GO 


-- Sessions
ALTER TABLE [dbo].[Sessions] ALTER COLUMN [SpeakerUserName] [nvarchar] (max) NOT NULL
GO

ALTER TABLE [dbo].[Sessions] ADD CONSTRAINT [DF_Sessions_Votes] DEFAULT (0) FOR [Votes]
GO

sp_rename '[dbo].[Sessions].[PK_dbo.Sessions]', 'PK_Sessions', 'index'
GO

sp_rename '[dbo].[Sessions].[IX_ConferenceId]', 'IX_Sessions_ConferenceId', 'index'
GO

ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [DF__Sessions__Confer__24927208]
GO

ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_dbo.Sessions_dbo.Conferences_ConferenceId]
GO

ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_Session_Conferences]
GO

ALTER TABLE [dbo].[Sessions] ADD CONSTRAINT [FK_Sessions_ConferenceId_Conferences_ConferenceId] FOREIGN KEY ([ConferenceId]) REFERENCES [dbo].[Conferences] ([ConferenceId])
GO


-- UserProfile
sp_rename 'UserProfile', 'UserProfiles'
GO

ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [df_EmptyStringName]
GO

ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [df_EmptyStringEmailAddress]
GO

ALTER TABLE [dbo].[UserProfiles] ALTER COLUMN [UserName] [nvarchar] (max) NOT NULL
GO

ALTER TABLE [dbo].[UserProfiles] DROP CONSTRAINT [df_NewSpeakerFalse]
GO

ALTER TABLE [dbo].[UserProfiles] ADD CONSTRAINT [DF_UserProfiles_NewSpeaker] DEFAULT 0 FOR [NewSpeaker]
GO

sp_rename '[dbo].[UserProfiles].[PK_dbo.UserProfile]', 'PK_UserProfiles', 'index'
GO


-- Votes
sp_rename '[dbo].[Votes].[PK_dbo.Votes]', 'PK_Votes', 'index'
GO

ALTER TABLE [dbo].[Votes] ALTER COLUMN [UserId] [int] NULL
GO

UPDATE [dbo].[Votes] SET [UserId] = NULL WHERE [UserId] = 0
GO

ALTER TABLE [dbo].[Votes] ADD CONSTRAINT [FK_Votes_UserId_UserProfiles_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfiles] ([UserId])
GO


-- webpages_Membership
CREATE TABLE [dbo].[webpages_Membership_new]
(
[UserId] [int] NOT NULL IDENTITY(1, 1),
[CreateDate] [datetime] NULL,
[ConfirmationToken] [nvarchar] (128) NULL,
[IsConfirmed] [bit] NULL CONSTRAINT [DF_webpages_Membership_IsConfirmed] DEFAULT ((0)),
[LastPasswordFailureDate] [datetime] NULL,
[PasswordFailuresSinceLastSuccess] [int] NOT NULL CONSTRAINT [DF_webpages_Membership_PasswordFailuresSinceLastSuccess] DEFAULT ((0)),
[Password] [nvarchar] (128) NOT NULL,
[PasswordChangedDate] [datetime] NULL,
[PasswordSalt] [nvarchar] (128) NOT NULL,
[PasswordVerificationToken] [nvarchar] (128) NULL,
[PasswordVerificationTokenExpirationDate] [datetime] NULL
)
GO

SET IDENTITY_INSERT [dbo].[webpages_Membership_new] ON
GO

INSERT INTO [dbo].[webpages_Membership_new]
(
[UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess],
[Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]
)
    SELECT [UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess],
			[Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]
    FROM [dbo].[webpages_Membership]
GO

SET IDENTITY_INSERT [dbo].[webpages_Membership_new] OFF
GO

DROP TABLE [dbo].[webpages_Membership]
GO

sp_rename 'webpages_Membership_new', 'webpages_Membership'
GO

ALTER TABLE [dbo].[webpages_Membership] ADD CONSTRAINT [PK_webpages_Membership] PRIMARY KEY CLUSTERED ([UserId])
GO


-- webpages_OAuthMembership
sp_rename '[dbo].[webpages_OAuthMembership].[PK__webpages__F53FC0ED6ECCCB60]', 'PK_webpages_OAuthMembership_Provider_ProviderUserId', 'index'
GO


-- webpages_Roles
sp_rename '[dbo].[webpages_Roles].[PK__webpages__8AFACE1A9D458D3C]', 'PK_webpages_Roles', 'index'
GO

sp_rename '[dbo].[webpages_Roles].[UQ__webpages__8A2B61602F2BD491]', 'UC_webpages_Roles_RoleName', 'index'
GO


-- webpages_UsersInRoles
sp_rename '[dbo].[webpages_UsersInRoles].[PK__webpages__AF2760AD15DD9609]', 'PK_webpages_UsersInRoles_UserId_RoleId', 'index'
GO

ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_RoleId]
GO

ALTER TABLE [dbo].[webpages_UsersInRoles] DROP CONSTRAINT [fk_UserId]
GO

ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [FK_webpages_UsersInRoles_RoleId_webpages_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO

ALTER TABLE [dbo].[webpages_UsersInRoles] ADD CONSTRAINT [FK_webpages_UsersInRoles_UserId_UserProfiles_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfiles] ([UserId])
GO


-- VersionInfo
CREATE TABLE [dbo].[VersionInfo]
(
	[Version] [bigint] NOT NULL,
	[AppliedOn] [datetime] NULL,
	[Description] [nvarchar](1024) NULL
)
GO

INSERT INTO [dbo].[VersionInfo] ([Version], [AppliedOn], [Description]) VALUES (20130629, getUtcDate(), 'Manual database upgrade to default schema')
GO
