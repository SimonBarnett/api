IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DATETOMIN]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
create FUNCTION [dbo].[DATETOMIN] 
(
	-- Add the parameters for the function here
	@DT as datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar as int

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar = (DATEdiff(MI, ''19880101'',@DT ))

	-- Return the result of the function
	RETURN @ResultVar

END
' 
END