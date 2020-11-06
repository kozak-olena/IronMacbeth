CREATE TABLE [dbo].[Newspaper]
(
    [NewspaperId] INT NOT NULL PRIMARY KEY,
    [NewspaperName] VARCHAR (255) NOT NULL,
    [City] VARCHAR (255) NOT NULL,
    [PublicationYear] VARCHAR (255) NOT NULL, 
    [TypeOfDocument] VARCHAR (255) NOT NULL,
    [Availability] VARCHAR (255) NOT NULL,
    [Location] VARCHAR (255) NOT NULL,
    [ElectronicVersion]VARCHAR (255) NOT NULL,
    [Rating]VARCHAR (255) NOT NULL,
    [Comments]VARCHAR (255) NOT NULL,
)
