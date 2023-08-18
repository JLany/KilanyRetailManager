CREATE TABLE [dbo].[SaleDetail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SaleId] INT NOT NULL,
	[ProductId] INT NOT NULL,
	[Quantity] INT NOT NULL DEFAULT 1,
	[PurchasePrice] MONEY NOT NULL,
	[Tax] MONEY NOT NULL DEFAULT 0,

	FOREIGN KEY ([SaleId]) REFERENCES [dbo].[Sale]([Id]),
	FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id])
)
