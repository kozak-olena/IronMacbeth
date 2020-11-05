CREATE TABLE [dbo].[RentBook]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [RentId] INT NOT NULL,
    [BookId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [ProductPrice] INT NOT NULL,
    [Date] VARCHAR (255) NOT NULL
)
