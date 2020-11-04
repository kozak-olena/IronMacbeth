CREATE TABLE [dbo].[Purchase]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [LinkId] INT NOT NULL, 
    [Number] INT NOT NULL, 
    [Linkname] NVARCHAR (1024) NULL, 
    [Firstname] NVARCHAR (1024) NULL, 
    [Secondname] NVARCHAR (1024) NULL, 
    [Email] NVARCHAR (1024) NULL, 
    [Date] NVARCHAR (1024) NULL, 
    [IsRead] NVARCHAR (1024) NULL
)