CREATE TABLE [dbo].[UserProfile] (
    [UserId]        INT            IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (MAX) NULL,
    [Name]          NVARCHAR (MAX) NOT NULL,
    [EmailAddress]  NVARCHAR (MAX) NOT NULL,
    [WebsiteUrl]    NVARCHAR (MAX) NULL,
    [TwitterHandle] NVARCHAR (MAX) NULL,
    [Bio]           NVARCHAR (MAX) NULL,
    [MobilePhone]   NVARCHAR (MAX) NULL,
    [NewSpeaker]    BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

