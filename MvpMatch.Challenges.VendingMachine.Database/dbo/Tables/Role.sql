﻿CREATE TABLE [dbo].[Role] (
    [Id]   INT           NOT NULL IDENTITY,
    [Name] VARCHAR (128) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);

