CREATE PROCEDURE [dbo].[spInventoryBatch_Insert]
	@Id int output,
	@ProductName nvarchar(100),
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@PurchaseDate datetime2

AS
BEGIN 
	
	SET NOCOUNT ON;

	INSERT INTO InventoryBatch(ProductName, ProductId, Quantity, PurchasePrice, PurchaseDate)
	VALUES(@ProductName, @ProductId, @Quantity, @PurchasePrice, @PurchaseDate);

	SELECT @Id = SCOPE_IDENTITY();

END