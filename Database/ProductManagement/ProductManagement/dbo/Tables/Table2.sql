CREATE TABLE [dbo].[Table2] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [UserId]     INT           NOT NULL,
    [MiddleName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Table2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Table2_Table1_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Table1] ([Id])
);

