/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2014 - 2018
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

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT  @ReturnCode = 0

--Delete job if it already exists
IF EXISTS ( SELECT  job_id
            FROM    msdb.dbo.sysjobs_view
            WHERE   name = N'_Archive_LoadTracking' ) 
    EXEC msdb.dbo.sp_delete_job 
        @job_name = N'_Archive_LoadTracking'
      , @delete_unused_schedule = 1;
GO

-- Ensure job category exists
IF NOT EXISTS ( SELECT  name
                FROM    msdb.dbo.syscategories
                WHERE   name = N'Bazooka'
                        AND category_class = 1 ) 
    BEGIN
        EXEC @ReturnCode = msdb.dbo.sp_add_category 
            @class = N'JOB'
          , @type = N'LOCAL'
          , @name = N'Bazooka';
        IF (@@ERROR <> 0
            OR @ReturnCode <> 0
           ) 
            GOTO QuitWithRollback

    END

-- Create Job
DECLARE @jobId BINARY(16)
EXEC @ReturnCode = msdb.dbo.sp_add_job 
    @job_name = N'_Archive_LoadTracking'
  , @enabled = 0
  , @notify_level_eventlog = 0
  , @notify_level_email = 2
  , @notify_level_netsend = 0
  , @notify_level_page = 0
  , @delete_level = 0
  , @description=N'Archive Records From Bazooka.LoadTracking Table to BazookaArchive.LoadTracking table', 
  , @category_name = N'Bazooka'
  , @owner_login_name = N'sa'
  , @notify_email_operator_name = N'DBAAlerts'
  , @job_id = @jobId OUTPUT;
IF (@@ERROR <> 0
    OR @ReturnCode <> 0
   ) 
    GOTO QuitWithRollback

-- Add job step
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep 
    @job_id = @jobId
  , @step_name = N'Archive LoadTracking Records.'
  , @step_id = 1
  , @cmdexec_success_code = 0
  , @on_success_action = 1
  , @on_success_step_id = 0
  , @on_fail_action = 2
  , @on_fail_step_id = 0
  , @retry_attempts = 0
  , @retry_interval = 0
  , @os_run_priority = 0
  , @subsystem = N'TSQL'
  , @command = N'EXEC [dbo].[spArchive_LoadTracking] '
  , @database_name = N'Bazooka'
  , @flags = 4;
IF (@@ERROR <> 0
    OR @ReturnCode <> 0
   ) 
    GOTO QuitWithRollback

-- Assign previous step to be first in job execution
EXEC @ReturnCode = msdb.dbo.sp_update_job 
    @job_id = @jobId
  , @start_step_id = 1;
IF (@@ERROR <> 0
    OR @ReturnCode <> 0
   ) 
    GOTO QuitWithRollback;

-- Add schedule for job
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule 
    @job_id = @jobId
  , @name = N'Daily at 2000'
  , @enabled = 1
  , @freq_type = 4
  , @freq_interval = 1
  , @freq_subday_type = 1
  , @freq_subday_interval = 0
  , @freq_relative_interval = 0
  , @freq_recurrence_factor = 0
  , @active_start_date=20171003
  , @active_end_date=99991231
  , @active_start_time=200000 
  , @active_end_time=235959 	
IF (@@ERROR <> 0
    OR @ReturnCode <> 0
   ) 
    GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobserver 
    @job_id = @jobId
  , @server_name = N'(local)';
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