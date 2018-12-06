CREATE TABLE [dbo].[tblEmployee] (
    [ID]           INT           NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [Gender]       NVARCHAR (50) NULL,
    [Salary]       INT           NULL,
    [DepartmentId] INT           NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[tblDepartment] ([ID])
);

