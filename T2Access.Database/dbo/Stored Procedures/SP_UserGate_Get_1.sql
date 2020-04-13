/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_UserGate_Get]  
   @UserId uniqueidentifier = null,
   @GateId uniqueidentifier = null
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

      SELECT @UserId, @GateId
      FROM usergate
      WHERE (@UserId IS NOT NULL AND UserId = @UserId) AND (@GateId IS NOT NULL AND GateId = @GateId)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_UserGate_Get', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_UserGate_Get';

