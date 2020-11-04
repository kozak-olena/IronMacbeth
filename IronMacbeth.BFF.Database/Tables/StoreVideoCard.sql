CREATE TABLE [dbo].[StoreVideoCard]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Storeid] INT NOT NULL, 
    [VideoCardId] INT NOT NULL, 
    [ProductPrice] INT NOT NULL, 
    [ProductWarranty] INT NOT NULL
)