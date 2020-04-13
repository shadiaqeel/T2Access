

CREATE PROCEDURE [dbo].[SP_Gate_SelectStatusById]  
   @Id uniqueidentifier
AS 
   BEGIN


      SELECT gate.Status
      FROM dbo.gate
      WHERE (@Id IS NOT NULL AND gate.Id = @Id)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_SelectStatusById', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_SelectStatusById';

