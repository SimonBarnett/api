declare @user varchar(32) --mandatory
set @user ='Simon'

select 
	'hello' as "@test",
	@user as "@name" 
for XML PATH('result'), type