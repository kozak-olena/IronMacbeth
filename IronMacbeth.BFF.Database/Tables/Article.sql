CREATE TABLE [dbo].[Article]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [Name] VARCHAR (255) NULL,
    [Author] VARCHAR (255) NOT NULL,
    [Year] VARCHAR (255) NOT NULL,
    [Pages] VARCHAR (255) NOT NULL,
    [TypeOfDocument] VARCHAR (255) NULL,
    [Availiability] VARCHAR (255) NULL,
    [MainDocumentId] INT NOT NULL FOREIGN KEY REFERENCES Book(Id),
    [ElectronicVersionFileName]VARCHAR (255) NULL,
    [ElectronicVersionPrice]VARCHAR (255) NOT NULL,
    [Rating]VARCHAR (255) NOT NULL,
    [Comments]VARCHAR (255) NOT NULL,
)
