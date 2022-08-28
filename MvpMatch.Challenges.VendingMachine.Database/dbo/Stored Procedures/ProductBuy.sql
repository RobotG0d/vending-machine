
CREATE   PROCEDURE [dbo].[ProductBuy]
	@userId int
,	@productId int
,	@productAmount int
AS
BEGIN

	SET NOCOUNT ON;

	BEGIN TRAN

	UPDATE [dbo].[Product]
	SET 
		[Product].[AmountAvailable] = [AmountAvailable] - @productAmount
	OUTPUT
		inserted.*
	,	inserted.[Cost] * @productAmount AS [TotalSpent]
	,	[User].[Deposit]- (inserted.[Cost] * @productAmount) AS [ChangeValue]
	,	@productAmount AS [ProductAmount]
	FROM [dbo].[Product]
	INNER JOIN [dbo].[User] ON ([User].[Id] = @userId)
	WHERE 
		[Product].[Id] = @productId
	AND [Product].[AmountAvailable] >= @productAmount
	AND [User].[Deposit] >= [Product].[Cost] * @productAmount

	IF @@ROWCOUNT = 0
	BEGIN
	
		DECLARE @totalCost INT
		DECLARE @existingMoney INT
		DECLARE @existingAmount BIT

		SELECT 
			@totalCost = [Product].[Cost] * @productAmount
		,	@existingAmount = [Product].[AmountAvailable]
		,	@existingMoney = [User].[Deposit]	
		FROM [dbo].[Product]
		INNER JOIN [dbo].[User] ON ([User].[Id] = @userId)
		WHERE [Product].[Id] = @productId

		ROLLBACK TRAN

		IF @totalCost > @existingMoney
			THROW 50000, 'User does not have enough money to fulfill the request.', 0
		ELSE IF @existingAmount < @productAmount
			THROW 50001, 'There aren''t enough products to satisfy the request.', 0

	END

	UPDATE [dbo].[User]
	SET [Deposit] = 0
	WHERE [User].[Id] = @userId

	COMMIT TRAN

END