-- see process running or sleeping 
SELECT * FROM sys.sysprocesses WHERE open_tran = 1

-- search for column, table or schema
select 
  s.name 'Schema', 
  t.name 'Table', 
  c.name 'Column'
from sys.columns c 
join sys.tables t on c.object_id = t.object_id
join sys.schemas s on s.schema_id = t.schema_id
where c.name LIKE '%desc%'
order by [Schema], [Table], [Column]

-- see queries executed
SELECT 
  execquery.last_execution_time AS [Date Time], 
  execsql.text AS [Script] 
FROM sys.dm_exec_query_stats AS execquery
  CROSS APPLY sys.dm_exec_sql_text(execquery.sql_handle) AS execsql
WHERE execquery.last_execution_time < '2019-12-04'
ORDER BY execquery.last_execution_time DESC
