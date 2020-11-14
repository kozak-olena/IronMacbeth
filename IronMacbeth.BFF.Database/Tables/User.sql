CREATE TABLE [dbo].[User]
(
    [Login] VARCHAR(255) NOT NULL PRIMARY KEY,
    [PasswordHash] VARCHAR(48) NOT NULL,
    [RoleId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[UserRole]([Id])
)
