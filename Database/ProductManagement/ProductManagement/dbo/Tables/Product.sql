CREATE TABLE [dbo].[Product] (
    [ProductId]   INT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Code]        VARCHAR (10)    NOT NULL,
    [Name]        VARCHAR (50)    NULL,
    [ReleaseDate] DATETIME        NULL,
    [Description] VARCHAR (MAX)   NULL,
    [Price]       MONEY           NULL,
    [StarRating]  DECIMAL (16, 2) NULL,
    [ImageUrl]    VARCHAR (500)   NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This column indicate the code of product.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Product', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This column indicates description of product', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Product', @level2type = N'COLUMN', @level2name = N'Description';

