CREATE TABLE [dbo].[CarrierAttributeGroup]
(
	[CarrierAttributeGroupId]					[INT]				NOT NULL	CONSTRAINT [PK_CarrierDiversityAttributes] PRIMARY KEY IDENTITY,
	[Name]										[VARCHAR](100)		NOT NULL
);
GO

CREATE TABLE [dbo].[CarrierAttributeLookup]
(
	[CarrierAttributeLookupId]					[INT]				NOT NULL	CONSTRAINT [PK_CarrierAttributeLookup] PRIMARY KEY IDENTITY,
	[Name]										[VARCHAR](100)		NOT NULL,
	[Type]										[VARCHAR](100)		NOT NULL,
	[CarrierAttributeGroupId]					[INT]				NOT NULL
);
GO

ALTER TABLE [dbo].[CarrierAttributeLookup]
ADD CONSTRAINT [FK_CarrierAttributeLookup_CarrierAttributeGroup_CarrierAttributeGroupId] FOREIGN KEY ([CarrierAttributeGroupId])
REFERENCES [dbo].[CarrierAttributeGroup]([CarrierAttributeGroupId]);
GO

CREATE TABLE [dbo].[CarrierAttributeXref]
(
	[CarrierAttributeXrefId]					[INT]				NOT NULL	CONSTRAINT [PK_CCarrierAttributeXref] PRIMARY KEY IDENTITY,
	[CreateDate]								[DATETIME]			NOT NULL	CONSTRAINT [DF_CarrierAttributeXref_CreateDate] DEFAULT (GETDATE()),
	[CreateDateUTC]								[DATETIME2]			NOT NULL	CONSTRAINT [DF_CarrierAttributeXref_CreateDateUTC] DEFAULT GETUTCDATE(),
	[CreateByUserId]							[INT]				NOT NULL,
	[UpdateDate]								[DATETIME]			NOT NULL	CONSTRAINT [DF_CarrierAttributeXref_UpdateDate] DEFAULT (GETDATE()),
	[UpdateDateUTC]								[DATETIME2]			NOT NULL	CONSTRAINT [DF_CarrierAttributeXref_UpdateDateUTC] DEFAULT GETUTCDATE(),
	[UpdateByUserId]							[INT]				NOT NULL,
	[CarrierId]									[INT]				NOT NULL,
	[CarrierAttributeLookupId]					[INT]				NOT NULL,
	[Value]										[VARCHAR](1000)		NULL	
);
GO

ALTER TABLE [dbo].[CarrierAttributeXref]
ADD CONSTRAINT [FK_CarrierAttributeXref_SystemUser_CreateByUserId] FOREIGN KEY ([CreateByUserId])
REFERENCES [dbo].[SystemUser]([UserId]);
GO

ALTER TABLE [dbo].[CarrierAttributeXref]
ADD CONSTRAINT [FK_CarrierAttributeXref_SystemUser_UpdateByUserId] FOREIGN KEY ([UpdateByUserId])
REFERENCES [dbo].[SystemUser]([UserId]);
GO

ALTER TABLE [dbo].[CarrierAttributeXref]
ADD CONSTRAINT [FK_CarrierAttributeXref_Carrier_CarrierId] FOREIGN KEY ([CarrierId])
REFERENCES [dbo].[Carrier]([Id]);
GO

ALTER TABLE [dbo].[CarrierAttributeXref]
ADD CONSTRAINT [FK_CarrierAttributeXref_CarrierAttributeLookup_CarrierAttributeLookupId] FOREIGN KEY ([CarrierAttributeLookupId])
REFERENCES [dbo].[CarrierAttributeLookup]([CarrierAttributeLookupId]);
GO


INSERT INTO [CarrierAttributeGroup]
(Name)VALUES('DIversity'),('Ownership')
-------------------------------------------
INSERT INTO [CarrierAttributeLookup]
([Name],[Type],[CarrierAttributeGroupId])
VALUES('SmallBusiness','BIT',1),('HUBZone','BIT',1),('LGBT','BIT',1),('Minority','BIT',2),('Veteran','BIT',2)
----------------------------------------------------------------
INSERT INTO [CarrierAttributeXref]
([CreateByUserId],[UpdateByUserId],[CarrierId],[CarrierAttributeLookupId],[Value])
VALUES(19574,19574,177746,1,'1'),(19574,19574,177746,2,'0'),(19574,19574,177746,3,'1'),(19574,19574,177746,4,'1'),(19574,19574,177746,5,'0')


--CREATE table #yourtable
--    ([Id] int, [Value] varchar(6), [ColumnName] varchar(13))
--;
    
--INSERT INTO #yourtable
--    ([Id], [Value], [ColumnName])
--VALUES
--    (1, 'John', 'FirstName'),
--    (2, '2.4', 'Amount'),
--    (3, 'ZH1E4A', 'PostalCode'),
--    (4, 'Fork', 'LastName'),
--    (5, '857685', 'AccountNumber')
--;
--select Firstname, Amount, PostalCode, LastName, AccountNumber
--from
--(
--  select value, columnname
--  from #yourtable
--) d
--pivot
--(
--  max(value)
--  for columnname in (Firstname, Amount, PostalCode, LastName, AccountNumber)
--) piv;
select SmallBusiness,HUBZone,LGBT,Minority,Veteran
from
(
  select cal.Name,cax.Value
  from  CarrierAttributeLookup cal 
		inner join CarrierAttributeXref cax on cal.CarrierAttributeLookupId = cax.CarrierAttributeLookupId
 ) d
pivot
(
  MAX(Value)
  for Name in (SmallBusiness,HUBZone,LGBT,Minority,Veteran)
) piv;


--CarrierId CarrierAttributeXrefId CarrierId CarrierAttributeLookupId Value
--SmallBusiness
--HUBZone
--LGBT
--Minority
--Veteran
--select cag.Name
  --from  CarrierAttributeGroup cag
		--inner join CarrierAttributesLookup cal on cag.CarrierAttributesGroupId = cal.CarrierAttributesGroupId
		--inner join CarrierAttributesXref cax on cal.CarrierAttributesLookupId = cax.CarrierAttributesLookupId
--drop table #yourtable

select * from carrier where code = 'democar'
select * from [CarrierAttributeGroup]
select * from [CarrierAttributeLookup]
select * from [CarrierAttributeXref]
		SELECT	cag.[Name],
				cag.CarrierAttributeGroupid,
				cal.[Name],
				cal.[Type],
				cax.[Value] 
		FROM	[CarrierAttributeGroup] cag
				INNER JOIN [dbo].[CarrierAttributeLookup] cal 
							ON cal.CarrierAttributeGroupId = cag.CarrierAttributeGroupId 
				INNER JOIN [dbo].[CarrierAttributeXref] cax	
							ON cax.CarrierAttributeLookupId = cal.CarrierAttributeGroupId AND cax.CarrierId = 177746;


select * from [dbo].[SecurityModuleTask]
select * from dbo.SecurityTemplateAccessXref where securitymoduletaskid = 345
select * from [dbo].[SecurityUserOverride] where securitymoduletaskid = 345
delete from [dbo].[SecurityModuleTask] where id = 345
delete from dbo.SecurityTemplateAccessXref where securitymoduletaskid = 345
delete from [dbo].[SecurityUserOverride] where id = 862522
select * from SecurityTemplateAccessXref
