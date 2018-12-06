use guru_dev
sp_helptext spCustomerReferrals_GetByCustomerId

ALTER TABLE [dbo].[CustomerReferral]
drop constraint 
--[PK_CustomerReferral_CustomerReferralId],
[DF_CustomerReferral_CreateDate],
[DF_CustomerReferral_CreateDateUTC],
[DF_CustomerReferral_UpdateDate],
[DF_CustomerReferral_UpdateDateUTC],
--[FK_CustomerReferral_CustomerReferralType_CustomerReferralTypeId],
[FK_CustomerReferral_Customer_CustomerId],
[FK_CustomerReferral_SystemUser_UpdateByUserId],
[FK_CustomerReferral_SystemUser_CreateByUserId]

drop procedure [spReferenceData_InsertCustomerReferralType]
drop procedure [spCustomerReferral_GetByCustomerId]

drop TABLE [dbo].[CustomerReferral]

truncate table [CustomerReferralType]
alter table [CustomerReferralType]drop constraint [PK_CustomerReferralType]
drop TABLE [dbo].[CustomerReferralType]
