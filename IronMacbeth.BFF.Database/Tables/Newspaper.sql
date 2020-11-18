CREATE TABLE [dbo].[Newspaper]
(
    [Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [Name] VARCHAR (255) NOT NULL,
    [City] VARCHAR (255) NULL,
    [Year] INT NULL, 
    [TypeOfDocument] VARCHAR (255) NOT NULL,
    [Availiability] INT NULL,
    [IssueNumber] INT NULL,
    [Location] VARCHAR (255) NULL,
    [ElectronicVersionFileName]VARCHAR (255) NULL,
    [RentPrice]VARCHAR (255) NOT NULL,
    [Rating]VARCHAR (255) NULL,
    [Comments]VARCHAR (255) NULL,
)
