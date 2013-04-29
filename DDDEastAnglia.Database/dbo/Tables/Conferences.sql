CREATE TABLE [dbo].[Conferences] (
    [ConferenceId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [ShortName]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.Conferences] PRIMARY KEY CLUSTERED ([ConferenceId] ASC)
);

