

CREATE PROCEDURE [dbo].[SP_Gate_Insert]  
   @Username nvarchar(255),
   @Password nvarchar(255),
   @NameAr nvarchar(255),
   @NameEn nvarchar(255)
AS 
   BEGIN

      INSERT dbo.gate(Id, Username, Password, NameAr,NameEn)
		/*OUTPUT INSERTED.Id*/ 
         VALUES (
            NEWID(), 
            @Username, 
            @Password, 
            @NameAr, 
            @NameEn)

   END



GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_Insert', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_Insert';

