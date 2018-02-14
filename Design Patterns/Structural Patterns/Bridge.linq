<Query Kind="Program" />

//Bridge: Separates an object’s interface from its implementation. 
//Decouple an abstraction from its implementation so that the two can vary independently.

/// <summary>
/// MainApp startup class for Structural
/// Bridge Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		Abstraction ab = new RefinedAbstraction();
		
		// Set implementation and call
		
		ab.Implementor = new ConcreteImplementorA();
		ab.Operation();
		// Change implemention and call
		ab.Implementor = new ConcreteImplementorB();
		ab.Operation();
	}
}

/// <summary>
/// The 'Abstraction' class
/// </summary>
class Abstraction
{
	protected Implementor implementor;
	
	public Implementor Implementor
	{
		set { implementor = value; }
	}

	public virtual void Operation()
	{
		implementor.Operation();
	}
}

/// <summary>
/// The 'Implementor' abstract class
/// </summary>
abstract class Implementor
{
	public abstract void Operation();
}

/// <summary>
/// The 'RefinedAbstraction' class
/// </summary>
class RefinedAbstraction : Abstraction
{
	public override void Operation()
	{
		implementor.Operation();
	}
}

/// <summary>
/// The 'ConcreteImplementorA' class
/// </summary>
class ConcreteImplementorA : Implementor
{
	public override void Operation()
	{
		Console.WriteLine("ConcreteImplementorA Operation");
	}
}

/// <summary>
/// The 'ConcreteImplementorB' class
/// </summary>
class ConcreteImplementorB : Implementor
{
	public override void Operation()
	{
		Console.WriteLine("ConcreteImplementorB Operation");
	}
}