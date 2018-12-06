CREATE TABLE [dbo].[Table3] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Table2Id] INT           NOT NULL,
    [LastName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Table3] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Table3_Table2_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Table2] ([Id])
);

