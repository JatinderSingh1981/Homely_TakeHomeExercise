
IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Listings_Suburb_CategoryType_StatusType')   
    DROP INDEX IX_Listings_Suburb_CategoryType_StatusType ON dbo.Listings;   
GO  

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Listings_CategoryType_StatusType')   
    DROP INDEX IX_Listings_CategoryType_StatusType ON dbo.Listings;   
GO  

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_Listings_StatusType')   
    DROP INDEX IX_Listings_StatusType ON dbo.Listings;   
GO  


CREATE NONCLUSTERED INDEX IX_Listings_Suburb_CategoryType_StatusType   
    ON dbo.Listings (Suburb,CategoryType,StatusType)
	INCLUDE (StreetNumber,Street, [State], PostCode, DisplayPrice, ShortPrice,Title);   
GO  

CREATE NONCLUSTERED INDEX IX_Listings_CategoryType_StatusType   
    ON dbo.Listings (CategoryType,StatusType)
	INCLUDE (Suburb,StreetNumber,Street, [State], PostCode, DisplayPrice, ShortPrice,Title);   
GO 


CREATE NONCLUSTERED INDEX IX_Listings_StatusType   
    ON dbo.Listings (StatusType)
	INCLUDE (Suburb,StreetNumber,Street, [State], PostCode, CategoryType, DisplayPrice, ShortPrice,Title);   
GO 


--This query will get us the list of indexes (used or unused) to understand the usage
--SELECT OBJECT_NAME(S.[OBJECT_ID]) AS [OBJECT NAME], 
--       I.[NAME] AS [INDEX NAME], 
--       USER_SEEKS, 
--       USER_SCANS, 
--       USER_LOOKUPS, 
--       USER_UPDATES 
--FROM   SYS.DM_DB_INDEX_USAGE_STATS AS S 
--       INNER JOIN SYS.INDEXES AS I ON I.[OBJECT_ID] = S.[OBJECT_ID] AND I.INDEX_ID = S.INDEX_ID 
--WHERE  OBJECTPROPERTY(S.[OBJECT_ID],'IsUserTable') = 1
--       AND S.database_id = DB_ID()
