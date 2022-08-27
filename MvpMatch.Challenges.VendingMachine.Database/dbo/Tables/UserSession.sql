CREATE TABLE [dbo].[UserSession] (
    [Id]        INT              NOT NULL IDENTITY,
    [UserId]    INT              NOT NULL,
    [UniqueId]  UNIQUEIDENTIFIER CONSTRAINT [DF_UserSession_UniqueId] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_UserSession] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSession_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserSession]
    ON [dbo].[UserSession]([UserId] ASC, [UniqueId] ASC);

