/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2015
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
*/
CREATE TABLE [dbo].[TypeOfDay]
(
	[TypeOfDayId] SMALLINT NOT NULL PRIMARY KEY, 
    [Code] VARCHAR(50) NULL, 
    [CreateByUserId] INT NOT NULL ,
    [CreateDate] DATETIME  NOT NULL   DEFAULT GETDATE() ,
    [CreateDateUTC] DATETIME2     NOT NULL DEFAULT GETUTCDATE() ,
    [UpdateByUserId] INT NOT NULL ,
    [UpdateDate] DATETIME  NOT NULL DEFAULT GETDATE() ,
    [UpdateDateUTC] DATETIME2 NOT NULL DEFAULT GETUTCDATE() ,
)
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Provides TypeOfDay', @level0type = N'SCHEMA',
    @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'TypeOfDay';
GO
