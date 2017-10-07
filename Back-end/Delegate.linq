<Query Kind="Program">
</Query>

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
		
		
		///////////////////////////////Output: 1, 2, 2
		Foo foo = new Foo(Foo1);
	    foo += Foo2;
	    foo();
	    foo -= Foo1;
	    foo();
    }
	
	public delegate void Foo();

   	public static void Foo1()
   	{
      	Console.WriteLine("1");
   	}

   	public static void Foo2()
   	{
      	Console.WriteLine("2");
   	}     
}

//A delegate in .NET is similar to a function pointer in C or C++. Using a delegate allows the programmer to encapsulate 
//a reference to a method inside a delegate object. The delegate object can then be passed to code which can call the 
//referenced method, without having to know at compile time which method will be invoked. In addition, we could use 
//delegate to create custom event within a class.

//C# delegates are similar to pointers to functions, in C or C++. A delegate is a reference type variable that holds the reference to a method. The reference can be changed at runtime.
//Delegates are especially used for implementing events and the call-back methods. All delegates are implicitly derived from the System.Delegate class.