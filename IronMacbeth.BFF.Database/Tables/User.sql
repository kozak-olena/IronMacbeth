CREATE TABLE [dbo].[User]
(
    [Login] VARCHAR(255) NOT NULL PRIMARY KEY,
    [Password] VARCHAR(255) NOT NULL,
    [AccessLevel] INT NOT NULL
)
