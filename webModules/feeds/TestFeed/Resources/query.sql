declare @part INT --mandatory

select '', (
	select	PARTNAME
	from PART
	where PART in (@part)
	for XML PATH('parts'), type)
for XML PATH('result'), type