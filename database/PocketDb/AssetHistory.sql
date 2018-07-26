﻿CREATE TABLE [dbo].[AssetHistory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AssetId] UNIQUEIDENTIFIER NOT NULL, 
    [EffectiveDate] DATETIME2 NOT NULL, 
    [ExchangeRateId] UNIQUEIDENTIFIER NULL, 
    [Balance] MONEY NOT NULL
)
