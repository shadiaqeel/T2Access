

CREATE PROCEDURE [dbo].[SP_User_SelectWithFilter]  
   @Username nvarchar(255),
   @Firstname nvarchar(255),
   @Lastname nvarchar(255),
   @Status int
AS 
   BEGIN

      SELECT 
         Id, 
         Username, 
         Firstname, 
         Lastname, 
         Status
      FROM [User]
      WHERE 
         (@Username = '' OR Username LIKE (N'%') + (@Username) + (N'%')) AND 
         (@Firstname = '' OR Firstname LIKE (N'%') + (@Firstname) + (N'%')) AND 
         (@Lastname = '' OR Lastname = @Lastname ) AND 
         (@Status = -1 OR Status = @Status)
         ORDER BY createdDate DESC
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_SelectWithFilter', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_SelectWithFilter';

