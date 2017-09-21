<Query Kind="Program">
  <Connection>
    <ID>7757c89b-2ec3-4e35-9ad2-91e2bf6a99fc</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Users\aderb\Desktop\GServicer\IntelServ\Plataforma\Plataforma.Infrastructure.Data\bin\Debug\Plataforma.Infrastructure.Data.dll</CustomAssemblyPath>
    <CustomTypeName>Plataforma.Infrastructure.Data.EntityConfig.EntityContext</CustomTypeName>
    <AppConfigPath>C:\Users\aderb\Desktop\GServicer\IntelServ\Plataforma\Plataforma.Mvc\Web.config</AppConfigPath>
  </Connection>
</Query>

//C#7
void Main()
{
//	Formula: Fn = Fn-1 + Fn-2
//	N	F(n)
//	0	0
//	1	1
//	2	1
//	3	2
//	4	3
//	5	5
//	6	8
//	7	13
//	8	21
//	9	34
//	10	55
	var result = Fibonacci(10);
	result.Dump("Result");
}

public int Fibonacci(int value)
{
    if (value < 0) throw new ArgumentException("Less negativity please!", nameof(value));
    return Fib(value).current;

	//Using tuple C#7
    (int current, int previous) Fib(int i)
    {
        if (i == 0) return (0, 0);
        if (i == 1) return (1, 0);
        if (i == 2) return (1, 1);
		
        var (p, pp) = Fib(i - 1);
		p.Dump("Current");
		p.Dump("Previous");
		
        return (p + pp, p);
    }
}