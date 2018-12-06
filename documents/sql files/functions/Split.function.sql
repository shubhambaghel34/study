CREATE FUNCTION [dbo].[Split]
(@str NVARCHAR (4000), @charToSplitOn NCHAR (1))
RETURNS 
     TABLE (
        [Value] NVARCHAR (250) NULL)
AS
 EXTERNAL NAME [BazookaSQLClasses].[BellyFire.Applications.Bazooka.SQLServer.Utilities].[Split]


GO
EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'Split';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'Utilities.vb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'Split';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = 12, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'Split';

