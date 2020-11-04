CREATE TABLE [dbo].[StoreMotherboard]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Storeid] INT NOT NULL, 
    [MotherboardId] INT NOT NULL, 
    [ProductPrice] INT NOT NULL, 
    [ProductWarranty] INT NOT NULL
)

/*
INSERT INTO dbo.[storemotherboard] (storeid, motherboardid, productprice, productwarranty) VALUES ('1', '1', '0', '0');

*/