

CREATE PROCEDURE [dbo].[SP_UserGate_GetByUserId]  
   @UserId uniqueidentifier
AS 
   BEGIN


      SELECT usergate.GateId
      FROM dbo.usergate
      WHERE (@UserId <> '00000000-0000-0000-0000-000000000000' AND UserId = @UserId)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_UserGate_GetByUserId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_UserGate_GetByUserId';

