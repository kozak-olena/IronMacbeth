CREATE TABLE [dbo].[Order]
(
    [Id] INT  NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [UserLogin] VARCHAR (255) NOT NULL,
    [BookId] INT NOT NULL,
    [ArticleId] INT NOT NULL,
    [PeriodicalId] INT NOT NULL,
    [NewspaperId] INT NOT NULL,
    [ThesesId] INT NOT NULL,
    [TypeOfOrder] VARCHAR (255) NOT NULL
)
