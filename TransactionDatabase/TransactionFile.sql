CREATE TABLE [dbo].[TransactionFile]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Content] VARBINARY(MAX) NOT NULL, 
    [FileName] NVARCHAR(MAX) NOT NULL, 
    [Note] NVARCHAR(MAX) NOT NULL, 
    [Size] BIGINT NOT NULL, 
    [UploadDate] DATETIMEOFFSET NOT NULL
)
