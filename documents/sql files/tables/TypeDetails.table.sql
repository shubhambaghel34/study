CREATE TABLE [dbo].[TypeDetails] (
    [ID]              INT           NOT NULL,
    [TypeCategoryID]  INT           NOT NULL,
    [Code]            VARCHAR (100) NULL,
    [Description]     VARCHAR (150) NULL,
    [ForeignLookupID] INT           NULL,
    [DisplayOrder]    INT           NULL,
    [EDIDataElement]  INT           NULL,
    [Key]             INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CreateDate]      DATETIME      NULL,
    [CreateByUserID]  INT           NULL,
    [UpdateDate]      DATETIME      NULL,
    [UpdateByUserID]  INT           NULL
);

