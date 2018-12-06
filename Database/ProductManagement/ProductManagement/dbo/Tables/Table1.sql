CREATE TABLE [dbo].[Table1] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Table1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

