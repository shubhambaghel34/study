CREATE TABLE [dbo].[Entity] (
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (500) NULL,
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [EntityType]  INT           NOT NULL,
    CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_EntityType] UNIQUE NONCLUSTERED ([EntityType] ASC)
);



