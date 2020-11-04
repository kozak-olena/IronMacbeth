CREATE TABLE [dbo].[StoreProcessor]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Storeid] INT NOT NULL, 
    [ProcessorId] INT NOT NULL, 
    [ProductPrice] INT NOT NULL, 
    [ProductWarranty] INT NOT NULL
)
