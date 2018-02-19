<Query Kind="Program" />

//Facade: A single class that represents an entire subsystem. 
//Provide a unified interface to a set of interfaces in a system. 
//Facade defines a higher-level interface that makes the subsystem easier to use.

/// <summary>
/// MainApp startup class for Structural
/// Facade Design Pattern.
/// </summary>
//class MainApp
//{
//	/// <summary>
//	/// Entry point into console application.
//	/// </summary>
//	public static void Main()
//	{
//		Facade facade = new Facade();
//		facade.MethodA();
//		facade.MethodB();
//	}
//}
//
///// <summary>
///// The 'Subsystem ClassA' class
///// </summary>
//class SubSystemOne
//{
//	public void MethodOne()
//	{
//		Console.WriteLine(" SubSystemOne Method");
//	}
//}
//
///// <summary>
///// The 'Subsystem ClassB' class
///// </summary>
//class SubSystemTwo
//{
//	public void MethodTwo()
//	{
//		Console.WriteLine(" SubSystemTwo Method");
//	}
//}
//
///// <summary>
///// The 'Subsystem ClassC' class
///// </summary>
//class SubSystemThree
//{
//	public void MethodThree()
//	{
//		Console.WriteLine(" SubSystemThree Method");
//	}
//}
//
///// <summary>
///// The 'Subsystem ClassD' class
///// </summary>
//class SubSystemFour
//{
//	public void MethodFour()
//	{
//		Console.WriteLine(" SubSystemFour Method");
//	}
//}
//
///// <summary>
///// The 'Facade' class
///// </summary>
//class Facade
//{
//	private SubSystemOne _one;
//	private SubSystemTwo _two;
//	private SubSystemThree _three;
//	private SubSystemFour _four;
//
//	public Facade()
//	{
//		_one = new SubSystemOne();
//		_two = new SubSystemTwo();
//		_three = new SubSystemThree();
//		_four = new SubSystemFour();
//	}
//
//	public void MethodA()
//	{
//		Console.WriteLine("\nMethodA() ---- ");
//		_one.MethodOne();
//		_two.MethodTwo();
//		_four.MethodFour();
//	}
//
//	public void MethodB()
//	{
//		Console.WriteLine("\nMethodB() ---- ");
//		_two.MethodTwo();
//		_three.MethodThree();
//	}
//}

/// <summary>
/// MainApp startup class for Real-World 
/// Facade Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		//Facade
		Mortgage mortgage = new Mortgage();
		
		//Evaluate mortgage eligibility for customer
		Customer customer = new Customer("Ann McKinsey");
		bool eligible = mortgage.IsEligible(customer, 125000);
		
		Console.WriteLine($"\n {customer.Name} has been  {(eligible ? "Approved" : "Rejected")}");
	}
}

/// <summary>
/// The 'Subsystem ClassA' class
/// </summary>
class Bank
{
	public bool HasSufficientSavings(Customer c, int amount)
	{
		Console.WriteLine($"Check bank for {c.Name}");
		return true;
	}
}

/// <summary>
/// The 'Subsystem ClassB' class
/// </summary>
class Credit
{
	public bool HasGoodCredit(Customer c)
	{
		Console.WriteLine($"Check credit for {c.Name}");
		return true;
	}
}

/// <summary>
/// The 'Subsystem ClassC' class
/// </summary>
class Loan
{
	public bool HasNoBadLoans(Customer c)
	{
		Console.WriteLine($"Check loans for {c.Name}");
		return true;
	}
}

/// <summary>
/// Customer class
/// </summary>
class Customer
{
	private string _name;

	//Constructor
	public Customer(string name)
	{
		this._name = name;
	}

	//Gets the name
	public string Name
	{
		get { return _name; }
	}
}

/// <summary>
/// The 'Facade' class
/// </summary>
class Mortgage
{
	private Bank _bank = new Bank();
	private Loan _loan = new Loan();
	private Credit _credit = new Credit();
	
	public bool IsEligible(Customer cust, int amount)
	{
		Console.WriteLine($"{cust.Name} applies for {amount:C} loan\n");
	
		bool eligible = true;
	
		//Check creditworthyness of applicant
		if (!_bank.HasSufficientSavings(cust, amount))
		{
			eligible = false;
		}
		else if (!_loan.HasNoBadLoans(cust))
		{
			eligible = false;
		}
		else if (!_credit.HasGoodCredit(cust))
		{
			eligible = false;
		}
	
		return eligible;
	}
}