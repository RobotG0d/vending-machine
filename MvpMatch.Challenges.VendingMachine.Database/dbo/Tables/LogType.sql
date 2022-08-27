CREATE TABLE [dbo].[LogType] (
    [Id]   INT           NOT NULL IDENTITY,
    [Name] VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

