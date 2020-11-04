CREATE TABLE [dbo].[VideoCard]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Memory] INT NOT NULL, 
    [GpuFrequency] INT NOT NULL, 
    [MemoryFrequency] INT NOT NULL, 
    [Bus] INT NOT NULL, 
    [Gpu] NVARCHAR (1024) NULL, 
    [MemoryType] NVARCHAR (1024) NULL, 
    [Interface] NVARCHAR (1024) NULL, 
    [Cooling] NVARCHAR (1024) NULL, 
    [Model] NVARCHAR (1024) NULL, 
    [Mpn] NVARCHAR (1024) NULL, 
    [ImageName] NVARCHAR (1024) NULL, 
    [DescriptionName] NVARCHAR (1024) NULL
)