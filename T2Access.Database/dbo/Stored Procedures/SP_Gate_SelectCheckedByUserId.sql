

CREATE PROCEDURE [dbo].[SP_Gate_SelectCheckedByUserId]  
   @UserId uniqueidentifier
AS 
   BEGIN




      SELECT 
		 IIF (ug.UserId IS NOT NULL , CAST(1 AS BIT) , CAST(0 AS BIT)), 
         g.Id, 
         g.Username, 
         g.NameAr, 
         g.NameEn, 
         g.Status
      FROM 
         dbo.gate  AS g 
            LEFT JOIN 
            (
               SELECT GateId, UserId
               FROM dbo.usergate
               WHERE UserId = @UserId
            )  AS ug 
            ON ug.GateId = g.Id
         ORDER BY g.CreatedDate
      



   END
GO
EXECUTE sp_addextendedproperty @name = N'MS_SSMA_SOURCE', @value = N't2access.SP_Gate_SelectCheckedByUserId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'PROCEDURE', @level1name = N'SP_Gate_SelectCheckedByUserId';

