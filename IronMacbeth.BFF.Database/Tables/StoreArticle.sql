CREATE TABLE [dbo].[StoreArticle]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [StoreId] INT NOT NULL,
    [BookId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [ProductPrice] INT NOT NULL,
)
