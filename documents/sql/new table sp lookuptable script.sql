CREATE TABLE [dbo].[CustomerReferralType]
(
	[CustomerReferralTypeId]			[TINYINT]				NOT NULL		CONSTRAINT [PK_CustomerReferralType] PRIMARY KEY,
	[Code]								[VARCHAR](100)			NOT NULL,
	[Description]						[VARCHAR](150)			NULL,
	[DisplayOrder]						[TINYINT]				NOT NULL
);
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
	@value = N'This table stores types of CustomerReferral',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'CustomerReferralType';
GO

CREATE TABLE [dbo].[CustomerReferral]
(
	[CustomerReferralId]			[INT]					NOT NULL			IDENTITY (1, 1) CONSTRAINT [PK_CustomerReferral_CustomerReferralId] PRIMARY KEY,
	[CreateDate]					[DATETIME]				NOT NULL			CONSTRAINT [DF_CustomerReferral_CreateDate] DEFAULT GETDATE(),
	[CreateDateUTC]					[DATETIME2]				NOT NULL			CONSTRAINT [DF_CustomerReferral_CreateDateUTC] DEFAULT GETUTCDATE(),
	[CreateByUserId]				[INT]					NOT NULL,
	[UpdateDate]					[DATETIME]				NOT NULL			CONSTRAINT [DF_CustomerReferral_UpdateDate] DEFAULT GETDATE(),
	[UpdateDateUTC]					[DATETIME2]				NOT NULL			CONSTRAINT [DF_CustomerReferral_UpdateDateUTC] DEFAULT GETUTCDATE(),
	[UpdateByUserID]				[INT]					NOT NULL,
	[CustomerId]					[INT]					NOT NULL,
	[CustomerReferralTypeId]		[TINYINT]				NOT NULL
);

ALTER TABLE [dbo].[CustomerReferral]
ADD CONSTRAINT [FK_CustomerReferral_SystemUser_CreateByUserId] FOREIGN KEY ([CreateByUserId])
REFERENCES [dbo].[SystemUser]([UserId]);
GO

ALTER TABLE [dbo].[CustomerReferral]
ADD CONSTRAINT [FK_CustomerReferral_SystemUser_UpdateByUserId] FOREIGN KEY ([UpdateByUserId])
REFERENCES [dbo].[SystemUser]([UserId]);
GO

ALTER TABLE [dbo].[CustomerReferral]
ADD CONSTRAINT [FK_CustomerReferral_Customer_CustomerId] FOREIGN KEY ([CustomerId])
REFERENCES [dbo].[Customer] ([ID]);
GO

ALTER TABLE [dbo].[CustomerReferral]
ADD CONSTRAINT [FK_CustomerReferral_CustomerReferralType_CustomerReferralTypeId] FOREIGN KEY ([CustomerReferralTypeId])
REFERENCES [dbo].[CustomerReferralType] ([CustomerReferralTypeId]);
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
	@value = N'This table stores CustomerReferral',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'CustomerReferral';
GO

CREATE PROCEDURE [dbo].[spCustomerReferral_GetByCustomerId]
(
	@CustomerId INT
)
AS
	BEGIN

		SET NOCOUNT ON;
		SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

		SELECT	'CustomerReferral' AS 'Table', 
				[CustomerReferralId],
				[CustomerId],
				[CustomerReferralTypeId]
		FROM	[dbo].[CustomerReferral]
		WHERE	[CustomerId] = @CustomerId;
	END;
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
	@value = N'This stored procedure fetch records of CustomerReferral table for a particular customer.',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'PROCEDURE',
	@level1name = N'spCustomerReferral_GetByCustomerId';
GO

CREATE PROCEDURE [dbo].[spReferenceData_InsertCustomerReferralType]
	@CustomerReferralTypeId TINYINT,
	@Code VARCHAR(100),
	@Description VARCHAR(150),
	@DisplayOrder TINYINT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS ( SELECT	1
					FROM	[dbo].[CustomerReferralType]
					WHERE	[CustomerReferralTypeId] = @CustomerReferralTypeId)
	BEGIN
			INSERT	[dbo].[CustomerReferralType]
					([CustomerReferralTypeId],
					 [Code],
					 [Description],
					 [DisplayOrder]
					)
			VALUES	(@CustomerReferralTypeId,
						@Code,
						@Description,
						@DisplayOrder
					);
	END;
END;
GO

EXEC sys.sp_addextendedproperty
	@name = N'MS_Description',
	@value = N'This stored procedure inserts Customer Referral Type in CustomerReferralType table.',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'PROCEDURE',
	@level1name = N'spReferenceData_InsertCustomerReferralType';
GO

EXEC dbo.spReferenceData_InsertCustomerReferralType 1, 'Cold Call', 'Cold Call', 0;
EXEC dbo.spReferenceData_InsertCustomerReferralType 2, 'Customer Referral', 'Customer Referral', 1;
EXEC dbo.spReferenceData_InsertCustomerReferralType 3, 'Personal Referral', 'Personal Referral', 3;
EXEC dbo.spReferenceData_InsertCustomerReferralType 4, 'Warm Lead', 'Warm Lead', 22;
EXEC dbo.spReferenceData_InsertCustomerReferralType 5, 'UPS Enterprise', 'UPS Enterprise', 8;
EXEC dbo.spReferenceData_InsertCustomerReferralType 6, 'UPS FSAE', 'UPS FSAE', 9;
EXEC dbo.spReferenceData_InsertCustomerReferralType 7, 'UPS ISR', 'UPS ISR', 12;
EXEC dbo.spReferenceData_InsertCustomerReferralType 8, 'UPS National', 'UPS National', 18;
EXEC dbo.spReferenceData_InsertCustomerReferralType 9, 'UPS DCC', 'UPS DCC', 6;
EXEC dbo.spReferenceData_InsertCustomerReferralType 10, 'UPS DCC Overflow', 'UPS DCC Overflow', 7;
EXEC dbo.spReferenceData_InsertCustomerReferralType 11, 'UPS International', 'UPS International', 11;
EXEC dbo.spReferenceData_InsertCustomerReferralType 12, 'UPS NMTM', 'UPS NMTM', 19;
EXEC dbo.spReferenceData_InsertCustomerReferralType 13, 'UPS Middle Market', 'UPS Middle Market', 17;
EXEC dbo.spReferenceData_InsertCustomerReferralType 14, 'UPS Mexico', 'UPS Mexico', 14;
EXEC dbo.spReferenceData_InsertCustomerReferralType 15, 'UPS SCS', 'UPS SCS', 20;
EXEC dbo.spReferenceData_InsertCustomerReferralType 16, 'UPS GFF', 'UPS GFF', 10;
EXEC dbo.spReferenceData_InsertCustomerReferralType 17, 'Meridian One', 'Meridian One', 2;
EXEC dbo.spReferenceData_InsertCustomerReferralType 18, 'UPS Call-in', 'UPS Call-in', 4;
EXEC dbo.spReferenceData_InsertCustomerReferralType 19, 'UPS COE', 'UPS COE', 5;
EXEC dbo.spReferenceData_InsertCustomerReferralType 20, 'UPS Major', 'UPS Major', 13;
EXEC dbo.spReferenceData_InsertCustomerReferralType 21, 'UPS Mexico FF', 'UPS Mexico FF', 15;
EXEC dbo.spReferenceData_InsertCustomerReferralType 22, 'UPS Mexico Other', 'UPS Mexico Other', 16;
EXEC dbo.spReferenceData_InsertCustomerReferralType 23, 'UPS.com', 'UPS.com', 21;