
CREATE PROCEDURE [dbo].[SP_User_Delete]  
   @Id uniqueidentifier
AS 
   BEGIN

      DELETE 
      FROM [User]
      WHERE id = @Id 
      

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_Delete', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_Delete';

