/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_User_ResetPassword]  
   @Id uniqueidentifier,
   @Password nvarchar(255)
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON


      UPDATE [User]
         SET 
            Password = 
               CASE 
                  WHEN @Password <> '' THEN @Password
                  ELSE Password
               END
      WHERE [User].Id = @Id 
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_ResetPassword', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_ResetPassword';

