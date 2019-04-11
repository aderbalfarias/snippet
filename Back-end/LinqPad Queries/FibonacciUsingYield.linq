<Query Kind="Program" />

void Main()
{
	foreach (int x in GenerateFibonacciNumbers(10))
       Console.WriteLine(x);
}

static IEnumerable<int> GenerateFibonacciNumbers(int n)
{
	for (int i = 0, j = 0, k = 1; i < n; i++)
	{	
		//Just to see yield working
		System.Threading.Thread.Sleep(1000);
		
		yield return j;
		
		int temp = j + k;
		j = k;
		k = temp;	
	}
}

// Define other methods and classes here
