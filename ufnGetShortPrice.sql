IF OBJECT_ID (N'dbo.ufnGetShortNamePrice', N'FN') IS NOT NULL  
    DROP FUNCTION ufnGetShortPrice;  
GO  
CREATE FUNCTION dbo.ufnGetShortPrice(@ListingId int)  
RETURNS varchar(100)   
AS   
-- Returns the stock level for the product.  
BEGIN  
    DECLARE @ret varchar(100);  
    
	declare @ME VARCHAR(255) = '%[^0-9,]%' --, @ME2 varchar(255)='%[0-9]%' --@MatchExpression
	declare @si int =1, @li int=2
	Set @ret=(Select 
		Replace(case when Num=0 then '' else DisplayPrice end,Num1, (case 
		when num / 1000000000 >= 1 then format(num,'0,,,.00B')
        when num / 1000000 >= 1 then format(num,'0,,.00M') 
        when num / 1000 >= 1 and num%1000>=1 then format(num,'0,.00K') 
		when num / 1000 >= 1 and num%1000=0 then format(num,'0,K') 
		--when num / 100 >= 1 and num%10>=1 then format(num,'0.00') 
		--when num / 100 >= 1 and num%10=0 then format(num,'0')
		when num=0.00 then '' --Removed DisplayPrice
        else format(num,'0')  end)) as ShortPrice
		from
		(
			SELECT 	DisplayPrice,
			SUBSTRING(
				DisplayPrice, 
				(case when CHARINDEX('$',DisplayPrice)=0 then 0
				else CHARINDEX('$',DisplayPrice)+@si end), 
				CASE 
					WHEN PATINDEX(@ME,SUBSTRING(DisplayPrice,CHARINDEX('$',DisplayPrice)+@li, LEN(DisplayPrice))) = 0  THEN  LEN(DisplayPrice) -1 
					ELSE PATINDEX(@ME,SUBSTRING(DisplayPrice,CHARINDEX('$',DisplayPrice)+@li, LEN(DisplayPrice))) 
				END
			) 
			AS Num1,
			Cast(Replace(SUBSTRING(
				DisplayPrice, 
				(case when CHARINDEX('$',DisplayPrice)=0 then 0
				else CHARINDEX('$',DisplayPrice)+@si end), 
				CASE 
					WHEN PATINDEX(@ME,SUBSTRING(DisplayPrice,CHARINDEX('$',DisplayPrice)+@li, LEN(DisplayPrice))) = 0  THEN  LEN(DisplayPrice) -1 
					ELSE PATINDEX(@ME,SUBSTRING(DisplayPrice,CHARINDEX('$',DisplayPrice)+@li, LEN(DisplayPrice))) 
				END
			),',','') as money)
			AS Num
			FROM 
			dbo.Listings where ListingId=@ListingId
		) 
		as #View )
    
	IF (@ret IS NULL)   
        SET @ret = '';  

    RETURN @ret;  
END; 