﻿CREATE TABLE [dbo].[Currency]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [Name] NVARCHAR(10) NOT NULL, 
    [Description] NVARCHAR(50) NULL
)
