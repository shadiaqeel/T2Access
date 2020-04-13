

CREATE PROCEDURE [dbo].[SP_Gate_Delete]  
   @Id uniqueidentifier
AS 
   BEGIN


      DELETE 
      FROM dbo.gate
      WHERE Id = @Id

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_Delete';

