<Query Kind="Expression" />

//Strategy: Encapsulates an algorithm inside a class. 
//Define a family of algorithms, encapsulate each one, and make them interchangeable.
//Strategy lets the algorithm vary independently from clients that use it.


/// <summary>
/// MainApp startup class for Structural
/// Strategy Design Pattern.
/// </summary>
class MainApp
{
    /// <summary>
    /// Entry point into console application.
    /// </summary>
    static void Main()
    {
        Context context;

        //Three contexts following different strategies
        context = new Context(new ConcreteStrategyA());
        context.ContextInterface();

        context = new Context(new ConcreteStrategyB());
        context.ContextInterface();

        context = new Context(new ConcreteStrategyC());
        context.ContextInterface();
    }
}

/// <summary>
/// The 'Strategy' abstract class
/// </summary>
abstract class Strategy
{
    public abstract void AlgorithmInterface();
}

/// <summary>
/// A 'ConcreteStrategy' class
/// </summary>
class ConcreteStrategyA : Strategy
{
    public override void AlgorithmInterface()
    {
        Console.WriteLine("Called ConcreteStrategyA.AlgorithmInterface()");
    }
}

/// <summary>
/// A 'ConcreteStrategy' class
/// </summary>
class ConcreteStrategyB : Strategy
{
    public override void AlgorithmInterface()
    {
        Console.WriteLine("Called ConcreteStrategyB.AlgorithmInterface()");
    }
}

/// <summary>
/// A 'ConcreteStrategy' class
/// </summary>
class ConcreteStrategyC : Strategy
{
    public override void AlgorithmInterface()
    {
        Console.WriteLine("Called ConcreteStrategyC.AlgorithmInterface()");
    }
}

/// <summary>
/// The 'Context' class
/// </summary>
class Context
{
    private Strategy _strategy;
    
    public Context(Strategy strategy)
    {
        this._strategy = strategy;
    }

    public void ContextInterface()
    {
        _strategy.AlgorithmInterface();
    }
}