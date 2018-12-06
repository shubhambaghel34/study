/*
// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
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
CREATE VIEW [dbo].[vwCountry]
AS 

SELECT       
	lco.LocationCountryId AS ID, 
	'' AS 'Code', 
	lco.ISOCodeAlpha3 AS 'Alpha3',
	lco.ISOCodeAlpha2 AS 'Alpha2',
	lco.Name AS 'Name', 
	lco.NumericCode AS 'NumericCode', 
	lco.GeoRegionId AS 'GeoRegionId',
	lco.PhoneCode AS 'PhoneCode',
	lco.FIPS AS 'FIPS',
	'' AS 'CegidCode',
	lco.CreateDate AS 'CreateDate',
	lco.CreateByUserId AS 'CreateByUserId',
	lco.UpdateDate AS 'UpdateDate',
	lco.UpdateByUserId AS 'UpdateByUserId'
FROM            
    [dbo].LocationCountry AS lco
