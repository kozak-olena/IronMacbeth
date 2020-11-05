CREATE TABLE [dbo].[Article]
(
    [ArticleId] INT NOT NULL PRIMARY KEY,
    [ArticleName] VARCHAR (255) NOT NULL,
    [ArticleAuthor] VARCHAR (255) NOT NULL,
    [PublicationYear] VARCHAR (255) NOT NULL,
    [NumberOfPages] VARCHAR (255) NOT NULL,
    [TypeOfDocument] VARCHAR (255) NOT NULL,
    [MainDocumentId] INT NOT NULL FOREIGN KEY REFERENCES Book(BookId)
)
