public class CheckPrime
{
	public static void Main()
	{
	    Console.WriteLine($"Is prime: {IsPrime(6)}");
	    Console.WriteLine($"Is prime: {IsPrime(11)}");
	    Console.WriteLine($"Is prime: {IsPrimeV2(11)}");
	}
	
	public static bool IsPrime(int n) 
	{
		for (int x = 2; x * x <= n; x++)
			if (n % x == 0) 
				return false;
		
		return true; 
	}
	
	public static bool IsPrimeV2(int n) 
	{
		for (int x = 2; x <= Math.Sqrt(n); x++)
			if (n % x == 0)
				return false;
		
		return true;
	} 
}

// Many people get this question wrong. If you're careful about your logic, it's fairly easy.
// The work inside the for loop is constant. Therefore, we just need to know how many iterations the for loop goes through in the worst case.
// The for loop will start when x = 2 and end when x*x = n. Or, in other words, it stops when x = √n (when x equals the square root of n). 