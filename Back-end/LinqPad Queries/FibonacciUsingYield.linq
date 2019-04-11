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

// Yield break statement
public IEnumerable<T> GetData<T>(IEnumerable<T> items)
{
    if (null == items)
		yield break;
	foreach (T item in items)
		yield return item;
}

// Yield accessor
public static IEnumerable<int> EvenNumbers
{
	get
	{
		for (int i = 1; i <= 10; i++)
		{
			if((i % 2) ==0)
			yield return i;
		}
	}
}

// Note:
// 	You cannot have the yield return statement in a try-catch block though you can have it inside a try-finally block
// 	You cannot have the yield break statement inside a finally block
// 	The return type of the method where yield has been used, should be IEnumerable, IEnumerable<T>, IEnumerator, or IEnumerator<T>
// 	You cannot have a ref or out parameter in your method in which yield has been used
// 	You cannot use the "yield return" or the "yield break" statements inside anonymous methods
// 	You cannot use the "yield return" or the "yield break" statements inside "unsafe" methods, i.e., 
// 		methods that are marked with the "unsafe" keyword to denote an unsafe context