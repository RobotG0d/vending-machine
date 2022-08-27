CREATE TABLE [dbo].[User] (
    [Id]       INT           NOT NULL,
    [RoleId]   INT           NOT NULL,
    [Username] VARCHAR (256) NOT NULL,
    [Password] VARCHAR (256) NOT NULL,
    [Deposit]  INT           CONSTRAINT [DF_User_Deposit] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User]
    ON [dbo].[User]([Username] ASC);

