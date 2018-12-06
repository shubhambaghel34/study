/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2013 - 2015
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
*/
CREATE TABLE [dbo].[LocationStateCountry](
	[LocationStateCountryId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL
		 CONSTRAINT PK_LocationStateCountry PRIMARY KEY CLUSTERED (LocationStateCountryId),
	[CreateDate] [datetime] NOT NULL
		CONSTRAINT DF_LocationStateCountry_01 DEFAULT getdate(),
	[CreateByUserId] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
		CONSTRAINT DF_LocationStateCountry_02 DEFAULT getdate(),
	[UpdateByUserId] [int] NOT NULL,		 
	[LocationStateId] [int] NULL,
	[LocationCountryId] [int] NOT NULL	
	);
GO

CREATE NONCLUSTERED INDEX [ix_LocationStateCountry_01] 
ON [dbo].[LocationStateCountry] ([LocationStateId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = ON, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90)
GO
