CREATE TABLE [dbo].[Motherboard]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Dimm] INT NOT NULL, 
    [Lan] INT NOT NULL, 
    [Usb] INT NOT NULL, 
    [Cpusocket] NVARCHAR (1024) NULL, 
    [Northbridge] NVARCHAR (1024) NULL, 
    [Southbridge] NVARCHAR (1024) NULL, 
    [Graphicalinterface] NVARCHAR (1024) NULL, 
    [Model] NVARCHAR (1024) NULL, 
    [Mpn] NVARCHAR (1024) NULL, 
    [ImageName] NVARCHAR (1024) NULL, 
    [DescriptionName] NVARCHAR (1024) NULL
)