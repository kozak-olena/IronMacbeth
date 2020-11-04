CREATE TABLE [dbo].[StoreMemory]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Storeid] INT NOT NULL, 
    [MemoryId] INT NOT NULL, 
    [ProductPrice] INT NOT NULL, 
    [ProductWarranty] INT NOT NULL
)
