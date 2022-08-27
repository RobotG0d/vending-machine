CREATE TABLE [dbo].[ProductLog] (
    [Id]        INT NOT NULL,
    [ProductId] INT NOT NULL,
    [LogTypeId] INT NOT NULL,
    [UserId]    INT NOT NULL,
    [Amount]    INT NOT NULL,
    CONSTRAINT [PK_ProductLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductLog_LogType] FOREIGN KEY ([LogTypeId]) REFERENCES [dbo].[LogType] ([Id]),
    CONSTRAINT [FK_ProductLog_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]),
    CONSTRAINT [FK_ProductLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

