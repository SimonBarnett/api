declare @CUSTNAME varchar(50)--mandatory
set @CUSTNAME = ''

SELECT '', (
	select CUSTNAME, CUSTDES, (
		select NAME 
		from PHONEBOOK
		where CUST = CUSTOMERS.CUST
		for XML PATH('contacts'), type 
	)
	from CUSTOMERS
	where CUSTNAME = @CUSTNAME
	for XML PATH('customer'), type 
) for XML PATH('customers'), type