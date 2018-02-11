<Query Kind="Program" />

//A: Creates an instance of several derived classes. Define an interface for creating an object, but let subclasses decide which class to instantiate. Factory Method lets a class defer instantiation to subclasses.

/// <summary>
/// MainApp startup class for Structural 
/// Factory Method Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		//An array of creators
		Creator[] creators = new Creator[2];
		creators[0] = new ConcreteCreatorA();
		creators[1] = new ConcreteCreatorB();
		
		//Iterate over creators and create products
		foreach (Creator creator in creators)
		{
			Product product = creator.FactoryMethod();
			Console.WriteLine($"Created {product.GetType().Name}");
		}
	}
}

/// <summary>
/// The 'Product' abstract class
/// </summary>
abstract class Product { }

/// <summary>
/// A 'ConcreteProduct' class
/// </summary>
class ConcreteProductA : Product { }

/// <summary>
/// A 'ConcreteProduct' class
/// </summary>
class ConcreteProductB : Product { }

/// <summary>
/// The 'Creator' abstract class
/// </summary>
abstract class Creator
{
	public abstract Product FactoryMethod();
}

/// <summary>
/// A 'ConcreteCreator' class
/// </summary>
class ConcreteCreatorA : Creator
{
	public override Product FactoryMethod()
	{
		return new ConcreteProductA();
	}
}

/// <summary>
/// A 'ConcreteCreator' class
/// </summary>
class ConcreteCreatorB : Creator
{
	public override Product FactoryMethod()
	{
		return new ConcreteProductB();
	}
}