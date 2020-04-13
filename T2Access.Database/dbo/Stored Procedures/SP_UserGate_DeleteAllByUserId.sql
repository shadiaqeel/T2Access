

CREATE PROCEDURE [dbo].[SP_UserGate_DeleteAllByUserId]  
   @UserId uniqueidentifier
AS 
   BEGIN

      DELETE 
      FROM dbo.usergate
      WHERE UserId = @UserId

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_UserGate_DeleteAllByUserId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_UserGate_DeleteAllByUserId';

