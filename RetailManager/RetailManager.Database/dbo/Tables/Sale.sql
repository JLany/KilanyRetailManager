CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[CashierId] NVARCHAR(128) NOT NULL,
	[SaleDate] DATETIME2(7) NOT NULL, -- Default??
	[SubTotal] MONEY NOT NULL,
	[Tax] MONEY NOT NULL,
	[Total] MONEY NOT NULL,

	FOREIGN KEY ([CashierId]) REFERENCES [dbo].[User]([Id])
)
