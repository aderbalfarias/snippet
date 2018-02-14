<Query Kind="Program" />

//Abstract Factory: Creates an instance of several families of classes. 
//Provide an interface for creating families of related or dependent objects without specifying their concrete classes.

//The classes and objects participating in this pattern are:

//AbstractFactory(ContinentFactory)
//declares an interface for operations that create abstract products

//ConcreteFactory(AfricaFactory, AmericaFactory)
//implements the operations to create concrete product objects

//AbstractProduct(Herbivore, Carnivore)
//declares an interface for a type of product object

//Product(Wildebeest, Lion, Bison, Wolf)
//defines a product object to be created by the corresponding concrete factory
//implements the AbstractProduct interface

//Client(AnimalWorld)
//uses interfaces declared by AbstractFactory and AbstractProduct classes

/// <summary>
/// MainApp startup class for Real-World
/// Abstract Factory Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	public static void Main() 
	{
	    //Create and run the African animal world
	    ContinentFactory africa = new AfricaFactory();
	    AnimalWorld world = new AnimalWorld(africa);
	    world.RunFoodChain();
	
	    //Create and run the American animal world	
	    ContinentFactory america = new AmericaFactory();
	    world = new AnimalWorld(america);
	    world.RunFoodChain();
	}
}

/// <summary>
/// The 'ContinentFactory' abstract class
/// </summary>
abstract class ContinentFactory 
{
	public abstract Herbivore CreateHerbivore();
	public abstract Carnivore CreateCarnivore();
}

/// <summary>
/// The 'AfricaFactory' class
/// </summary>
class AfricaFactory : ContinentFactory 
{
	public override Herbivore CreateHerbivore() 
	{
		return new Wildebeest();
	}
	
	public override Carnivore CreateCarnivore() 
	{
    	return new Lion();
	}
}

/// <summary>
/// The 'AmericaFactory' class
/// </summary>
class AmericaFactory : ContinentFactory
{
	public override Herbivore CreateHerbivore() 
	{
		return new Bison();
	}
	
	public override Carnivore CreateCarnivore() 
	{
		return new Wolf();
	}
}

/// <summary>
/// The 'Herbivore' abstract class
/// </summary>
abstract class Herbivore { }

/// <summary>
/// The 'Carnivore' abstract class
/// </summary>
abstract class Carnivore
{
	public abstract void Eat(Herbivore h);
}

class Wildebeest : Herbivore { }

class Lion : Carnivore
{
	public override void Eat(Herbivore h) 
	{
		//Eat Wildebeest	
		Console.WriteLine($"{this.GetType().Name} eats {h.GetType().Name}");
	}
}

class Bison : Herbivore { }

class Wolf : Carnivore
{
	public override void Eat(Herbivore h) 
	{
		//Eat Bison
		Console.WriteLine($"{this.GetType().Name} eats {h.GetType().Name}");
	}
}

class AnimalWorld
{
	private Herbivore _herbivore;
	private Carnivore _carnivore;
	
	//Constructor
	public AnimalWorld(ContinentFactory factory) 
	{
		_carnivore = factory.CreateCarnivore();
		_herbivore = factory.CreateHerbivore();
	}
	
	public void RunFoodChain() 
	{
		_carnivore.Eat(_herbivore);
	}
}