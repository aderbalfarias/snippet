<Query Kind="Program" />

//C# 7
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