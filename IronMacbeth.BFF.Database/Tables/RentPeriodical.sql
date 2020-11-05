CREATE TABLE [dbo].[RentPeriodical]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [RentId] INT NOT NULL,
    [PeriodicalId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [ProductPrice] INT NOT NULL,
    [Date] VARCHAR (255) NOT NULL
)
