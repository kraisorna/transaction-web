CREATE TABLE [dbo].[TransactionImport]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TransactionId] INT NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [CurrencyCode] NCHAR(3) NULL, 
    [Status] NCHAR(1) NULL, 
    [TransactionDate] DATETIMEOFFSET NULL, 
    [ImportStatus] INT NOT NULL, 
    [Note] NVARCHAR(MAX) NULL, 
    [ImportDate] DATETIMEOFFSET NOT NULL
)
