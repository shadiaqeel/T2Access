/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE dbo.SP_Gate_SelectByUserName  
   @Username nvarchar(255)
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

      SELECT 
         gate.Id, 
         gate.Username, 
         gate.NameAr, 
         gate.NameEn, 
         gate.Status
      FROM dbo.gate
      WHERE (gate.Username <> '' AND gate.Username = @Username)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_SelectByUserName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_SelectByUserName';

