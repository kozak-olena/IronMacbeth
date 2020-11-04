CREATE TABLE [dbo].[Store]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Name] NVARCHAR (1024) NULL, 
    [Delivery] NVARCHAR (1024) NULL, 
    [OwnerId] NVARCHAR (1024) NULL, 
    [ImageName] NVARCHAR (1024) NULL
)