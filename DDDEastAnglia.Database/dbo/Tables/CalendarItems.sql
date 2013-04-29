CREATE TABLE [dbo].[CalendarItems] (
    [CalendarItemId]  INT                IDENTITY (1, 1) NOT NULL,
    [ConferenceId]    INT                NOT NULL,
    [IsPublic]        BIT                DEFAULT ((0)) NOT NULL,
    [Authorised]      BIT                DEFAULT ((0)) NOT NULL,
    [Description]     NVARCHAR (MAX)     NOT NULL,
    [StartDate]       DATETIMEOFFSET (7) NOT NULL,
    [EndDate]         DATETIMEOFFSET (7) NULL,
    [EntryType]       INT                NOT NULL,
    [EntryTypeString] NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_dbo.CalendarItems] PRIMARY KEY CLUSTERED ([CalendarItemId] ASC),
    CONSTRAINT [FK_dbo.CalendarItems_dbo.Conferences_ConferenceId] FOREIGN KEY ([ConferenceId]) REFERENCES [dbo].[Conferences] ([ConferenceId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ConferenceId]
    ON [dbo].[CalendarItems]([ConferenceId] ASC);

