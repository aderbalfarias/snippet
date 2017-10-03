<Query Kind="Program" />

class Test
{
    delegate void TestDelegate(string s);
    static void M(string s)
    {
        Console.WriteLine(s);
    }

    static void Main(string[] args)
    {
        // Original delegate syntax required 
        // initialization with a named method.
        TestDelegate testDelA = new TestDelegate(M);

        // C# 2.0: A delegate can be initialized with
        // inline code, called an "anonymous method." This
        // method takes a string as an input parameter.
        TestDelegate testDelB = delegate(string s) { Console.WriteLine(s); };

        // C# 3.0. A delegate can be initialized with
        // a lambda expression. The lambda also takes a string
        // as an input parameter (x). The type of x is inferred by the compiler.
        TestDelegate testDelC = (x) => { Console.WriteLine(x); };

        // Invoke the delegates.
        testDelA("Original Delegate");
        testDelB("Delegate C# 2");
        testDelC("Delegate C# 3");
		
		//Other options
		Func<int> getRandomNumber = () => new Random().Next(1, 100);
		Console.WriteLine(getRandomNumber()); 
		
		Func<int, int, int> getRandomNumber2 = (a, b) => new Random().Next(a, b);
		Console.WriteLine(getRandomNumber2(10, 30));
		
		//Another option
		Func<int, int, int>  Sum  = (x, y) => x + y;
        int result = Sum(10, 10);
        Console.WriteLine(result);
		
		Func<int, int, string>  Sum1  = (x, y) => $"Sum is {x + y}";
        string result1 = Sum1(10, 10);
        Console.WriteLine(result1);
		
		Func<int , bool> IsPositivo = numero => numero > 0;
		Console.WriteLine(IsPositivo(-40));
		
		
		//Action delegate
		Action<int> printActionDel = i => Console.WriteLine(i);
        printActionDel(10);
		
		//Predicate
		Predicate<string> isUpper = s => s.Equals(s.ToUpper());
		Console.WriteLine(isUpper("hello!"));
		Console.WriteLine(isUpper("HELLO!"));
    }
}