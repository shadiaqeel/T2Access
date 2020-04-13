/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_UserGate_Delete]  
   @UserId uniqueidentifier,
   @GateId uniqueidentifier
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

      DELETE 
      FROM dbo.usergate
      WHERE (@UserId = '00000000-0000-0000-0000-000000000000' OR UserId = @UserId) AND (@GateId = '00000000-0000-0000-0000-000000000000' OR GateId = @GateId)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_UserGate_Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_UserGate_Delete';

