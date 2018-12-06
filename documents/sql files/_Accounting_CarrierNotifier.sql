/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2016
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

USE [msdb]
GO

/****** Object:  Job [_Accounting_CarrierNotifier]    Script Date: 27-04-2015 14:37:49 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0

-- Drop job if exists
IF EXISTS (SELECT job_id FROM sysjobs_view WHERE name = N'_Accounting_CarrierNotifier') BEGIN
	EXEC @ReturnCode = sp_delete_job @job_name=N'_Accounting_CarrierNotifier', @delete_unused_schedule=1;
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
	END

-- Ensure job category exists
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'Bazooka' AND category_class=1)
	BEGIN
	EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'Bazooka'
	IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
	END

-- Create Job
DECLARE @jobId BINARY(16)
EXEC	@ReturnCode =  msdb.dbo.sp_add_job 
		@job_name=N'_Accounting_CarrierNotifier' 
		,@enabled=1 
		,@notify_level_eventlog=0 
		,@notify_level_email=2 
		,@notify_level_netsend=0 
		,@notify_level_page=0 
		,@delete_level=0 
		,@description=N'Carrier Vouchers and Payments Notify to Carrier.' 
		,@category_name=N'Bazooka' 
		,@owner_login_name=N'sa' 
		,@notify_email_operator_name=N'BazookaSupport' 
		,@job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

-- Add job step
EXEC	@ReturnCode = msdb.dbo.sp_add_jobstep 
		@job_id=@jobId 
		,@step_name=N'Run executable for Carrier Voucher and Payment Notifier' 
		,@step_id=1 
		,@cmdexec_success_code=0 
		,@on_success_action=1 
		,@on_success_step_id=0 
		,@on_fail_action=2 
		,@on_fail_step_id=0 
		,@retry_attempts=0 
		,@retry_interval=0 
		,@os_run_priority=0 
		,@subsystem=N'CmdExec' 
		,@command=N'' -- provide executable path
		,@flags=0
IF (@@ERROR <> 0 
	OR @ReturnCode <> 0) 
	GOTO QuitWithRollback

-- Add schedule for job
EXEC	@ReturnCode = msdb.dbo.sp_add_jobschedule 
		@job_id=@jobId 
		,@name=N'Run Every Nightly' 
		,@enabled=1 
		,@freq_type=4 
		,@freq_interval=1 
		,@freq_subday_type=1 
		,@freq_subday_interval=15 
		,@freq_relative_interval=0 
		,@freq_recurrence_factor=0 
		,@active_start_date=20150428
		,@active_end_date=99991231 
		,@active_start_time=20000 
		,@active_end_time=235959;
IF (@@ERROR <> 0 
	OR @ReturnCode <> 0
	) 
	GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobserver 
	 @job_id = @jobId 
	 ,@server_name = N'(local)'
IF (@@ERROR <> 0 
	OR @ReturnCode <> 0
	) 
	GOTO QuitWithRollback

COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
IF (@@TRANCOUNT > 0) 
	ROLLBACK TRANSACTION
EndSave:

GO
