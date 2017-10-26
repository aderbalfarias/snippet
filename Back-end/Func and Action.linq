<Query Kind="Program" />

class Program
{
    static void Main(string[] args)
    {
        Action<int> myAction = new Action<int>(DoSomething);
        myAction(123);           // Prints out "123"
		
        Func<int, double> myFunc = new Func<int, double>(CalculateSomething);
        Console.WriteLine(myFunc(5));   // Prints out "2.5"
    }

    static void DoSomething(int i)
    {
        Console.WriteLine(i);
    }

    static double CalculateSomething(int i)
    {
        return (double)i/2;
    }
}

//Action is a delegate (pointer) to a method, that takes zero, one or more input parameters, but does not return anything
//
//Func is a delegate (pointer) to a method, that takes zero, one or more input parameters, and returns a value (or reference)
//
//Predicate is a special kind of Func often used for comparisons
//Predicate is a delegate that takes generic parameters and returns bool
