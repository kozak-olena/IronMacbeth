CREATE TABLE [dbo].[Processor]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [NumberOfCores] INT NOT NULL,
    [Lithography] INT NOT NULL, 
    [Tdp] INT NOT NULL, 
    [Level2Cache] INT NOT NULL, 
    [Level3Cache] INT NOT NULL, 
    [BaseFrequency] NVARCHAR (1024) NULL, 
    [TurboFrequency] NVARCHAR (1024) NULL, 
    [Socket] NVARCHAR (1024) NULL, 
    [ProcessorCore] NVARCHAR (1024) NULL, 
    [ProcessorGraphics] NVARCHAR (1024) NULL, 
    [Model] NVARCHAR (1024) NULL, 
    [Mpn] NVARCHAR (1024) NULL, 
    [ImageName] NVARCHAR (1024) NULL, 
    [DescriptionName] NVARCHAR (1024) NULL
)