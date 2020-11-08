CREATE TABLE [dbo].[Newspaper]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [Name] VARCHAR (255) NOT NULL,
    [City] VARCHAR (255) NOT NULL,
    [Year] VARCHAR (255) NOT NULL, 
    [TypeOfDocument] VARCHAR (255) NOT NULL,
    [Availability] VARCHAR (255) NOT NULL,
    [Location] VARCHAR (255) NOT NULL,
    [ElectronicVersionFileName]VARCHAR (255) NOT NULL,
    [RentPrice]VARCHAR (255) NOT NULL,
    [ElectronicVersionPrice]VARCHAR (255) NOT NULL,
    [Rating]VARCHAR (255) NOT NULL,
    [Comments]VARCHAR (255) NOT NULL,
      [ImageName] NVARCHAR (1024) NULL,
    [DescriptionName] NVARCHAR (1024) NULL
)
