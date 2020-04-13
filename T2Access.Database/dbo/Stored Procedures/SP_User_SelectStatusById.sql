
CREATE PROCEDURE [dbo].[SP_User_SelectStatusById]  
   @Id uniqueidentifier = Null
AS 
   BEGIN

      SELECT Status
      FROM [User]
      WHERE (@Id IS NOT NULL AND Id = @Id )
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_SelectStatusById', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_SelectStatusById';

