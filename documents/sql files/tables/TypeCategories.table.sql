CREATE TABLE [dbo].[TypeCategories] (
    [ID]             INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]           VARCHAR (100) NULL,
    [CreateByUserID] INT           NULL,
    [CreateDate]     DATETIME      NULL,
    [UpdateByUserID] INT           NULL,
    [UpdateDate]     DATETIME      NULL
);

