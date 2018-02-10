-- Testing a Clustered Columnstore Index and comparing to Row store data compression

-- Columnstore Indexes Guide
-- https://msdn.microsoft.com/en-us/library/gg492088.aspx

-- Using Clustered Columnstore Indexes
-- https://msdn.microsoft.com/en-us/library/dn589807(v=sql.120).aspx

-- Switch to correct database
USE TestingWorks;
GO

-- Three new tables have been added to the TestingWorks database
-- dbo.bigTransactionHistory				- Uses Row store with no data compression
-- dbo.bigTransactionHistoryPageCompressed  - Uses Row store PAGE data compression
-- dbo.bigTransactionHistoryCCSI			- Uses a clustered columnstore index

-- Each table has identical data and a row count of 62.5 million rows

-- You can use any large table for similar testing

-- Turn on Graphical Execution Plan (Ctrl + M)

-- Look at columnstore row groups
-- Maximum number of rows per rowgroup is 1,048,576 rows
SELECT OBJECT_NAME(object_id) AS [Oobject Name], 
       index_id, state_description, total_rows 
FROM sys.column_store_row_groups;

SET STATISTICS IO ON;

SET NOCOUNT ON;

-- Flush dirty pages to disk
CHECKPOINT;

-- Flush buffer pool (don't do this in Production)
-- This will force SQL Server to do physical reads
DBCC DROPCLEANBUFFERS;

-- Query 1 *******************************
DECLARE @StartTime datetime = GETDATE();

-- Force a clustered index scan from row store with no data compression
SELECT COUNT(*) AS [Row Count]
FROM dbo.bigTransactionHistory WITH (INDEX(0));

DECLARE @EndTime datetime = GETDATE();
DECLARE @ElapsedTime int = DATEDIFF(ms, @StartTime, @EndTime);
SELECT @ElapsedTime AS [Query 1 Elapsed Time in ms]
PRINT 'Query 1 Done';
PRINT ' ';



-- Query 2 ********************************
SET @StartTime = GETDATE();

-- Force a clustered index scan from row store with PAGE compression
SELECT COUNT(*) AS [Row Count]
FROM dbo.bigTransactionHistoryPageCompressed WITH (INDEX(0));

SET @EndTime = GETDATE();
SET @ElapsedTime = DATEDIFF(ms, @StartTime, @EndTime);
SELECT @ElapsedTime AS [Query 2 Elapsed Time in ms]
PRINT 'Query 2 Done';
PRINT ' ';

-- Query 3 ********************************
SET @StartTime = GETDATE();

-- Force an index scan from clustered columnstore index 
SELECT COUNT(*) AS [Row Count]
FROM dbo.bigTransactionHistoryCCSI WITH (INDEX(0));

SET @EndTime = GETDATE();
SET @ElapsedTime = DATEDIFF(ms, @StartTime, @EndTime);
SELECT @ElapsedTime AS [Query 3 Elapsed Time in ms]
PRINT 'Query 3 Done';
PRINT ' ';


-- Query 1 Elapsed time 4876ms, 66% of batch cost
-- Table 'bigTransactionHistory'. Scan count 9, logical reads 288048, physical reads 1, read-ahead reads 287493, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

-- Query 2 Elapsed time 2133ms, 32% of batch cost  
-- Table 'bigTransactionHistoryPageCompressed'. Scan count 9, logical reads 121337, physical reads 1, read-ahead reads 120824, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.

-- Query 3 Elapsed time 123ms, 3% of batch cost
-- Table 'bigTransactionHistoryCCSI'. Scan count 8, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 975, lob physical reads 1, lob read-ahead reads 3.
-- Table 'Worktable'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.
-- Table 'Workfile'. Scan count 0, logical reads 0, physical reads 0, read-ahead reads 0, lob logical reads 0, lob physical reads 0, lob read-ahead reads 0.