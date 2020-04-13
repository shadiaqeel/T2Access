/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_Transaction_GetByGateId]  
   @GateId uniqueidentifier,
   @Status int
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

      SELECT TOP (1) 
         Id, 
         UserId, 
         GateId, 
        Status, 
        StatusDate
      FROM dbo.[transaction]
      WHERE GateId = @GateId AND Status = @Status
         ORDER BY CreatedDate DESC

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Transaction_GetByGateId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Transaction_GetByGateId';

