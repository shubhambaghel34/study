/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2017
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
CREATE TABLE [dbo].[LocationState](
	[LocationStateId] [int] NOT NULL
		 CONSTRAINT PK_LocationState PRIMARY KEY CLUSTERED (LocationStateId),
	[CreateDate] [datetime] NOT NULL
		CONSTRAINT DF_LocationState_01 DEFAULT getdate(),
	[CreateByUserId] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
		CONSTRAINT DF_LocationState_02 DEFAULT getdate(),
	[UpdateByUserId] [int] NOT NULL,		 
	[Code] [varchar](3) NOT NULL,
	[Name] [varchar](50) NOT NULL
);

GO

CREATE NONCLUSTERED INDEX [IX_LocationState_Code]
ON [dbo].[LocationState] ([Code])
WITH (SORT_IN_TEMPDB = ON, ONLINE = ON, FILLFACTOR = 90)
GO

EXECUTE sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Contains information about States',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of this entry (Primary Key)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'LocationStateId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was created',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'CreateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that created this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'CreateByUserId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Date that this entry was last updated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ID of the user that last updated this entry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'UpdateByUserId';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Name of this State',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'Name';
GO

EXEC sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'ISO code of this State',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocationState',
    @level2type = N'COLUMN',
    @level2name = N'Code';
GO