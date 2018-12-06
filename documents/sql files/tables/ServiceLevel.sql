/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2015 - 2016
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
CREATE TABLE [dbo].[ServiceLevel]
    (
      [ServiceLevelId] [TinyINT]
        NOT NULL
        CONSTRAINT [PK_ServiceLevel]
        PRIMARY KEY CLUSTERED ( [ServiceLevelId] ) WITH ( FILLFACTOR = 100 ) ,
      [Code] [VARCHAR](1) NOT NULL ,
      [Description] [VARCHAR](100) NULL 
    );

GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description' , 
	@value = N'This is a lookup table that stores the service level code. ''Service Level'' is a term that UPS operations uses to describe different types of services (i.e. Next Day, Second Day, Third Day, etc).',
	@level0type = N'SCHEMA' , 
	@level0name = N'dbo' , 
	@level1type = N'TABLE' , 
	@level1name = N'ServiceLevel';
GO
