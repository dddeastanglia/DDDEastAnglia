CREATE TABLE [dbo].[Votes] (
    [VoteId]           INT              IDENTITY (1, 1) NOT NULL,
    [SessionId]        INT              NOT NULL,
    [CookieId]         UNIQUEIDENTIFIER NOT NULL,
    [TimeRecorded]     DATETIME         NOT NULL,
    [UserId]           INT              NOT NULL,
    [IPAddress]        NVARCHAR (MAX)   NULL,
    [WebSessionId]     NVARCHAR (MAX)   NULL,
    [UserAgent]        NVARCHAR (MAX)   NULL,
    [ScreenResolution] NVARCHAR (MAX)   NULL,
    [Referrer]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_dbo.Votes] PRIMARY KEY CLUSTERED ([VoteId] ASC)
);

