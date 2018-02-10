	-- SQL Server Data Compression Examples

	-- SQL Server 2008, 2008 R2, 2012, 2014, and 2016 Data Compression Estimation Queries
    -- This may take some time to run, depending on your hardware infrastructure and table size

	-- When you compress an object (index or table or partition), there are two components of the space savings 
	-- The first component is fragmentaton (i.e. the original object might have been fragmented) 
	-- The object gets defragmented as part of doing compression so you get some space savings 
	-- The second component is the actual data compression savings 

	-- If you specify the current compression setting with sp_estimate_data_compression_savings,
	-- you will see the saving from just defragmenting the index

	-- Get estimated data compression savings and other index info for every index in the specified table
    SET NOCOUNT ON;

    DECLARE @SchemaName sysname = N'dbo';                                -- Specify schema name
    DECLARE @TableName sysname = N'OnlineSearchHistoryNonCompressed';    -- Specify table name

    DECLARE @FullName sysname = @SchemaName + '.' + @TableName;
    DECLARE @IndexID int = 1;
    DECLARE @CompressionType nvarchar(60) = N'PAGE';                 -- Specify desired data compression type (PAGE, ROW, or NONE)
    SET @FullName = @SchemaName + N'.' + @TableName;
 
    -- Get Table name, row count, and compression status for clustered index or heap table
    SELECT OBJECT_NAME(object_id) AS [Object Name], 
    SUM(Rows) AS [RowCount], data_compression_desc AS [Compression Type]
    FROM sys.partitions WITH (NOLOCK)
    WHERE index_id < 2
    AND OBJECT_NAME(object_id) = @TableName
    GROUP BY object_id, data_compression_desc
    ORDER BY SUM(Rows) DESC;
 
    -- Breaks down buffers used by current table in this database by object (table, index) in the buffer pool
    -- Shows you which indexes are taking the most space in the buffer cache, 
	-- so they might be possible candidates for data compression
    SELECT OBJECT_NAME(p.[object_id]) AS [Object Name],
    p.index_id, COUNT(*)/128 AS [Buffer size(MB)],  COUNT(*) AS [Buffer Count], 
    p.data_compression_desc AS [Compression Type]
    FROM sys.allocation_units AS a WITH (NOLOCK)
    INNER JOIN sys.dm_os_buffer_descriptors AS b WITH (NOLOCK)
    ON a.allocation_unit_id = b.allocation_unit_id
    INNER JOIN sys.partitions AS p WITH (NOLOCK)
    ON a.container_id = p.hobt_id
    WHERE b.database_id = DB_ID()
    AND OBJECT_NAME(p.[object_id]) = @TableName
    AND p.[object_id] > 100
    GROUP BY p.[object_id], p.index_id, p.data_compression_desc
    ORDER BY [Buffer Count] DESC;

 
    -- Get the current and estimated size for every index in specified table
    DECLARE curIndexID CURSOR FAST_FORWARD
    FOR
        -- Get list of index IDs for this table
        SELECT i.index_id
        FROM sys.indexes AS i WITH (NOLOCK)
        INNER JOIN sys.tables AS t WITH (NOLOCK)
        ON i.[object_id] = t.[object_id]
        WHERE t.type_desc = N'USER_TABLE'
        AND OBJECT_NAME(t.[object_id]) = @TableName
        ORDER BY i.index_id;
 
    OPEN curIndexID;
 
    FETCH NEXT FROM curIndexID INTO @IndexID;
 
    -- Loop through every index in the table and run sp_estimate_data_compression_savings
    WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Get current and estimated size for specified index with specified compression type
            EXEC sp_estimate_data_compression_savings @SchemaName, @TableName, @IndexID, NULL, @CompressionType;
 
            FETCH NEXT
            FROM curIndexID
            INTO @IndexID;
        END
    CLOSE curIndexID;
    DEALLOCATE curIndexID;

    -- Index Read/Write stats for this table
    SELECT OBJECT_NAME(s.[object_id]) AS [TableName],
    i.name AS [IndexName], i.index_id,
    SUM(user_seeks) AS [User Seeks], SUM(user_scans) AS [User Scans],
    SUM(user_lookups)AS [User Lookups],
    SUM(user_seeks + user_scans + user_lookups)AS [Total Reads],
    SUM(user_updates) AS [Total Writes]     
    FROM sys.dm_db_index_usage_stats AS s WITH (NOLOCK)
    INNER JOIN sys.indexes AS i WITH (NOLOCK)
    ON s.[object_id] = i.[object_id]
    AND i.index_id = s.index_id
    WHERE OBJECTPROPERTY(s.[object_id],'IsUserTable') = 1
    AND s.database_id = DB_ID()
    AND OBJECT_NAME(s.[object_id]) = @TableName
    GROUP BY OBJECT_NAME(s.[object_id]), i.name, i.index_id
    ORDER BY [Total Writes] DESC, [Total Reads] DESC;

    -- Get basic index information for this table 
	-- (does not include filtered indexes or included columns)
    EXEC sp_helpindex @FullName;

    -- Individual File Sizes and space available for current database
	-- Make sure you have enough free space in your data file(s) to compress index  
    SELECT f.name AS [File Name] , f.physical_name AS [Physical Name],
    CAST((f.size/128.0) AS decimal(15,2)) AS [Total Size in MB],
    CAST(f.size/128.0 - CAST(FILEPROPERTY(f.name, 'SpaceUsed') AS int)/128.0 AS decimal(15,2))
    AS [Available Space In MB], [file_id], fg.name AS [Filegroup Name]
    FROM sys.database_files AS f WITH (NOLOCK)
    LEFT OUTER JOIN sys.data_spaces AS fg WITH (NOLOCK) 
    ON f.data_space_id = fg.data_space_id OPTION (RECOMPILE);
