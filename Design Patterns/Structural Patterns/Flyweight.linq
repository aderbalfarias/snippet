<Query Kind="Program" />

//Flyweight: A fine-grained instance used for efficient sharing. 
//Use sharing to support large numbers of fine-grained objects efficiently. 
//A flyweight is a shared object that can be used in multiple contexts simultaneously. 
//The flyweight acts as an independent object in each context — it’s indistinguishable from an instance of the object that’s not shared.

/// <summary>
/// MainApp startup class for Structural 
/// Flyweight Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		//Arbitrary extrinsic state
		int extrinsicstate = 22;
		FlyweightFactory factory = new FlyweightFactory();
		
		//Work with different flyweight instances
		Flyweight fx = factory.GetFlyweight("X");
		fx.Operation(--extrinsicstate);
		
		Flyweight fy = factory.GetFlyweight("Y");
		fy.Operation(--extrinsicstate);
		
		Flyweight fz = factory.GetFlyweight("Z");
		fz.Operation(--extrinsicstate);
		
		UnsharedConcreteFlyweight fu = new UnsharedConcreteFlyweight();
		fu.Operation(--extrinsicstate);
	}
}

/// <summary>
/// The 'FlyweightFactory' class
/// </summary>
class FlyweightFactory
{
	private Hashtable flyweights = new Hashtable();
	
	public FlyweightFactory()
	{
		flyweights.Add("X", new ConcreteFlyweight());
		flyweights.Add("Y", new ConcreteFlyweight());
		flyweights.Add("Z", new ConcreteFlyweight());
	}

	public Flyweight GetFlyweight(string key)
	{
		return ((Flyweight)flyweights[key]);
	}
}

/// <summary>
/// The 'Flyweight' abstract class
/// </summary>
abstract class Flyweight
{
	public abstract void Operation(int extrinsicstate);
}

/// <summary>
/// The 'ConcreteFlyweight' class
/// </summary>
class ConcreteFlyweight : Flyweight
{
	public override void Operation(int extrinsicstate)
	{
		Console.WriteLine($"ConcreteFlyweight: {extrinsicstate}");
	}
}

/// <summary>
/// The 'UnsharedConcreteFlyweight' class
/// </summary>
class UnsharedConcreteFlyweight : Flyweight
{
	public override void Operation(int extrinsicstate)
	{
		Console.WriteLine($"UnsharedConcreteFlyweight: {extrinsicstate}");
	}
}