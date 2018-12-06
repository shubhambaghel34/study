CREATE TABLE [dbo].[SystemUser] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Email] VARCHAR (100) NOT NULL,
    [Code]  VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_SystemUser_Code] UNIQUE NONCLUSTERED ([Code] ASC),
    CONSTRAINT [UK_SystemUser_Email] UNIQUE NONCLUSTERED ([Email] ASC)
);

