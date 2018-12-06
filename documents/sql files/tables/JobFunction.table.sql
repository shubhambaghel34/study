/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
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
CREATE TABLE [dbo].[JobFunction](
	[JobFunctionID]	[INT] IDENTITY(0,1) NOT FOR REPLICATION NOT NULL,
		CONSTRAINT PK_JobFunction PRIMARY KEY CLUSTERED ( [JobFunctionID] ),
	[Code]			[VARCHAR](50)		NOT NULL,
	[Description]	[VARCHAR](50)		NOT NULL,
	[CreateDate] [DATETIME] NULL,
	[CreateByUserID] [INT] NULL,
	[UpdateDate] [DATETIME] NULL,
	[UpdateByUserID] [INT] NULL
)
GO


EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'JobFunction table stores JobFunctions to be applied to InternalEmployees to help better represent data from Ultipro. Each JobTitle should be mapped to only one JobFunction',
@level0type = N'SCHEMA',
@level0name = N'dbo',
@level1type = N'TABLE',
@level1name = N'JobFunction';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'JobFunctionId is the primary key and identity of the table',
@level0type = N'SCHEMA',
@level0name = N'dbo',
@level1type = N'TABLE',
@level1name = N'JobFunction',
@level2type = N'COLUMN',
@level2name = N'JobFunctionID';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'Code represents the Job Function Code provided by Ultipro',
@level0type = N'SCHEMA',
@level0name = N'dbo',
@level1type = N'TABLE',
@level1name = N'JobFunction',
@level2type = N'COLUMN',
@level2name = N'Code';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'Description represents the Job Function Description provided by Ultipro',
@level0type = N'SCHEMA',
@level0name = N'dbo',
@level1type = N'TABLE',
@level1name = N'JobFunction',
@level2type = N'COLUMN',
@level2name = N'Description';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'Id value of the user that created the Job Function',
@level0type = N'SCHEMA', 
@level0name = N'dbo', 
@level1type = N'TABLE', 
@level1name = N'JobFunction', 
@level2type = N'Column', 
@level2name = N'CreateByUserId';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'Date and Time the Job Function was created.',
@level0type = N'SCHEMA', 
@level0name = N'dbo', 
@level1type = N'TABLE', 
@level1name = N'JobFunction', 
@level2type = N'Column', 
@level2name = N'CreateDate';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'Id value of the user that last updated the Job Function',
@level0type = N'SCHEMA', 
@level0name = N'dbo', 
@level1type = N'TABLE', 
@level1name = N'JobFunction', 
@level2type = N'Column', 
@level2name = N'UpdateByUserId';
GO

EXEC sys.sp_addextendedproperty
@name = N'MS_Description',
@value = N'The last Date and Time the Job Function was updated.',
@level0type = N'SCHEMA', 
@level0name = N'dbo', 
@level1type = N'TABLE', 
@level1name = N'JobFunction', 
@level2type = N'Column', 
@level2name = N'UpdateDate';
GO
