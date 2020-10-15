### C# Versus

#### ```abstract class``` vs ```interface```
An abstract class allows you to create functionalities that subclasses can implement or override and it also can have have constructors. An interface only allows you to define functionalities, not implement it (however from C# 8.0 on you can have default methods and you also can change modifiers). And whereas a class can extend only one abstract class, it can take advantage of multiple interfaces.

#### ```System.String``` vs ```System.StringBuilder```
- ```System.String```: It is immutable, it means when a string object is created you cannot modify and you have always to create a new object string type in memory.<br>  
```
string x = "hi";
x += "how are you?"; // it's a new string instance, we can't change the old one 
```
- ```System.StringBuilder```: It is mutable, means if you create string builder object then you can perform any operation like insert, replace or append without creating new instance for every time. It will update string at one place in memory doesn't create new space in memory.<br>
```
StringBuilder strb = new StringBuilder("hi");
strb.Append("how are you?");
string x = strb.ToString();
```

#### ```String``` vs ```string```
Essentially, there is no difference between string and String in C#.<br>
String is a class in the .NET framework in the System namespace. The fully qualified name is ```System.String```. Whereas, the lower case string is an alias of ```System.String```.

#### ```Action``` vs ```Func``` vs ```Predicate```
- ```Action```: Delegate (pointer) to a method that takes zero, one or more input parameters but doesn't return anything.<br>
- ```Func```: Delegate (pointer) to a method that takes zero, one or more input parameters and returns a value or reference.<br>
- ```Predicate```: A special form of Func and mainly used to validate something and return bool. It is mainly used with collections to whether the item in the collection is valid or not. Basically, its a wrapper of Func like ```Func<T, bool>```.<br>
*When to use that*?<br>
Action is useful if we don’t want to return any result. But if we want to return result, we could use Func. Predicate is mainly to used to validate any condition.

#### Deferred Execution vs Immediate Execution
- Deferred Execution: It simply means that the query is not executed at the time it's specified. Specifically, this is accomplished by assigning the query to a variable. When this is done the query definition is stored in the variable but the query ins't executed until the query variable is interated.
```
var x = from product in context.Products where product.Type == "y" select product;
var x = context.Products.Where(w => w.Type == "y");
foreach(var item x) { Console.WriteLine(item.Name); } // Query executes at x point
```
- Immediate Execution: Query is executed at the point of its declaration. It can be useful if the database is being updated frequently in order to ensure the results where returned at the point the database query is specified. It often uses methods such as ```First(), Avarage(), Sum(), Count(), ToList(), ToArray(), ToDictionary()```.   
```
var x = (from product in context.Products where product.Type == "y" select product).ToList();
var x = context.Products.Where(w => w.Type == "y").ToList();
```

#### Stack vs Heap
In short, in the **Stack** are stored value types (types inherited from ```System.ValueType``` like ```bool, int, long, decimal, float, short```) and in the **Heap** are stored reference types (types inherited from ```System.Object``` such as ```string, object, dynamic```).<br>
Stack is responsible for keeping track what is actually executing and where each executing thread is (each thread has its own stack).<br>
Heap is responsible for keeping track of the data, or more precise objects.

#### ```class``` vs Object
In short, a class is the definition of an object, and an object is an instance of a class.<br>
The class in c# is nothing but a collection of various data members (fields, properties, events, etc.) and member functions. The object in c# is an instance of a class to access the defined properties and methods.

#### Managed vs Unmanaged code
Managed code is the code which is managed by the CLR(Common Language Runtime) in .NET Framework. Whereas the Unmanaged code is the code which is directly executed by the operating system.<br>
- Managed Code: 
    - It is executed by managed runtime environment or managed by the CLR.	
    - It provides security to the application written in .NET Framework.	
    - Memory buffer overflow does not occur.
- Unmanaged Code:
    - It is executed directly by the operating system.
    - It does not provide any security to the application.
    - Memory buffer overflow may occur.
   
#### ```Object.ToString()``` vs ```Convert.ToString()```
```Object.ToString()``` cannot handle ```null``` values which means the *Null reference exception* will be thrown when trying to use ```.ToString()``` on a object which is ```null```, in the other hand ```Convert.ToString()``` can handle ```null``` values it won't generate *Null reference exception*.

#### ```while``` vs ```for```
The difference is that the ```for``` loop is used when you know how many times you need to interate through the code, on the other hand, the ```while``` loop is used when you need to repeat something until a given statement is true.
```
while(condition == true){}
for(initializer; condition; iterator){}
```

#### Boxing vs Unboxing
- Boxing is the process of converting a value type to type object.<br>
- Unboxing is the extraction of the value type from the object.<br>
While the boxing is implicit, unboxing is explicit.<br>
```
int x = 10;
object myObj = x; // Boxing 
int y = (int) myObj; // Unboxing
```

#### Constants vs Readonly Variables 
- Constants can be declared in methods or global context they are declared with ```const``` modifier, it is used for immutable values, they are evaluated at compile time, user-defined types, including classes, structs, and arrays, cannot be ```const```.
- Readonly cannot be delcared in methods, they use ```readonly``` modifier, they are evaluated at runtime, it also can hold reference-type variables, it is mostly used when its actual value is unknown before the runtime and it can only be inilialised at the time of declaration or in a constructor.

### C# Keywords

#### Explain the keyword ```virtual```?
The ```virtual``` keyword is used to modify a method, property, indexer, or event declaration and allow for it to be overridden in a derived class. You cannot use the virtual modifier with the ```static, abstract, private, override``` modifiers.

#### Explain the keyword ```volatile```?
The ```volatile``` keyword indicates that a field might be modified by multiple threads that are executing at the same time. The compiler, the runtime system, and even hardware may rearrange reads and writes to memory locations for performance reasons. Fields that are declared volatile are not subject to these optimizations.

#### Explain the keyword ```using```?
The using keyword has three major uses:<br>
- The using statement defines a scope at the end of which an object will be disposed.
- The using directive creates an alias for a namespace or imports types defined in other namespaces.
- The using static directive imports the members of a single class.

#### Explain the keyword ```unsafe```?
The ```unsafe``` keyword denotes an unsafe context, which is required for any operation involving pointers. You can use the ```unsafe``` modifier in the declaration of a type or a member. The entire textual extent of the type or member is therefore considered an unsafe context

#### Explain the keyword ```sealed```?
When applied to a class, the ```sealed``` modifier prevents other classes from inheriting from it. Like, class **B** inherits from class **A**, but no class can inherit from class **B**
```
class A {}
sealed class B : A {}
```

### C# Object-oriented programming (OOP)

#### What is OOP and how does it relate to the .NET Frameworks?
OOP allows .Net developers to create classes containing methods, properties, fields, events and other logical modules. It also let developers create modular programs with they can assemble as applications and reuse code. OOP have four basic features: encapsulation, abstraction, polimorphism and inheritance.

#### What is Encapsulation?
It is one of the four basic features of OOP and refers to an object's ability to hide data and behavior that are not necessary to its user. Encapsulation helps to keep data from unwanted access through biding code and data in an object which is the basic single self-contained unit of a system. Encapsulation is used to restrict access to the members of a class so as to prevent the user of a given class from manipulating objects in ways that are not intended by the designer, it also has the priciple of hiding the state of an object by using private or protected modifiers. Benefits of it:
- Protection of data from accidental corruption.
- Specification of the accessibility of each of the members of a class to the code outside the class.
- Flexibility and extensibility of the code and reduction in complexity.
- Lower coupling between objects and hence improvement in code maintainability.
- Less likely that other objects can modify the state or behavior of the object in question.
```
public class Bank
{
	private double balance;
	public double Balance
	{
		get
		{
			return balance;
		}
		set
		{
			balance = value;
		}
	}
}
```

#### What is Abstraction?
Abstraction is a principle of OOP and it is used to hide the implementation details and display only essential features of the object. The word abstract means a concept or an idea not associated with any specific instance. We apply the same meaning of abstraction by making classes not associated with any specific instance. Abstraction is needed when we need to only inherit from a certain class, but do not need to instantiate objects of that class. In such a case the base class can be regarded as "Incomplete". Such classes are known as an "Abstract Base Class". 
``` 
public abstract class Animal
{
	private string Name { get; set; }
	private string Specie { get; set; }

	public Animal()
	{
		Console.WriteLine("Do something");
	}

	public Animal(string name, string specie)
	{
		this.Name = name;
		this.Specie = specie;
	}

	public string NameAndSpecie() => $"{Name}, {Specie}"; // concrete method

	public abstract string AnimalType();

	public virtual int AverageLifeInMonths(int years) => years / 12;
}

public class Tiger : Animal
{
	public override string AnimalType() => "Mammals";
}

public class Frog : Animal
{
	public override string AnimalType() => "Amphibians";
}

public class Salmon : Animal
{
	public override string AnimalType() => "Fish";
}
```

#### What is Polymorphism?
Polymorphism means providing an ability to take more than one form, polymorphism provides an ability for the classes to implement different methods that are called through the same name and it also provides an ability to invoke the methods of a derived class through base class reference during runtime based on our requirements.
```
public void AddNumbers(int a, int b)
{
	Console.WriteLine($"a + b = { a + b }");
}

public void AddNumbers(int a, int b, int c)
{
	Console.WriteLine($"a + b + c = { a + b + c }");
}
```

#### What is Inheritance?
Inheritance is one of the core concepts of OOP languages. It is a mechanism where you can to derive a class from another ```class``` for a hierarchy of classes that share a set of attributes and methods. It allows developers to reuse, extend and modify the behavior defined in other classes, **the ```class``` whose members are inherited is called the base class and the ```class``` that inherit those members is called the derived class**. Inheritance allows you to define a base class that provides specific functionality (data and behavior) and to define derived classes that either inherit or override that functionality.<br>
*e.g.:* A base class called ```Vehicle```, and then derived classes called ```Truck, Car, Motprcycle``` all of which inherit the attributes of vehicle.
```
public class A
{
	public string Name;
	public void GetName() => Console.WriteLine("Name: {0}", Name);
}

public class B : A
{
	public string Location;
	public void GetLocation() => Console.WriteLine("Location: {0}", Location);
}

public class C : B
{
	public int Age;
	public void GetAge() => Console.WriteLine("Age: {0}", Age);
}

class Program
{
	static void Main(string[] args)
	{
		C c = new C();
		c.Name = "Test Name";
		c.Location = "Test Location";
		c.Age = 32;

		c.GetName();
		c.GetLocation();
		c.GetAge();
	}
}
```

#### What is Inversion of Control?
Inversion of control is a *Principle in softeware engineering* by which the control of objects or portions of a program is transferred to a container or framework. It is most oftem used in the context of OOP.

### C# Design Patterns

#### Repository
Repositories are classes or components that encapsulate the logic required to access data sources. They centralize common data access functionality, providing better maintainability and decoupling the infrastructure or technology used to access databases from the domain model layer.<br>
The Repository pattern is a well-documented way of working with a data source. In the book Patterns of Enterprise Application Architecture, Martin Fowler describes a repository as follows:<br>
*A repository performs the tasks of an intermediary between the domain model layers and data mapping, acting in a similar way to a set of domain objects in memory. Client objects declaratively build queries and send them to the repositories for answers. Conceptually, a repository encapsulates a set of objects stored in the database and operations that can be performed on them, providing a way that is closer to the persistence layer. Repositories, also, support the purpose of separating, clearly and in one direction, the dependency between the work domain and the data allocation or mapping.*<br>
Basically, a repository allows you to populate data in memory that comes from the database in the form of the domain entities. Once the entities are in memory, they can be changed and then persisted back to the database through transactions.<br>
A Repository Pattern can be implemented in Following ways:<br>
- **One repository per entity (non-generic)**: This type of implementation involves the use of one repository class for each entity. For example, if you have two entities Order and Customer, each entity will have its own repository.
- **Generic repository**: A generic repository is the one that can be used for all the entities, in other words it can be either used for Order or Customer or any other entity.

#### Dependency Injection (DI)
Dependency Injection is a software design pattern that allows us to develop loosely coupled code, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies. DI reduces tight coupling between software components and also enables us to better manage future changes and other complexity in a software. The purpose of DI is to make code maintainable.<br>
Advantages:
- Reduces class coupling.
- Increases code reusability.
- Improves code maintainability.
- Make unit testing possible.

The Dependency Injection pattern involves 3 types of classes:
- Client Class: The client class (dependent class) is a class which depends on the service class
- Service Class: The service class (dependency) is a class that provides service to the client class.
- Injector Class: The injector class injects the service class object into the client class.

Types of Dependency Injection: 
- Constructor Injection: In the constructor injection, the injector supplies the service (dependency) through the client class constructor.
- Property Injection: In the property injection (aka (also known as) the Setter Injection), the injector supplies the dependency through a public property of the client class.
- Method Injection: In this type of injection, the client class implements an interface which declares the method(s) to supply the dependency and the injector uses this interface to supply the dependency to the client class.

Dependency Lifetimes<br>
At registration time, dependencies require a lifetime definition. The service lifetime defines the conditions under which a new service instance will be created. Below are the lifetimes defined by the ASP.Net DI framework. The terminology may be different if you choose to use a different framework.<br>
- **Transient** – Created every time they are requested
- **Scoped** – Created once per scope. Most of the time, scope refers to a web request. But this can also be used for any unit of work, such as the execution of an Azure Function.
- **Singleton** – Created only for the first request. If a particular instance is specified at registration time, this instance will be provided to all consumers of the registration type.

#### [Chain of Responsibility](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Behavioral%20Patterns/ChainOfResponsibility.linq)
Chain of Responsibility is a **Behavioral Pattern** that simplifies object interconnections. Instead of senders and receivers maintaining references to all candidate receivers, each sender keeps a single reference to the head of the chain, and each receiver keeps a single reference to its immediate successor in the chain.<br>
The derived classes know how to satisfy Client requests. If the "current" object is not available or sufficient, then it delegates to the base class, which delegates to the "next" object, and the circle of life continues.

#### [Observer](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Behavioral%20Patterns/Observer.linq)
Observer is a **Behavioral Pattern** in which an object, called the subject, maintains a list of its dependents, called observers, and notifies them automatically of any state changes, usually by calling one of their methods.<br>
Observer pattern is used when there is one-to-many relationship between objects such as if one object is modified, its depenedent objects are to be notified automatically. Observer pattern falls under behavioral pattern category.

#### [Abstract Factory](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Creational%20Patterns/AbstractFactory.linq)
Abstract Factory is a **Creational Pattern** that lets you produce families of related objects without specifying their concrete classes.<br>
Provide a level of indirection that abstracts the creation of families of related or dependent objects without directly specifying their concrete classes. The "factory" object has the responsibility for providing creation services for the entire platform family. Clients never create platform objects directly, they ask the factory to do that for them.

#### [Singleton](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Creational%20Patterns/Singleton.linq)
Singleton is a **Creational Pattern** which makes the class of the single instance object responsible for creation, initialization, access, and enforcement. Declare the instance as a private static data member. Provide a public static member function that encapsulates all initialization code, and provides access to the instance.<br>
The Singleton pattern can be extended to support access to an application-specific number of instances.<br>
The Singleton pattern ensures that a class has only one instance and provides a global point of access to that instance. It is named after the singleton set, which is defined to be a set containing one element. The office of the President of the United States is a Singleton.

#### [Facade](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Structural%20Patterns/Facade.linq)
Facade is a **Structural Pattern** that hides the complexities of the system and provides an interface to the client using which the client can access the system. This pattern involves a single class which provides simplified methods required by client and delegates calls to methods of existing system classes.<br>
The facade pattern is appropriate when you have a complex system that you want to expose to clients in a simplified way, or you want to make an external communication layer over an existing system which is incompatible with the system. Facade deals with interfaces, not implementation. Its purpose is to hide internal complexity behind a single interface that appears simple on the outside.

#### [Adapter](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Structural%20Patterns/Adapter.linq)
Adapter is a **Structural Pattern** which converts the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.<br>
Suppose you have a Bird class with ```fly()```, and ```makeSound()``` methods. And also a ToyDuck class with ```squeak()``` method. Let’s assume that you are short on ToyDuck objects and you would like to use Bird objects in their place. Birds have some similar functionality but implement a different interface, so we can't use them directly. So we will use adapter pattern.

### C# Variety 

#### What is ```delegate```?
A ```delegate``` is a type that represents references to methods with a particular parameter list and return type. When you instantiate a delegate, you can associate its instance with any method with a compatible signature and return type. You can invoke (or call) the method through the delegate instance.<br>
*Delegates are used to pass methods as arguments to other methods*. Event handlers are nothing more than methods that are invoked through delegates. 

#### Does C# support multiple inheritance?
No, however, you can implement multiple interfaces.

#### When ```break``` is used inside two nested ```for``` loops (for inside for), the ```break``` is inside second loop, what does happen? 
It breaks from the inner loop only.

#### Explain what LINQ is?
LINQ in an acronym for Language Integrated Query, it allow data manipulation, regardless of the data source which means that it supports many data providers like .NET Framework collections, SQL Server databases, MySql databases, ADO.NET databases, XML documents, and any collection of objects that support ```IEnumerable``` or generic ```IEnumerable<T>``` interfaces, *In short, LINQ bridges the gap between the world of objects and the world of data*.

#### Explain the difference between Task and Thread in .NET?
- Thread represents an actual OS-level thread, with its own stack and kernel resources. Thread allows the highest degree of control; you can ```Abort()``` or ```Suspend()``` or ```Resume()``` a thread, you can observe its state, and you can set thread-level properties like the stack size, apartment state, or culture. ThreadPool is a wrapper around a pool of threads maintained by the CLR.
- The Task class from the Task Parallel Library offers the best of both worlds. Like the ThreadPool, a task does not create its own OS thread. Instead, tasks are executed by a TaskScheduler; the default scheduler simply runs on the ThreadPool. Unlike the ThreadPool, Task also allows you to find out when it finishes, and (via the generic Task) to return a result.

#### What garbage collection is and how it works. Provide example of how you can enforce it in .NET?
Garbage collection is a low-priority process that serves as an automatic memory manager which manages the allocation and release of memory for the applications. Each time a new object is created, the CLR allocates memory for that object from the managed **heap**. As long as free memory space is available in the managed heap the runtime continuos to allocate space for new objects. However memory is not infinite and when heap memory is full garbage collection comes to free some memory.<br>
When **Garbage Collector** performs a collection it checks for objects in the managed heap that are no longer being used by the applications and performs the necessary operations to relcaim the memory, it will stop all running threads and find the objects in the heap that aren't being accessed by the main program and delete them, then reorganize all the objects left in the heap in order to make space and adjust all pointers to these objects in the heap and the stack.<br>
It can be implemented by using the ```IDisposable interface```.<br>
```System.GC.Collect() // Force garbage collection```

### .NET Core

#### Explain what is a Middlaware?
Middleware is software that's assembled into an app pipeline to handle requests and responses. ASP.NET Core provides a rich set of built-in middleware components, but in some scenarios you might want to write a custom middleware. Middleware should follow the Explicit Dependencies Principle by exposing its dependencies in its constructor. Middleware is constructed once per application lifetime, it is possible to create a middleware pipeline with ```IApplicationBuilder``` inside the method ```public void Configure(IApplicationBuilder app)```. The ASP.NET Core request pipeline consists of a sequence of request delegates, called one after the other.<br>
The incoming requests are passes through this pipeline where all middleware is configured, and middleware can perform some action on the request before passes it to the next middleware. Same as for the responses, they are also passing through the middleware but in reverse order.

#### What are the advantages of ASP.NET Core over ASP.NET?
There are following advantages of ASP.NET Core over ASP.NET:
- It is cross-platform, so it can be run on Windows, Linux, and Mac.
- There is no dependency on framework installation because all the required dependencies are ship with our application.
- ASP.NET Core can handle more request than the ASP.NET.
- Multiple deployment options available withASP.NET Core.

#### What is the startup class in ASP.NET core?
Startup class is the entry point of the ASP.NET Core application. Every .NET Core application must have this class. This class contains the application configuration. It is not necessary that class name must "Startup.cs", it can be anything, we can configure startup class in Program class.
```
public class Program
{
	public static void Main(string[] args)
	{
	    CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
	    Host.CreateDefaultBuilder(args)
	    .ConfigureLogging(logging =>
	    {
			logging.AddConsole();
			logging.AddDebug();
	    })
	    .ConfigureWebHostDefaults(webBuilder =>
	    {
			webBuilder.UseStartup<Startup>();
	    })
	    .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
	}
}

public class Startup
{
	private const string DemoConnection = "DemoConnection";
	private const string corsSettings = "CorsOrigin";
	private string[] Schemes = { JwtBearerDefaults.AuthenticationScheme, "ADFS" };

	public Startup(IConfiguration configuration, IWebHostEnvironment environment)
	{
		Configuration = configuration;
		Environment = environment;
	}

	public IConfiguration Configuration { get; }
	public IWebHostEnvironment Environment { get; }

	// This method gets called by the runtime. 
	// Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		var corsOrigin = Configuration.GetSection(corsSettings).Get<string[]>();

		services.AddCors(options =>
		{
			options.AddDefaultPolicy(builder =>
			{
				builder.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
			});
		});

		// Dependency Injection
		services.Services();
		services.Repositories();
		services.Databases(Configuration.GetConnectionString(DemoConnection));
		services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

		var authenticationOption = Configuration
			.GetSection(nameof(ApplicationOptions.Authentication))
			.Get<AuthenticationOptions>();

		services.AddSingleton(authenticationOption);

		services.AddHealthChecks()
			.AddSqlServer(Configuration.GetConnectionString(DemoConnection));

		services.AddControllers();
		services.AddApiVersioning();
		services.AddSwagger(authenticationOption);

		var oidc = Configuration
			.GetSection(nameof(ApplicationOptions.OidcAuthorizationServer))
			.Get<OidcAuthorizationServerOptions>();

		services.AddSingleton(oidc);
		services.AddAuthorization(options =>
		{
			options.DefaultPolicy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.AddAuthenticationSchemes(Schemes)
				.Build();
		});

		services.AddBearers(Environment, oidc, authenticationOption, Schemes);
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
	{
	    app.UseAuthentication();

	    logger.LogInformation($"In {env.EnvironmentName} environment");

	    app.UseSwagger();
	    app.UseSwaggerUI(c =>
	    {
			c.SwaggerEndpoint("../swagger/v1/swagger.json", "Core App v1");
			c.RoutePrefix = string.Empty;
	    });

	    if (env.IsDevelopment())
	    {
			app.UseDeveloperExceptionPage();
			app.UseMiddleware<ExceptionMiddleware>();
	    }
	    else
	    {
			app.UseMiddleware<ExceptionMiddleware>();
			app.UseExceptionHandler("/Error");
			app.UseStatusCodePagesWithReExecute("/Error/{0}");
			app.UseHsts();

			app.UseHttpsRedirection();
	    }

	    app.UseRouting();
	    app.UseAuthorization();
	    app.UseStaticFiles();
	    app.UseCors();
	    app.UseHealthChecks();

	    app.UseEndpoints(endpoints =>
	    {
			endpoints.MapControllers();
	    });
	}
}
```

#### What is the use of ```ConfigureServices(IServiceCollection services)``` method of startup class?
This is an optional method of startup class. It can be used to configure the services that are used by the application. This method calls first when the application is requested for the first time. Using this method, we can do things like adding services to the DI container, so services are available as a dependency in controller constructor.

#### What is the use of the ```Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)``` method of startup class?
It defines how the application will respond to each HTTP request. We can configure the request pipeline by configuring the middleware. It accepts IApplicationBuilder as a parameter and also it has two optional parameters: IWebHostEnvironment and ILogger. Using this method, we can configure built-in middleware such as routing, authentication, session, etc. as well as third-party or custom middlewares.

#### What is the difference between ```IApplicationBuilder.Use()``` and ```IApplicationBuilder.Run()```?
We can use both the methods in Configure methods of startup class. Both are used to add middleware delegate to the application request pipeline. 
- ```IApplicationBuilder.Use()``` may call the next middleware in the pipeline 
- ```IApplicationBuilder.Run()``` method never calls the subsequent or next middleware. After ```IApplicationBuilder.Run``` method, system stop adding middleware in request. pipeline.

#### What is routing in ASP.NET Core?
Routing is functionality that map incoming request to the route handler. The route can have values (extract them from URL) that used to process the request. The Routing uses routes for map incoming request with route handler and Generate URL that used in response.

### SOLID

#### S - Single responsibility Principle
The Single Responsibility Principle states that every module or class should have responsibility for a single part of the functionality provided by the software.

#### O - Open-Closed Principle
The open/closed principle states that software entities (classes, modules, functions) should be open for extensions, but closed for modification.

#### L - Liskov Substitution Principle
The Liskov substitution principle states that if S is a subtype of T, then objects of type T may be replaced (or substituted) with objects of type S.<br>
We can formulate this mathematically as:
- Let ϕ(x) be a property provable about objects x of type T.
- Then ϕ(y) should be valid for objects y of type S, where S is a subtype of T.
More generally, it states that objects in a program should be replaceable with instances of their subtypes without altering the correctness of that program.

#### I - Interface Segregation Principle
The interface segregation principle states not to force a client to depend on methods it does not use. Do not add additional functionality to an existing interface by adding new methods. Instead, create a new interface and let your class implement multiple interfaces if needed.

#### D - Dependency Inversion Principle (Dependency injection)
The dependency inversion principle is a way to decouple software modules. This principle states that:
- High-level modules should not depend on low-level modules. Both should depend on abstractions.
- Abstractions should not depend on details. Details should depend on abstractions.
To comply with this principle, we need to use a design pattern known as a dependency inversion pattern, most often solved by using dependency injection.
 
### Sql Server

### Architecture Basic

### [Architectural Principles](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)

### Front-end Javascript

### Front-end Angular

#### What is a Directive?
At the core, a directive is a function that executes whenever the Angular compiler finds it in the DOM. Angular directives are used to extend the power of the HTML by giving it new syntax. Each directive has a name, either one from the Angular predefined like ```ng-repeat```, or a custom one which you can name as you prefer. There are 3 types of directives:
- **Component Directives** These form the main class having details of how the component should be processed, instantiated and used at runtime.
- **Structural Directives** basically deals with manipulating the DOM elements. Structural directives have a * sign before the directive. For example, *ngIf and *ngFor.
- **Attribute Directives** they deal with changing the look and behavior of the dom element. You can create your own directives.

#### What are Lifecycle hooks in Angular? Explain some of them
Angular components enter its lifecycle from the time it is created to the time it is destroyed. Angular hooks provide ways to tap into these phases and trigger changes at specific phases in a lifecycle.
- ```ngOnChanges()```: This method is called whenever one or more input properties of the component changes. The hook receives a SimpleChanges object containing the previous and current values of the property.
- ```ngOnInit()```: This hook gets called once, after the ```ngOnChanges``` hook. It initializes the component and sets the input properties of the component.
- ```ngDoCheck()```: It gets called after ```ngOnChanges``` and ```ngOnInit``` and is used to detect and act on changes that cannot be detected by Angular. We can implement our change detection algorithm in this hook. 
- ```ngAfterContentInit()```: It gets called after the first ```ngDoCheck``` hook. This hook responds after the content gets projected inside the component.
- ```ngAfterContentChecked()```: It gets called after ngAfterContentInit and every subsequent ```ngDoCheck```. It responds after the projected content is checked.
- ```ngAfterViewInit()```: It responds after a component's view, or a child component's view is initialized.
- ```ngAfterViewChecked()```: It gets called after ```ngAfterViewInit```, and it responds after the component's view, or the child component's view is checked.
- ```ngOnDestroy()```: It gets called just before Angular destroys the component. This hook can be used to clean up the code and detach event handlers.

#### Explain Dependency Injection in Angular?
Dependency injection is an application design pattern that is implemented by Angular and forms the core concepts of Angular. <br>
Dependencies in Angular are services which have a functionality. Various components and directives in an application can need these functionalities of the service. Angular provides a smooth mechanism by which these dependencies are injected into components and directives.

#### Describe the MVVM architecture?
MVVM architecture removes tight coupling between each component. The MVVM architecture comprises of three parts:
- **Model**: It represents the data and the business logic of an application, or we may say it contains the structure of an entity. It consists of the business logic - local and remote data source, model classes, repository.
- **View**: View is a visual layer of the application, and so consists of the UI Code(in Angular- HTML template of a component.). It sends the user action to the ViewModel but does not get the response back directly. It has to subscribe to the observables which ViewModel exposes to it to get the response. 
- **ViewModel**: It is an abstract layer of the application and acts as a bridge between the View and Model(business logic). It does not have any clue which View has to use it as it does not have a direct reference to the View. View and ViewModel are connected with data-binding so, any change in the View the ViewModel takes note and changes the data inside the Model. It interacts with the Model and exposes the observable that can be observed by the View.

#### How to navigating between different routes in an Angular app?
The following snippet demonstrate how to do that:
```
import from "@angular/router";
@Component({
	selector: 'app-header',
	template: `
		<nav class="navbar navbar-light bg-faded">
			<a class="navbar-brand" (click)="goHome()">Search App</a> 
			<ul class="nav navbar-nav">
				<li class="nav-item">
					<a class="nav-link" (click)="goHome()">Home</a> 
				</li>
				<li class="nav-item">
					<a class="nav-link" (click)="goSearch()">Search</a> 
				</li>
			</ul>
		</nav>
	`
})
class HeaderComponent {
	constructor(private router: Router) {} 
	goHome() {
		this.router.navigate(['']); 
	}
	goSearch() {
		this.router.navigate(['search']); 
	}
}
```

#### What is the AOT (Ahead-Of-Time) Compilation? What are its advantages?
An angular application consists of components and templates which a browser cannot understand. Therefore, every Angular application needs to be compiled before running inside the browser. The Angular compiler takes in the JS code, compiles it, and then produces some JS code. It is known as AOT compilation and happens only once per occasion per user.<br> 
There are two kinds of compilation that Angular provides:
- **JIT(Just-in-Time) compilation**: the application compiles inside the browser during runtime
- **AOT(Ahead-of-Time) compilation**: the application compiles during the build time.
Advantages of AOT compilation:
- **Fast Rendering**: The browser loads the executable code and renders it immediately as the application is compiled before running inside the browser. 
- **Fewer Ajax Requests**: The compiler sends the external HTML and CSS files along with the application, eliminating AJAX requests for those source files. 
- **Minimizing Errors**: Easy to detect and handle errors during the building phase. 
- **Better Security**: Before an application runs inside the browser, the AOT compiler adds HTML and templates into the JS files, so there are no extra HTML files to be read, thus providing better security for the application.

#### Could you explain services in Angular?
Singleton objects in Angular that get instantiated only once during the lifetime of an application are called services. An Angular service contains methods that maintain the data throughout the life of an application.<br> 
The primary intent of an Angular service is to organize as well as share business logic, models, or data and functions with various components of an Angular application.<br> 
The functions offered by an Angular service can be invoked from any Angular component, such as a controller or directive.

#### What is string interpolation in Angular?
Also referred to as moustache syntax, string interpolation in Angular refers to a special type of syntax that makes use of template expressions in order to display the component data. These template expressions are enclosed within double curly braces i.e. ```{{ }}```.

#### Can you explain the concept of scope hierarchy in Angular?
Angular organizes the ```$scope``` objects into a hierarchy that is typically used by views. This is known as the scope hierarchy in Angular. It has a root scope that can further contain one or several scopes called child scopes.<br>
In a scope hierarchy, each view has its own $scope. Hence, the variables set by a view's view controller will remain hidden to other view controllers. Following is a typical representation of a Scope Hierarchy:
- Root $scope
	- $scope for Controller 1
	- $scope for Controller 2 
	- $scope for Controller n
	
#### How to generate a class in Angular using CLI?
```ng generate class MyClassName [options]```

#### How do Observables differ from Promises?
As soon as a promise is made, the execution takes place. However, this is not the case with observables because they are lazy. This means that nothing happens until a subscription is made. While promises handle a single event, observable is a stream that allows passing of more than one event. A callback is made for each event in an observable.

#### Could you explain the concept of templates in Angular?
Written with HTML, templates in Angular contains Angular-specific attributes and elements. Combined with information coming from the controller and model, templates are then further rendered to cater the user with the dynamic view.

#### Explain the difference between an Annotation and a Decorator in Angular?
- **Annotations** are used for creating an annotation array. They are only metadata set of the class using the Reflect Metadata library.
- **Decorators** are design patterns used for separating decoration or modification of some class without changing the original source code.

#### What are the building blocks of Angular?
There are essentially 9 building blocks of an Angular application. These are:
- **Components**: A component controls one or more views. Each view is some specific section of the screen. Every Angular application has at least one component, known as the root component. It is bootstrapped inside the main module, known as the root module. A component contains application logic defined inside a class. This class is responsible for interacting with the view via an API of properties and methods.
- **Data Binding**: The mechanism by which parts of a template coordinates with parts of a component is known as data binding. In order to let Angular know how to connect both sides (template and its component), the binding markup is added to the template HTML.
- **Dependency Injection (DI)**: Angular makes use of DI to provide required dependencies to new components. Typically, dependencies required by a component are services. A component's constructor parameters tell Angular about the services that a component requires. So, a dependency injection offers a way to supply fully-formed dependencies required by a new instance of a class.
- **Directives**: The templates used by Angular are dynamic in nature. Directives are responsible for instructing Angular about how to transform the DOM when rendering a template. Actually, components are directives with a template. Other types of directives are attribute and structural directives.
- **Metadata**: In order to let Angular know how to process a class, metadata is attached to the class. For doing so decorators are used.
- **Modules**: Also known as ```NgModules```, a module is an organized block of code with a specific set of capabilities. It has a specific application domain or a workflow. Like components, any Angular application has at least one module. This is known as the root module. Typically, an Angular application has several modules.
- **Routing**: An Angular router is responsible for interpreting a browser URL as an instruction to navigate to a client-generated view. The router is bound to links on a page to tell Angular to navigate the application view when a user clicks on it.
- **Services**: A very broad category, a service can be anything ranging from a value and function to a feature that is required by an Angular app. Technically, a service is a class with a well-defined purpose.
- **Template**: Each component's view is associated with its companion template. A template in Angular is a form of HTML tags that lets Angular know that how it is meant to render the component.
