<Query Kind="Program" />

//A: A class of which only a single instance can exist. Ensure a class only has one instance, and provide a global point of access to it.

/// <summary>
/// MainApp startup class for Structural
/// Singleton Design Pattern.
/// </summary>
class MainApp
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		//Constructor is protected -- cannot use new
		Singleton s1 = Singleton.Instance();
		Singleton s2 = Singleton.Instance();
		
		//Test for same instance
		if (s1 == s2)
		{
			Console.WriteLine("Objects are the same instance");
		}
	}
}

/// <summary>
/// The 'Singleton' class
/// </summary>
class Singleton
{
	private static Singleton _instance;

	//Constructor is 'protected'
	protected Singleton() { }

	public static Singleton Instance()
	{
		//Uses lazy initialization.
		//Note: this is not thread safe.
		if (_instance == null)
		{
			_instance = new Singleton();
		}
		
		return _instance;
	}
}

///////////////////////////////////////////////////////////////////////////////

//Ex: 2
/// <summary>
/// MainApp startup class for Real-World 
/// Singleton Design Pattern.
/// </summary>
class MainApp2
{
	/// <summary>
	/// Entry point into console application.
	/// </summary>
	static void Main()
	{
		LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
		LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
		LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
		LoadBalancer b4 = LoadBalancer.GetLoadBalancer();
	
		//Same instance?
		if (b1 == b2 && b2 == b3 && b3 == b4)
		{
			Console.WriteLine("Same instance\n");
		}
	
		//Load balance 15 server requests
		LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
		for (int i = 0; i < 15; i++)
		{
			string server = balancer.Server;
			Console.WriteLine($"Dispatch Request to: {server}");
		}
	}
}

/// <summary>
/// The 'Singleton' class
/// </summary>
class LoadBalancer
{
	private static LoadBalancer _instance;
	private List<string> _servers = new List<string>();
	private Random _random = new Random();
	
	//Lock synchronization object
	private static object syncLock = new object();
	
	//Constructor (protected)
	protected LoadBalancer()
	{
		//List of available servers
		_servers.Add("ServerI");
		_servers.Add("ServerII");
		_servers.Add("ServerIII");
		_servers.Add("ServerIV");
		_servers.Add("ServerV");
	}
	
	public static LoadBalancer GetLoadBalancer()
	{
		//Support multithreaded applications through
		//'Double checked locking' pattern which (once
		//the instance exists) avoids locking each
		//time the method is invoked
		if (_instance == null)
		{
			lock (syncLock)
			{
				if (_instance == null)
				{
					_instance = new LoadBalancer();
				}
			}
		}
		
		return _instance;
	}
	
	//Simple, but effective random load balancer
	public string Server
	{ 
		get
		{
			int r = _random.Next(_servers.Count);
			return _servers[r].ToString();
		}
	}
}

