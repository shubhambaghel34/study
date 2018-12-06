CREATE TABLE [dbo].[User] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (100) NOT NULL,
    [LastName]  VARCHAR (100) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Mobile]    VARCHAR (13)  NULL,
    [Code]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Code] UNIQUE NONCLUSTERED ([Code] ASC)
);



