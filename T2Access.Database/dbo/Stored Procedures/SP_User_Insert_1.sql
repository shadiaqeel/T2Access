


CREATE PROCEDURE [dbo].[SP_User_Insert]  
@Username nvarchar(255),
@Password nvarchar(255),
@Firstname nvarchar(255),
@Lastname varchar(255)
AS	

    DECLARE @Id  uniqueidentifier 
	SET @Id = NEWID();
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON



      INSERT dbo.[User]( Id, Username,  Password, Firstname,Lastname)
		 OUTPUT INSERTED.Id 
         VALUES (
            NEWID(), 
            @Username, 
            @Password, 
            @Firstname, 
            @Lastname)
			;


 

   END