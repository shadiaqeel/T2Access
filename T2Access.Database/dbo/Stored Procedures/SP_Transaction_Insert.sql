
CREATE PROCEDURE [dbo].[SP_Transaction_Insert]  
   @UserId uniqueidentifier,
   @GateId uniqueidentifier
AS 
   BEGIN



      INSERT dbo.[transaction](UserId,GateId)
         VALUES (@UserId, @GateId)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Transaction_Insert', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Transaction_Insert';

