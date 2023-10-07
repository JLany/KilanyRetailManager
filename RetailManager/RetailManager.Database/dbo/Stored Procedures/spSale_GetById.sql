CREATE PROCEDURE [dbo].[spSale_GetById]
	@Id int
AS
BEGIN 

	SET NOCOUNT ON;

	SELECT [Id], [CashierId], [SaleDate], [SubTotal], [Tax], [Total]
	FROM [dbo].[Sale]
	WHERE [Id] = @Id;

END