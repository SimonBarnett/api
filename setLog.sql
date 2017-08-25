USE [system]
update [dbo].[apiLog] set 
[severity] = '{2}',
[logText] = '{1}'
where BubbleID = '{0}'