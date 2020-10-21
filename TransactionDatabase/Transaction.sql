CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TransactionId] NVARCHAR(50) NOT NULL, 
    [Amount] DECIMAL(18, 2) NOT NULL, 
    [CurrencyCode] NCHAR(3) NOT NULL, 
    [Status] NCHAR(1) NOT NULL, 
    [TransactionDate] DATETIMEOFFSET NOT NULL
)
