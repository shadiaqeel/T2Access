

CREATE PROCEDURE dbo.SP_Gate_Login  
   @Username nvarchar(255)
AS 
   BEGIN



      SELECT 
         gate.Id, 
         gate.Username, 
         gate.Password, 
         gate.NameAr, 
         gate.NameEn, 
         gate.Status
      FROM dbo.gate
      WHERE (@Username <> '' AND gate.Username = @Username)

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_Login', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_Login';

