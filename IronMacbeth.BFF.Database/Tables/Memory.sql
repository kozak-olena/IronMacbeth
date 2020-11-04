CREATE TABLE [dbo].[Memory]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Size] INT NOT NULL,
    [Frequency] INT NOT NULL,
    [Type] NVARCHAR (1024) NULL,
    [Standart] NVARCHAR (1024) NULL,
    [Timings] NVARCHAR (1024) NULL,
    [Voltage] NVARCHAR (1024) NULL,
    [Formfactor] NVARCHAR (1024) NULL,
    [Model] NVARCHAR (1024) NULL,
    [Mpn] NVARCHAR (1024) NULL,
    [ImageName] NVARCHAR (1024) NULL,
    [DescriptionName] NVARCHAR (1024) NULL
)