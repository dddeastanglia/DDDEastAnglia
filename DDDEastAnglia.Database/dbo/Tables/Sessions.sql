CREATE TABLE [dbo].[Sessions] (
    [SessionId]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (MAX) NOT NULL,
    [Abstract]        NVARCHAR (MAX) NOT NULL,
    [SpeakerUserName] NVARCHAR (MAX) NULL,
    [Votes]           INT            NOT NULL,
    [ConferenceId]    INT            DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_dbo.Sessions] PRIMARY KEY CLUSTERED ([SessionId] ASC),
    CONSTRAINT [FK_dbo.Sessions_dbo.Conferences_ConferenceId] FOREIGN KEY ([ConferenceId]) REFERENCES [dbo].[Conferences] ([ConferenceId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Session_Conferences] FOREIGN KEY ([ConferenceId]) REFERENCES [dbo].[Conferences] ([ConferenceId])
);




GO
CREATE NONCLUSTERED INDEX [IX_ConferenceId]
    ON [dbo].[Sessions]([ConferenceId] ASC);

