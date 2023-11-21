CREATE PROCEDURE [dbo].[spInventoryBatch_GetAll]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [ProductName], [ProductId], [Quantity], [PurchasePrice], [PurchaseDate] 
	FROM InventoryBatch;

END