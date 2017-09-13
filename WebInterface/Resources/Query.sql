declare @dd int
declare @mm int
declare @yy int

declare @environment varchar(24) --mandatory
set @environment = 'demo'

declare @endpoint varchar(255) --mandatory
set @endpoint = 'basda-order-v3.ashx'

set @dd = datepart(day, getdate())
set @mm = datepart(MONTH, getdate())
set @yy = datepart(year, getdate())

select '', (
	SELECT 
		[displayTime] as "@Time"
		  ,[BubbleID] as "@BubbleID"
      
		  --,[method]
		  --,[endpoint]
		  ,[severity] as "@Severity"
		  ,[fKey] as "@fkey"
		  --,[logXml]
		  --,[logText]


	  FROM [system].[dbo].[apiLog]
	  where 0=0
		  and [logYear] = @yy
		  and [logMonth] = @mm
		  and [logDay]= @dd
		  and [environment] = @environment
		  and [endpoint] = @endpoint 

		order by logTime DESC
		for XML PATH('item'), type)
for XML PATH('log'), type