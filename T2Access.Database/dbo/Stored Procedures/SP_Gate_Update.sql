
Create PROCEDURE [dbo].[SP_Gate_Update]  
   @Id uniqueidentifier,
   @NameAr nvarchar(255),
   @NameEn nvarchar(255),
   @Status int
AS 
   BEGIN

      UPDATE [Gate]
         SET 
            NameAr = 
               CASE 
                  WHEN @NameAr <> '' THEN @NameAr
                  ELSE NameAr
               END, 
            NameEn = 
               CASE 
                  WHEN @NameEn <> '' THEN @NameEn
                  ELSE NameEn
               END, 
            Status = 
               CASE 
                  WHEN @Status <> -1 THEN @Status
                  ELSE Status
               END
      WHERE Id = @Id 
      



   END