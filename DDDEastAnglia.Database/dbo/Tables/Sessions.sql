CREATE TABLE [dbo].[Sessions] (
    [SessionId]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (MAX) NOT NULL,
    [Abstract]        NVARCHAR (MAX) NOT NULL,
    [SpeakerUserName] NVARCHAR (MAX) NULL,
    [Votes]           INT            NOT NULL,
    CONSTRAINT [PK_dbo.Sessions] PRIMARY KEY CLUSTERED ([SessionId] ASC)
);

