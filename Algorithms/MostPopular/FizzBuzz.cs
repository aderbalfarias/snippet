public class FizzBuzz
{
	public static void Main()
	{
		for (int i = 1; i <= 100; i++)
	    {
	        var fizz = i % 3 == 0;
	        var buzz = i % 5 == 0;
	
	        if (fizz && buzz)
	            Console.WriteLine("FizzBuzz");
	        else if (fizz)
	            Console.WriteLine("Fizz");
	        else if (buzz)
	            Console.WriteLine("Buzz");
	        else
	            Console.WriteLine(i);
	    }
	}
}