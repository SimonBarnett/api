USE [system]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[apiLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[apiLog](
		[BubbleID] [varchar](64) NOT NULL,
		[logDate] [date] NOT NULL CONSTRAINT [DF_apiLog_logDate]  DEFAULT (getdate()),
		[logTime] [time] NOT NULL CONSTRAINT [DF_apiLog_logTime]  DEFAULT (getdate()),
		[environment] [varchar](50) NOT NULL,
		[method] [nchar](10) NOT NULL,
		[endpoint] [varchar](250) NOT NULL,
		[severity] [int] NULL,
		[logXml] [xml] NULL,
		[logText] [nvarchar](MAX) NULL,
		[fKey] [nvarchar](255),
		[logYear]  AS (datepart(year,[logDate])),
		[logMonth]  AS (datepart(month,[logDate])),
		[logDay]  AS (datepart(day,[logDate])),
		[displayTime]  AS CAST(
			RIGHT('00' + ltrim(str(datepart(hh, [logTime]))),2) +':'+ 
			RIGHT('00' + ltrim(str(datepart(mi, [logTime]))),2) +':'+  
			RIGHT('00' + ltrim(str(datepart(ss, [logTime]))),2) 
		AS VARCHAR(8))
		,
	 CONSTRAINT [PK_apiLog] PRIMARY KEY CLUSTERED 
	(
		[logDate] ASC,
		[BubbleID] ASC
	
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	CREATE UNIQUE NONCLUSTERED INDEX [IX_Env_fKey] ON [dbo].[apiLog]
	(
		[environment] ASC,
		[fKey] ASC
	) where [fKey] is not null
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_endpoint_apiLog] ON [dbo].[apiLog]
	(
		[endpoint] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_env_apiLog] ON [dbo].[apiLog]
	(
		[environment] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_method_apiLog] ON [dbo].[apiLog]
	(
		[method] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

	CREATE NONCLUSTERED INDEX [IX_logTime_apiLog] ON [dbo].[apiLog]
	(
		[logTime] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

END

insert into [dbo].[apiLog] 
	([BubbleID], [environment], [method], [endpoint])
values 
	('{0}', '{1}', '{2}', '{3}')
