CREATE TABLE [dbo].[Article]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Name] VARCHAR (255) NOT NULL,
    [Author] VARCHAR (255) NOT NULL,
    [Year] VARCHAR (255) NOT NULL,
    [Pages] VARCHAR (255) NOT NULL,
    [TypeOfDocument] VARCHAR (255) NULL,
    [Availiability] VARCHAR (255) NULL,
    [MainDocumentId] INT NOT NULL,
    [ElectronicVersionFileName]VARCHAR (255) NULL,
    [ElectronicVersionPrice]VARCHAR (255) NULL,
    [Rating]VARCHAR (255) NULL,
    [Comments]VARCHAR (255) NULL,
      [ImageName] NVARCHAR (1024) NULL,
    [DescriptionName] NVARCHAR (1024) NULL
)
