declare @part varchar(32) --mandatory

select '', (
	select	PARTNAME
	from PART
	where PARTNAME in (@part)
	for XML PATH('parts'), type)
for XML PATH('result'), type