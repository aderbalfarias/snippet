void Main()
{
	var query = tableA
		.GroupJoin(tableB, a => a.Id, b => b.TableAId,
        		(x, y) => new { tbA = x, tbB = y })
       .SelectMany(x => x.tbB.DefaultIfEmpty(), (x, y) => new { A = x.tbA, B = y });
}
