declare @user varchar(32) --mandatory
set @user ='Simon'

select @user as "@name" 
for XML PATH('result'), type