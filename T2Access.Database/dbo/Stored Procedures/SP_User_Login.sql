
CREATE PROCEDURE [dbo].[SP_User_Login]  
   @Username nvarchar(255)
AS 
   BEGIN

      
      SELECT 
         Id, 
         Username, 
         Password, 
         Firstname, 
         Lastname, 
         Status
      FROM [User]
      WHERE (@Username <> '' AND Username = @Username )
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_Login', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_Login';

