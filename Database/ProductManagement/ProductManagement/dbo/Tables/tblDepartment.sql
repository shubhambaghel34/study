CREATE TABLE [dbo].[tblDepartment] (
    [ID]             INT           NOT NULL,
    [DepartmentName] NVARCHAR (50) NULL,
    [Location]       NVARCHAR (50) NULL,
    [DepartmentHead] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

