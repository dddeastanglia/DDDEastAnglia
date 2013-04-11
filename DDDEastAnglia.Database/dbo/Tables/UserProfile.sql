CREATE TABLE [dbo].[UserProfile] (
    [UserId]        INT            IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (MAX) NULL,
    [Name]          NVARCHAR (MAX) CONSTRAINT [df_EmptyStringName] DEFAULT ('') NOT NULL,
    [EmailAddress]  NVARCHAR (MAX) CONSTRAINT [df_EmptyStringEmailAddress] DEFAULT ('') NOT NULL,
    [WebsiteUrl]    NVARCHAR (MAX) NULL,
    [TwitterHandle] NVARCHAR (MAX) NULL,
    [Bio]           NVARCHAR (MAX) NULL,
    [MobilePhone]   NVARCHAR (MAX) NULL,
    [NewSpeaker]    BIT            CONSTRAINT [df_NewSpeakerFalse] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC)
);



