﻿CREATE TABLE [dbo].[Periodical]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [Periodical] VARCHAR (255) NOT NULL,
    [ResponsibleAuthors] VARCHAR (255) NOT NULL,
    [IssueNumber]VARCHAR (255) NOT NULL,
    [PublishingHouse] VARCHAR (255) NOT NULL,
    [City] VARCHAR (255) NOT NULL,
    [Year] VARCHAR (255) NOT NULL,
    [Pages]VARCHAR (255) NOT NULL,
    [Availiability] VARCHAR (255) NOT NULL,
    [Location]VARCHAR (255) NOT NULL,
    [TypeOfDocument]VARCHAR (255) NOT NULL,
    [ElectronicVersionFileName]VARCHAR (255) NOT NULL,
    [RentPrice]VARCHAR (255) NOT NULL,
    [ElectronicVersionPrice]VARCHAR (255) NOT NULL,
    [Rating]VARCHAR (255) NOT NULL,
    [Comments]VARCHAR (255) NOT NULL,
)
