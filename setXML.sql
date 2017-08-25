USE [system]
update [dbo].[apiLog] set 
[logXml] = '{1}'
where BubbleID = '{0}'