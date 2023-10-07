﻿CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
BEGIN 

	SET NOCOUNT ON;

	SELECT [Id], [ProductName], [Description], [RetailPrice], [QuantityInStock], [IsTaxable], [CreatedDate], [LastModified]
	FROM [dbo].[Product]
	ORDER BY [ProductName];

END
