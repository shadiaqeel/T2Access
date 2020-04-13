/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE [dbo].[SP_User_Update]  
   @Id uniqueidentifier,
   @Username nvarchar(255),
   @Firstname nvarchar(255),
   @Lastname nvarchar(255),
   @Status int
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON



      UPDATE [User]
         SET 
            Username = 
               CASE 
                  WHEN @Username <> '' THEN @Username
                  ELSE [User].Username
               END, 
            Firstname = 
               CASE 
                  WHEN @Firstname <> '' THEN @Firstname
                  ELSE [User].Firstname
               END, 
            Lastname = 
               CASE 
                  WHEN @Lastname <> '' THEN @Lastname
                  ELSE [User].Lastname
               END, 
            Status = 
               CASE 
                  WHEN @Status <> -1 THEN @Status
                  ELSE [User].Status
               END
      WHERE [User].Id = @Id 
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_User_Update', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_User_Update';

