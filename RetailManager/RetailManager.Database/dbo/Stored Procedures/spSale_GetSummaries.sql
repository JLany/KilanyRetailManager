CREATE PROCEDURE [dbo].[spSale_GetSummaries]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [s].[Id] SaleId, [s].[SaleDate], [s].[SubTotal], [s].[Tax],
		[s].[Total], [u].[FirstName] CashierFirstName, [u].[LastName] CashierLastName,
		[u].[EmailAddress] CashierEmailAddress

	FROM Sale s
	INNER JOIN [User] u 
		ON s.CashierId = u.Id;

END