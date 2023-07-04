-- Index Suggestions:
--WARNING: Do not immediately trust the create script.  Ensure you review the column orderings and also the workload to make sure the index is required.
--The views in SQL are not the law on indexes, just a guide that maybe will help
SELECT ROUND(dm_db_missing_index_group_stats.avg_total_user_cost * dm_db_missing_index_group_stats.avg_user_impact * (dm_db_missing_index_group_stats.user_seeks + dm_db_missing_index_group_stats.user_scans), 0) AS [Total Cost],
	dm_db_missing_index_details.[statement], dm_db_missing_index_details.equality_columns, dm_db_missing_index_details.inequality_columns, dm_db_missing_index_details.included_columns,
	dm_db_missing_index_group_stats.user_seeks, dm_db_missing_index_group_stats.last_user_seek, dm_db_missing_index_group_stats.system_seeks, dm_db_missing_index_group_stats.last_system_seek,
	dm_db_missing_index_group_stats.user_scans, dm_db_missing_index_group_stats.last_user_scan, dm_db_missing_index_group_stats.system_scans, dm_db_missing_index_group_stats.last_system_scan,
	dm_db_missing_index_group_stats.unique_compiles,
	'CREATE INDEX [IX_' + LEFT (PARSENAME(dm_db_missing_index_details.statement, 1), 32)
		+ ISNULL('_' + REPLACE(SUBSTRING(dm_db_missing_index_details.equality_columns, 2, LEN(dm_db_missing_index_details.equality_columns) - 2), '], [', '_'), '')
		+ ISNULL('_' + REPLACE(SUBSTRING(dm_db_missing_index_details.inequality_columns, 2, LEN(dm_db_missing_index_details.inequality_columns) - 2), '], [', '_'), '')
		+ ISNULL('_Include_' + REPLACE(SUBSTRING(dm_db_missing_index_details.included_columns, 2, LEN(dm_db_missing_index_details.included_columns) - 2), '], [', '_'), '')
		+ '] ON ' + dm_db_missing_index_details.statement 
		+ ' (' + ISNULL (dm_db_missing_index_details.equality_columns,'') 
		+ CASE WHEN dm_db_missing_index_details.equality_columns IS NOT NULL AND dm_db_missing_index_details.inequality_columns IS NOT NULL THEN ',' ELSE '' END 
		+ ISNULL (dm_db_missing_index_details.inequality_columns, '')
		+ ')' 
		+ ISNULL (' INCLUDE (' + dm_db_missing_index_details.included_columns + ')', '')
		+ ' WITH FILLFACTOR=XXXX' AS create_index_statement
FROM sys.dm_db_missing_index_groups
INNER JOIN sys.dm_db_missing_index_group_stats ON dm_db_missing_index_group_stats.group_handle = dm_db_missing_index_groups.index_group_handle
INNER JOIN sys.dm_db_missing_index_details ON dm_db_missing_index_details.index_handle = dm_db_missing_index_groups.index_handle
--where database_id=DB_ID('SonyBMGDDEX')
--ORDER BY [Total Cost] DESC
order by [statement]
