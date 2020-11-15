CREATE TABLE [dbo].[Order]
(
    [Id] INT  NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [UserLogin] VARCHAR (255) NOT NULL,
    [BookId] INT NULL,
    [ArticleId] INT NULL,
    [PeriodicalId] INT NULL,
    [NewspaperId] INT NULL,
    [ThesesId] INT NULL,
    [TypeOfOrder] VARCHAR (255) NOT NULL
)
