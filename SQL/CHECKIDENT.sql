DBCC CHECKIDENT ("TABLE_NANE") -- Verify the identity level

DBCC CHECKIDENT ("TABLE_NANE", NORESEED) -- Let the value as is
 
DBCC CHECKIDENT ("TABLE_NANE", RESEED, new_identity) -- Change the identity to the new value "new_identity"