/*
*   SSMA informational messages:
*   M2SS0003: The following SQL clause was ignored during conversion:
*   DEFINER = `root`@`localhost`.
*/

CREATE PROCEDURE dbo.SP_Gate_SelectWithFilter  
   @Username nvarchar(255),
   @NameAr nvarchar(255),
   @NameEn nvarchar(255),
   @Status int
AS 
   BEGIN

      SET  XACT_ABORT  ON

      SET  NOCOUNT  ON

      SELECT 
         g.Id, 
         g.Username, 
         g.NameAr, 
         g.NameEn, 
         g.Status
      FROM dbo.gate  AS g
      WHERE 
         (@Username = '' OR g.Username LIKE N'%' + @Username + N'%') AND 
         (@NameAr = '' OR g.NameAr LIKE N'%' + @NameAr + N'%') AND 
         (@NameEn = '' OR g.NameEn LIKE N'%' + @NameEn + N'%') AND 
         (@Status = -1 OR g.Status = @Status)
         ORDER BY g.CreatedDate

   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_SelectWithFilter', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_SelectWithFilter';

