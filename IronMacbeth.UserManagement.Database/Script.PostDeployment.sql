/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

--- Populate UserRole table ---
MERGE [dbo].[UserRole] AS target
USING
(
    SELECT Id, [Name]
    FROM
    (
        VALUES
        (1, 'User'),
        (2, 'Librarian'),
        (3, 'Admin')
    ) as userRoles(Id, [Name])
) AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET [Name] = source.[Name]
WHEN NOT MATCHED BY target THEN
    INSERT (Id, [NAME]) VALUES (source.Id, source.[Name])
WHEN NOT MATCHED BY source THEN
    DELETE;
    