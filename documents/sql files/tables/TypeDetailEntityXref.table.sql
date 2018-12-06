-- // /////////////////////////////////////////////////////////////////////////////////////
-- //                           Copyright (c) 2012 - 2012
-- //                            Coyote Logistics L.L.C.
-- //                          All Rights Reserved Worldwide
-- //
-- // WARNING:  This program (or document) is unpublished, proprietary
-- // property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
-- // Unauthorized reproduction, distribution or disclosure of this program
-- // (or document), or any program (or document) derived from it is
-- // prohibited by State and Federal law, and by local law outside of the U.S.
-- // //////////////////////////////////////////////////////////////////////////////////////
CREATE TABLE [dbo].[TypeDetailEntityXref]
    (
      [ID] [int] IDENTITY(1, 1) NOT FOR REPLICATION NOT NULL ,
      [TypeCategoryID] [int] NOT NULL ,
      [TypeDetailID] [int] NOT NULL ,
      [EntityID] [int] NOT NULL ,
      [EntityType] [int] NOT NULL ,
      [CreateDate] [datetime] NOT NULL ,
      [CreateByUserID] [int] NOT NULL ,
      [UpdateDate] [datetime] NOT NULL ,
      [UpdateByUserID] [int] NOT NULL
    )

GO
