use system
declare @form bigint --mandatory
set @form = 1
SELECT
	ENAME AS "@name",
	TITLE AS "@title", 		
	INS as "@ins",		
	UPD as "@upd",
	DEL as "@del",
		(SELECT CNAME     AS "@name", 
				CTITLE    AS "@title",						   
				CPOS      AS "@pos",
				CREADONLY AS "@readonly", 
				CHIDE     AS "@hidden", 
				MANDATORY AS "@mandatory", 
				REGEX     AS "@regex",
				BARCODE2D AS "@barcode2d"
		FROM   dbo.FormColumns(@form) 
		FOR xml path('column'), type) ,
		(
			select dbo.FormDef(SONFORM)
			from FORMLINKS
			WHERE FATFORM = @form
			FOR xml path('ChildForms'), type
		)				  				    
				  
		FROM dbo.T$EXEC
		WHERE dbo.T$EXEC.T$EXEC = @form
FOR xml path('form'), type