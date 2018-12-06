CREATE TABLE [dbo].[Address] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [EntityType]  INT           NOT NULL,
    [EntityId]    INT           NOT NULL,
    [City]        VARCHAR (100) NULL,
    [State]       VARCHAR (100) NULL,
    [Country]     VARCHAR (100) NULL,
    [AddressLine] VARCHAR (100) NULL,
    [Street]      VARCHAR (100) NULL,
    [AddressType] INT           NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Address_Address] FOREIGN KEY ([Id]) REFERENCES [dbo].[Address] ([Id])
);



