CREATE TABLE [dbo].[InventoryBatch]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductName] nvarchar(100) NOT NULL,
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 1, 
    [PurchasePrice] MONEY NOT NULL, 
    [PurchaseDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_InventoryBatch_ToProduct] FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
)
