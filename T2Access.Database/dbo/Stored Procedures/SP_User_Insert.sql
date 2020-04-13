


Create PROCEDURE [dbo].[SP_User_Insert]  
@Username nvarchar(255),
@Password nvarchar(255),
@Firstname nvarchar(255),
@Lastname varchar(255)
AS	
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

    DECLARE @Id  uniqueidentifier = newid();  

      INSERT dbo.gate(
         dbo.gate.Id, 
         dbo.gate.Username, 
         dbo.gate.Password, 
         dbo.gate.NameAr, 
         dbo.gate.NameEn)
         VALUES (
            @Id, 
            @Username, 
            @Password, 
            @Firstname, 
            @Lastname);

		SELECT @Id;
 

   END