/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
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
CREATE TABLE [dbo].[IMDLTrackingExceptionsAudit]
(
	[IMDLTrackingExceptionsAuditId]			[INT]			NOT NULL IDENTITY(1,1) NOT FOR REPLICATION
															CONSTRAINT [PK_IMDLTrackingExceptionsAudit] PRIMARY KEY CLUSTERED,
	[CreateDateUTC]							[DATETIME2]		NOT NULL CONSTRAINT [DF_IMDLTrackingExceptionsAudit_CreateDateUTC] DEFAULT (GETUTCDATE()),
	[CreateByUserId]						[INT]			NOT NULL CONSTRAINT [FK_IMDLTrackingExceptionsAudit_SystemUser_CreateByUserId] 
															FOREIGN KEY([CreateByUserId]) REFERENCES [dbo].[SystemUser] ([UserId]),
	[ExceptionId]							[INT]			NOT NULL,
	[ExceptionCreateDateUTC]				[DATETIME2]		NOT NULL,
	[LoadId]								[INT]			NOT NULL CONSTRAINT [FK_IMDLTrackingExceptionsAudit_Load_LoadId] 
															FOREIGN KEY([LoadId]) REFERENCES [dbo].[Load] ([Id]),
	[ExceptionTypeId]						[TINYINT]		NOT NULL CONSTRAINT [FK_IMDLTrackingExceptionsAudit_IMDLTrackingExceptionType_ExceptionTypeId] FOREIGN KEY([ExceptionTypeId])
															REFERENCES [dbo].[IMDLTrackingExceptionType] ([IMDLTrackingExceptionTypeId]),
	[IsVerified]							[BIT]			NOT NULL CONSTRAINT [DF_IMDLTrackingExceptionsAudit_IsVerified] DEFAULT 0,
	[VerifiedByUserId]						[INT]			NULL CONSTRAINT [FK_IMDLTrackingExceptionsAudit_SystemUser_VerifiedByUserId] 
															FOREIGN KEY([VerifiedByUserId]) REFERENCES [dbo].[SystemUser] ([UserId]),
	[VerifiedDateUTC]						[DATETIME]		NULL, 
	[ResolvedDateUTC]						[DATETIME]		NULL,
	[ResolvedByUserId]						[INT]			NULL CONSTRAINT [FK_IMDLTrackingExceptionsAudit_SystemUser_ResolvedByUserId] 
															FOREIGN KEY([ResolvedByUserId]) REFERENCES [dbo].[SystemUser] ([UserId]),
	[ProgressType]							[INT]			NULL,
	[IMDLRepId]								[INT]			NULL,
);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description'
  , @value = N'This table stores IMDLTrackingExceptions table''s history.'
  , @level0type = N'SCHEMA'
  , @level0name = N'dbo'
  , @level1type = N'TABLE'
  , @level1name = N'IMDLTrackingExceptionsAudit';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This column stores the IMDL Load Exception TypeId.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'ExceptionTypeId';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This column states whether the IMDL Load Exception has been verified.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'IsVerified';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This Column stores the Rep Id of the rep who has verified the IMDL Load Exception.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'VerifiedByUserId';
GO
EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This Column stores the verified datetime of the IMDL Load Exception.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'VerifiedDateUTC';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This Column stores the resolved datetime of the IMDL Load Exception.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'ResolvedDateUTC';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This column stores Progress type of the Load.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'ProgressType';
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This column stores RepId of the LoadCarrier.' , 
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'IMDLTrackingExceptionsAudit',
	@level2type = N'Column' , 
	@level2name = N'IMDLRepId';
GO