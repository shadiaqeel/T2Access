/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_CheckIfValid]  
   @UserId uniqueidentifier,
   @GateId uniqueidentifier
AS 
   BEGIN





      SELECT usergate.UserId, usergate.GateId, u.Status, g.Status
      FROM 
         ([User] AS u 
            INNER JOIN dbo.usergate 
            ON u.Id = @UserId ) 
            INNER JOIN dbo.gate  AS g 
            ON @GateId = g.Id
      WHERE (@UserId IS NOT NULL AND usergate.UserId = @UserId) AND (@GateId IS NOT NULL AND usergate.GateId = @GateId)
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_CheckIfValid', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_CheckIfValid';

