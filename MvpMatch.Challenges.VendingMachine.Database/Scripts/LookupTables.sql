SET IDENTITY_INSERT [dbo].[Role] ON

INSERT INTO [dbo].[Role] ([Id], [Name])
VALUES
	(1, 'Seller')
,	(2, 'Buyer')

SET IDENTITY_INSERT [dbo].[Role] OFF