CREATE PROCEDURE [dbo].[spSale_Update]
	@Id int,
	@CashierId nvarchar(128),
	@SaleDate datetime2(7), 
	@SubTotal money,
	@Tax money,
	@Total money
AS
BEGIN 
	SET NOCOUNT ON;

	UPDATE [dbo].[Sale]
	SET 
		SubTotal = @SubTotal,
		Tax = @Tax,
		Total = @Total

	WHERE [Id] = @Id;

END