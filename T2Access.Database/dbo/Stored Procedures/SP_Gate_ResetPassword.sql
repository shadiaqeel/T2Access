
CREATE PROCEDURE [dbo].[SP_Gate_ResetPassword]  
   @Id uniqueidentifier,
   @Password nvarchar(255)
AS 
   BEGIN



      UPDATE dbo.gate
         SET 
            Password = 
               CASE 
                  WHEN @Password <> '' THEN @Password
                  ELSE gate.Password
               END
      WHERE gate.Id = @Id

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_ResetPassword', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_ResetPassword';

