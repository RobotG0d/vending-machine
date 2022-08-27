CREATE TABLE [dbo].[Product] (
    [Id]              INT           NOT NULL IDENTITY,
    [SellerId]        INT           NOT NULL,
    [Name]            VARCHAR (128) NOT NULL,
    [Cost]            INT           NOT NULL,
    [AmountAvailable] INT           NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Product_User] FOREIGN KEY ([SellerId]) REFERENCES [dbo].[User] ([Id])
);

