SELECT
	t.name AS NomeTabela,
	c.name AS NomeColuna,
	c.is_identity AS AutoIncremente,
	d.name AS TipoDados,
	fk.name AS NomeFK,
	OBJECT_NAME(fk.referenced_object_id) AS TabelaReferencia
FROM SYS.TABLES AS t
	INNER JOIN SYS.COLUMNS c ON t.OBJECT_ID = c.OBJECT_ID
	INNER JOIN SYS.TYPES d ON c.system_type_id = d.system_type_id
	LEFT JOIN SYS.FOREIGN_KEYS fk ON t.object_id = fk.parent_object_id
WHERE c.column_id = 1 --pega somente a coluna de ID
ORDER BY t.name, c.name