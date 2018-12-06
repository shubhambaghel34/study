/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
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
CREATE TABLE [dbo].[ServiceToken]
(
      [Name] VARCHAR(75) CONSTRAINT [PK_ServiceToken] PRIMARY KEY NOT NULL ,
	  [PrimaryGuid] VARCHAR(75) NOT NULL
		CONSTRAINT [DF_ServiceToken_PrimaryString] DEFAULT ( '' ),
	  [SecondaryGuid] VARCHAR(75) NOT NULL
		CONSTRAINT [DF_ServiceToken_SecondaryString] DEFAULT ( '' ),
	  [ExpirationDate] DATETIME2 NULL,
	  [UpdateDateUTC] DATETIME2 NOT NULL
        CONSTRAINT [DF_ServiceToken_UpdateDateUTC] DEFAULT ( GETUTCDATE() ) ,
)
