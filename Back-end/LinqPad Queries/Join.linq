void Main()
{
	var query = tableA
		.Join(tableB, a => a.Id, b => b.TableAId, (x, y) => x);
}
