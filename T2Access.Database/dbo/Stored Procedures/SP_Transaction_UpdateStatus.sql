
CREATE PROCEDURE [dbo].[SP_Transaction_UpdateStatus]  
   @Id numeric(18, 0)
AS 
   BEGIN

      UPDATE dbo.[transaction]
         SET 
            StatusDate = getdate(), 
            status = Status + 1
      WHERE Id = @Id

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Transaction_UpdateStatus', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Transaction_UpdateStatus';

