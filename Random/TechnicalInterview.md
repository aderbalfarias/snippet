### .NET Core and .NET

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

#### What are the available HTTP Methods? 
- **GET**, Retrieve data.
- **HEAD**, Like get without response.
- **POST**, Submit entity.
- **PUT**, Changes which requires payload. 
- **DELETE**, Delete data.
- **TRACE**, Performs a message loop-back test along the path to the target resource.
- **OPTIONS**, Used to describe the communication options for the target resource.
- **CONNECT**, Estabilishes tunnels. Proxy server to tunnel the TCP connection to the desired destination.
- **PATCH**, Making partial changes to an existing resource.

<hr>

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

#### ```public``` vs ```static``` vs ```void``` 
- ```public``` declared variables or methods are accessible anywhere in the application. 
- ```static``` declared variables or methods are globally accessible without creating an instance of the class. Static member are by default not globally accessible it depends upon the type of access modified used. The compiler stores the address of the method as the entry point and uses this information to begin execution before any objects are created. 
- ```void``` is a type modifier that states that the method or variable does not return any value.

#### ```ref``` vs ```out``` parameters
An argument passed as ```ref``` must be initialized before passing to the method whereas ```out``` parameter needs not to be initialized before passing to a method.

<hr>

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

<hr>

### C# General 

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

#### How to pass a method as parameter in C#?
Using Action if it doesn't return anything or Func if it does return value or reference
```
var result = await Retry(async () => await MyMethodAsync(), TimeSpan.FromSeconds(10), 5);

private async Task<T> Retry<T>(Func<Task<T>> action, TimeSpan retryInterval, int maxAttemptCount = 3)
{
    var exceptions = new List<Exception>();

    for (int attempted = 0; attempted < maxAttemptCount; attempted++)
    {
        try
        {
            if (attempted > 0)
                await Task.Delay(retryInterval);

            return await action();
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }
    }

    throw new AggregateException(exceptions);
}

```

#### Can we use ```this``` command within a static method?
We can't use ```this``` in a static method because we can only use static variables/methods in a static method.

<hr>

### Object-oriented programming (OOP)

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
        Console.WriteLine ("Do something");
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

<hr>

### Design Patterns

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

<hr>

### SOLID

#### S - Single responsibility Principle
**Definition**: The Single Responsibility Principle states that every module or class should have responsibility for a single part of the functionality provided by the software.<br>
**Example**: If we have two reasons to change a class, we have to split the functionality into two classes. Each class will handle only one responsibility lets say you have ```class Client``` which has register client and sends email functionalities, that is wrong, they should be splitted in two different classes (```class Client``` and ```class Email```) each one responsible for its own functionality.<br>
**Why**: If we put more than one functionality in one class then it introduces coupling between two functionalities. So, if we change one functionality there is a chance we broke coupled functionality, which requires another round of testing to avoid any bug in the production environment. It reduces bug fixes and testing time once an application goes into the maintenance phase.<br>
**Benefits**:
- Reduction in complexity of a code. A code is based on its functionality. A method holds logic for a single functionality or task. So, it reduces the code complexity.
- Increased readability, extensibility, and maintenance. Each method has a single functionality so it is easy to read and maintain. 
- Reusability and reduced error. As code separates based functionality so if the same functionality uses somewhere else in an application then don't write it again.
- Better testability. In the maintenance, when a functionality changes then we don't need to test the entire model.
- Reduced coupling. It reduced the dependency code. A method's code doesn't depend on other methods.

#### O - Open-Closed Principle
**Definition**: The open/closed principle states that software entities (classes, modules, functions) should be open for extensions, but closed for modification.<br>
**Example**: An implementation (of a class or function), once its logic and/or functionality is created, should be closed for further modification, you may use code refactoring in order to resolve errors of implementation, in the other hand the implementation (of a class or function) is open for extension of its logic and/or functionality.
- Wrong Implementation for Mortgage class:
```
public class Mortgage
{
    public decimal CalculateInterest(MortgageType mortgageType)
    {
        decimal interest = 0;

        if (mortgageType == "FixedRate")
            interest = Loan * 0.4;
        else if (mortgageType == "VariableRate")
            interest = Loan * Variant;

        return interest;
    }
}
```
The implementation above is not following the Open Closed principle because if in the future a new Mortgage type is introduced then there will be a requirement to modify this method in order to add a new condition for calculating the interest for the new mortgage type. That means the method is always open for modification.
- Correct Implementation for Mortgage class following Open Closed principle:
```
interface IMortgage
{
    decimal CalculateInterest();
}

public class FixedRateMortgage : IMortgage
{
    public decimal CalculateInterest() => Loan * 0.5;
}

public class VariableRateMortgage : IMortgage
{
    public decimal CalculateInterest() => Loan * Variant;
}
```
So if there is a new mortgage type added there is no need to modify the logic of exiting classes, just extend the functionality by inheriting an interface (Mortgage interface in this case and create a new class which will have the implementation for ```CalculateInterest()``` method. I am using an interface but it could be an abstract class as well.<br>
**Why**: When a single change to a program results in a cascade of changes to dependent modules, that program exhibits the undesirable attributes that we have come to associate with 'bad' design. The program becomes fragile, rigid, unpredictable, and unreusable. A function that is doing too many things outside the realm of its responsibilities creates unnecessary entanglements that makes it harder to read and debug.<br>
**Benefits**:
- Extensibility. Where the design modules never change. When requirements change, you extend the behavior of such modules by adding new code, not by changing old code that already works in order to prevent cascade changes to dependent modules.
- Maintainability. The main benefit of this approach is that an interface introduces an additional level of abstraction which enables loose coupling. The implementations of an interface are independent of each other and don't need to share any code.
- It makes the code readable, testable, changeable, and reusable.

#### L - Liskov Substitution Principle
**Definition**: The Liskov substitution principle states that if S is a subtype of T, then objects of type T may be replaced (or substituted) with objects of type S.<br>
We can formulate this mathematically as:
- Let ϕ(x) be a property provable about objects x of type T.
- Then ϕ(y) should be valid for objects y of type S, where S is a subtype of T.
More generally, it states that objects in a program should be replaceable with instances of their subtypes without altering the correctness of that program.<br>
**Example**: 
- Violation of Liskov Substitution Principle
```
public interface IBankAccount
{
    bool Withdrawal(decimal amount);
}

public class SalaryAccount : IBankAccount
{
    public bool Withdrawal(decimal amount) => amount >= 0 && amount <= 10000 ? true : false;
}

public class RegularAccount : IBankAccount
{
    public bool Withdrawal(decimal amount) => amount >= 0 ? true : false;
}

public class FixDepositAccount : IBankAccount
{
    public bool Withdrawal(decimal amount) => throw new Excpetion("Function not supported by this account type"); 
}
```
In the preceding code the ```IBankAccount``` interface is implemented by a different kind of account types of the bank, like Regular, Salary and FixDeposit savings account, however, the ```FixDeposit``` doesn't provide a withdrawal facility whereas another bank account might provide a withdrawal facility. If you try to call it an excepition will be displayed:
```
public static class AccountManager
{
    public static bool WithdrawalFromAccount(IBankAccount bankAccount)
    {
        return bankAccount.Withdrawal(10);
    }
}

public class Program
{
    public static void Main()
    {
        // Calling AccountManager
        AccountManager.WithdrawalFromAccount(new RegularAccount()); // ok  
        AccountManager.WithdrawalFromAccount(new SalaryAccount());  // ok  
        AccountManager.WithdrawalFromAccount(new FixDepositAccount()); // throws exception at runtime as withdrawal is not supported  
    }
}
```
- Implementing Liskov Substitution Principle
```
public interface IBankAccount
{
}

public class AccountWithWithdrawal : IBankAccount
{
    public virtual bool Withdrawal(decimal amount) => false;
}

public class AccountWithoutWithdrawal : IBankAccount
{
}

public class SalaryAccount : AccountWithWithdrawal
{
    public override bool Withdrawal(decimal amount) => amount >= 0 && amount <= 10000 ? true : false;
}

public class RegularAccount : AccountWithWithdrawal
{
    public override bool Withdrawal(decimal amount) => amount >= 0 ? true : false;
}

public class FixDepositAccount : AccountWithoutWithdrawal
{
}

public class AccountManager
{
    public static bool WithdrawalFromAccount(AccountWithWithdrawal bankAccount)
    {
        return bankAccount.Withdrawal(10);
    }
}

public class Program
{
    public static void Main()
    {
        // Calling AccountManager
        AccountManager.WithdrawalFromAccount(new RegularAccount()); // ok, Output: true  
        AccountManager.WithdrawalFromAccount(new SalaryAccount());  // ok, Output: true  
        //AccountManager.WithdrawalFromAccount(new FixDepositAccount()); // compiler gives error, it should be removed
    }
}
```
It's not always true that one must make changes in the inheritance tree but making changes at the class and method level also resolves problems, but the solution above was done by changing the inheritance tree.<br>
**Why**: Places in implementation (class/function) that use a base class (in other words consume a service of a base class), must work correctly when the base class object is replaced by a child class (derived class) object.<br>
**Benefits**:
- Compatibility. It enables the binary compatibility between multiple releases and patches. In other words, It keeps the client code away from being impacted.
- Type Safety. It's the easiest approach to handle type safety with inheritance, as types are not allowed to vary when inheriting.
- Maintainability. Code that adheres to Liskov substitution is loosely dependent on each other, makes the right abstraction, and encourages code reusability.

#### I - Interface Segregation Principle
**Definition**: The interface segregation principle states not to force a client to depend on methods it does not use. Do not add additional functionality to an existing interface by adding new methods. Instead, create a new interface and let your class implement multiple interfaces if needed.<br>
**Example**: If there is big fat interface then break it into a set of small interfaces with the related method(s) in it. It's similar to normalizing our database like normalizing database from 1NF to 3NF where a big table is broken into tables with related columns.
- Violation of Interface Segregation
```
public interface IMachine 
{
    void Print();
    void Fax();
    void Scan();
}

public class MultiFunctionPrinter : IMachine // It is ok
{  
    public void Print() => Console.WriteLine("Print Document");
    public void Fax() => Console.WriteLine("Send Document");
    public void Scan() => Console.WriteLine("Scan Document");
}

public class Printer : IMachine // It is not ok
{  
    public void Print() => Console.WriteLine("Print Document");
    public void Fax() => throw new NotImplementedException();
    public void Scan() => throw new NotImplementedException();
}

public class Scanner : IMachine // It is not ok
{  
    public void Print() => throw new NotImplementedException();
    public void Fax() => throw new NotImplementedException();
    public void Scan() => Console.WriteLine("Scan Document");
}
```
- Implementing Interface Segregation
```
public interface IPrinter 
{
    void Print();
}

public interface IFaxer
{
    void Fax();
}

public interface IScanner
{
    void Scan();
}

public class MultiFunctionPrinter : IPrinter, IFaxer, IScanner // It is ok
{  
    public void Print() => Console.WriteLine("Print Document");
    public void Fax() => Console.WriteLine("Send Document");
    public void Scan() => Console.WriteLine("Scan Document");
}

public class Printer : IPrinter // It is ok
{  
    public void Print() => Console.WriteLine("Print Document");
}

public class Scanner : IScanner // It is ok
{  
    public void Scan() => Console.WriteLine("Scan Document");
}
```
**Why**: Do not design a big fat interface that forces the client to implement a method that is not required by it, instead design a small interface. So by doing this class only implement the required set of interface(s).<br>
**Benefits**:
- Faster Compilation. If you have violated interface segregation i.e. stuffed methods together in the interface, and when method signature changes, you need to recompile all the derived classes.
- Reusability. "Fat interfaces" (interfaces with additional useless methods) lead to inadvertent coupling between classes. Thus, an experienced dev knows coupling is the bane of reusability.
- Maintainability. By avoiding unneeded dependencies, the system becomes easier to understand, lighter to test, quicker to change. Similarly, to the reader of your code, it would be harder to get an idea of what your class does from the class declaration line. So, if dev sees only the one god-interface that may have inherited other interfaces it will likely not be obvious. Compare ```MyMachine : IMachine``` to ```MyMachine : IPrinter, IScanner, IFaxer```. The latter tells you a lot, the first makes you guess at best.

#### D - Dependency Inversion Principle (Dependency injection)
**Definition**: The dependency inversion principle is a way to decouple software modules. This principle states that:
- High-level modules should not depend on low-level modules. Both should depend on abstractions.
- Abstractions should not depend on details. Details should depend on abstractions.
To comply with this principle, we need to use a design pattern known as a dependency inversion pattern, most often solved by using dependency injection.<br>
**Example**:<br>
- Not following the Dependency Inversion
```
public class SalaryCalculator
{
    public float CalculateSalary(int hoursWorked, float hourlyRate) => hoursWorked * hourlyRate;
}

public class EmployeeDetails
{
    public int HoursWorked { get; set; }
    public int HourlyRate { get; set; }
    public float GetSalary()
    {
        var salaryCalculator = new SalaryCalculator();
        return salaryCalculator.CalculateSalary(HoursWorked, HourlyRate);
    }
}
```
- Implementing the Dependency Inversion
```
public interface ISalaryCalculator
{
    float CalculateSalary(int hoursWorked, float hourlyRate);
}

public class SalaryCalculatorModified : ISalaryCalculator
{
    public float CalculateSalary(int hoursWorked, float hourlyRate) => hoursWorked * hourlyRate;
}

public class EmployeeDetailsModified
{
    public int HoursWorked { get; set; }
    public int HourlyRate { get; set; }

    private readonly ISalaryCalculator _salaryCalculator;

    public EmployeeDetailsModified(ISalaryCalculator salaryCalculator)
    {
        _salaryCalculator = salaryCalculator;
    }

    public float GetSalary() => _salaryCalculator.CalculateSalary(HoursWorked, HourlyRate);
}

public class Program
{
    public static void Main()
    {
        var employeeDetailsModified = new EmployeeDetailsModified(new SalaryCalculatorModified());  
        employeeDetailsModified.HourlyRate = 50;  
        employeeDetailsModified.HoursWorked = 168;  
        Console.WriteLine($"The Total Pay is { employeeDetailsModified.GetSalary() }");
    }
}
```
**Why**: The principle says that high-level modules should depend on abstraction, not on the details, of low level modules, in other words not the implementation of the low level module. Abstraction should not depend on details. Details should depend on abstraction. In simple words the principle says that there should not be a tight coupling among components (in other words two modules, two classes) of software and to avoid that, the components should depend on abstraction, in other words a contract (interface or abstract class).<br>
**Benefits**:
- Reusability. Effectively, the dependency inversion reduces coupling between different pieces of code. Thus we get reusable code.
- Maintainability. It is also important to mention that changing already implemented modules is risky. By depending on abstraction and not on concrete implementation, we can reduce that risk by not having to change high-level modules in our project. It also gives us flexibility and stability at the level of the entire architecture of our application. Our application will be able to evolve more securely and become stable and robust.

<hr>

### Sql Server

#### What is SQL Profiler?
SQL Profiler is a tool which allows system administrator to monitor events in the SQL server. This is mainly used to capture and save data about each event of a file or a table for analysis.

#### What is recursive stored procedure?
SQL Server supports recursive stored procedure which calls by itself. Recursive stored procedure can be defined as a method of problem solving wherein the solution is arrived repetitively. It can nest up to 32 levels.
```
CREATE PROCEDURE[dbo].[Fact]
(
    @Number Integer,
    @RetVal Integer OUTPUT
)
AS
DECLARE @In Integer
DECLARE @Out Integer
IF @Number != 1
BEGIN
    SELECT @In = @Number - 1
    EXEC Fact @In, @Out OUTPUT -- Same stored procedure has been called again(Recursively)
    SELECT @RetVal = @Number * @Out
END
ELSE
BEGIN
    SELECT @RetVal = 1
END
RETURN
GO
```

#### What are the differences between local and global temporary tables?
- Local temporary tables are visible when there is a connection, and are deleted when the connection is closed ```CREATE TABLE #[table name]```
- Global temporary tables are visible to all users, and are deleted when the connection that created it is closed ```CREATE TABLE ##[table name]```

#### Can we check locks in database? If so, how can we do this lock check?
Yes, we can check locks in the database. It can be achieved by using in-built stored procedure called sp_lock.

#### What is a Trigger?
Triggers are used to execute a batch of SQL code when insert or update or delete commands are executed against a table. Triggers are automatically triggered or executed when the data is modified. It can be executed automatically on insert, delete and update operations.

#### What is an IDENTITY column in insert statements?
IDENTITY column is used in table columns to make that column as Auto incremental number or a surrogate key.

#### What is the difference between ```UNION``` and ```UNION ALL```?
- ```UNION```: To select related information from two tables ```UNION``` command is used. It is similar to ```JOIN``` command.
- ```UNION ALL```: The ```UNION ALL``` command is equal to the ```UNION``` command, except that ```UNION ALL``` selects all values. It will not remove duplicate rows, instead it will retrieve all rows from all tables.
```
TableA = 1, 2, 3, 4
TableB = 3, 4, 5, 6

select * from TableA UNION select * from TableB -- = 1, 2, 3, 4, 5, 6
select * from TableA UNION ALL select * from TableB -- = 1, 2, 3, 4, 3, 4, 5, 6
```

#### JOIN!
```
TableA = 1, 2, 3, 4
TableB = 3, 4, 5, 6

select * from TableA a INNER JOIN TableB b on a.Key = b.Key -- = 3, 4 
select * from TableA a LEFT JOIN TableB b on a.Key = b.Key -- = 1, 2, 3, 4
select * from TableA a LEFT JOIN TableB b on a.Key = b.Key where b.Key is NULL -- = 1, 2
select * from TableA a RIGHT JOIN TableB b on a.Key = b.Key -- = 3, 4, 5, 6
select * from TableA a RIGHT JOIN TableB b on a.Key = b.Key where a.Key is NULL -- = 5, 6
select * from TableA a FULL OUTER JOIN TableB b on a.Key = b.Key -- = 1, 2, 3, 4, 5, 6
select * from TableA a FULL OUTER JOIN TableB b on a.Key = b.Key where a.Key is NULL OR b.Key is NULL -- = 1, 2, 5, 6
```

#### How Global temporary tables are represented and its scope?
Global temporary tables are represented with **##** before the table name. Scope will be the outside the session whereas local temporary tables are inside the session. Session ID can be found using ```@@SPID```.

#### What is Collation?
Collation is defined to specify the sort order in a table. There are three types of sort order:
- Case sensitive
- Case Insensitive
- Binary

#### Which SQL Server table is used to hold the stored procedure scripts?
```select * from Sys.SQL_Modules``` is a SQL Server table used to store the script of stored procedure. Name of the stored procedure is saved in the table called ```Sys.Procedures```.

#### What is the difference between SUBSTR and CHARINDEX in the SQL Server?
The SUBSTR function is used to return specific portion of string in a given string. But, CHARINDEX function gives character position in a given specified string.
```
SUBSTRING('Smiley', 1, 3) -- Gives result as Smi
CHARINDEX('i', 'Smiley', 1) -- Gives 3 as result as I appears in 3rd position of the string
```

#### What is ISNULL() operator?
```ISNULL``` function is used to check whether value given is ```NULL``` or not ```NULL``` in sql server. This function also provides to replace a value with the ```NULL```.

#### What is the difference between COMMIT and ROLLBACK?
Every statement between BEGIN and COMMIT becomes persistent to database when the COMMIT is executed. Every statement between BEGIN and ROOLBACK are reverted to the state when the ROLLBACK was executed.

#### What is the use of ```@@SPID```?
A ```@@SPID``` returns the session ID of the current user process.

#### What is Filtered Index?
Filtered Index is used to filter some portion of rows in a table to improve query performance, index maintenance and reduces index storage costs. When the index is created with ```WHERE``` clause, then it is called Filtered Index.

<hr>

### [Git Commands](https://github.com/AderbalFarias/snippet/blob/master/Commands/git-commands.md)

The code below is meant to be used in shell scripts where the comments are done using the **#** character
```
git clone [url] #> Clone a repository
git status #> Check status 
git fetch #> Update your remote-tracking branches 
git pull #> It does a git fetch followed by a git merge
git add --all #> Adding all changes to staging area
git commit -m "message" #> Adding files to local repository
git push origin [branch name] #> It pushes the code to remote branch
git branch #> List branches (the asterisk denotes the current branch)
git branch -a #> List all branches (local and remote)
git branch [branch name] #> Create a new branch
git branch -d [branch name] #> Delete local branch
git push -d [remote name] [branch name] #> Delete remote branch, note: remote name in most of the case is origin
git branch -m [old name] [new name] #> rename local branch
git branch -m [new name] #> rename current local branch
git push origin :[old name] [new name] #> delete the old name remote branch and push the new name local branch
git push origin -u [new name] #> reset the upstream branch for the new name local branch
git push origin --delete [branch name] #> Delete a remote branch
git checkout -b [branch name] #> It creates a new branch
git checkout -b [branch name] [commit id] #> It creates a new branch from a specific commit
git checkout [branch name] #> Switch branches
git checkout - #> Switch to the branch last checked out
git checkout -- [file-name.txt] #> Discard changes to a file
git add -A #> Add all new and changed files to the staging area
git rm -r [file-name] #> Remove a file from staging area
git stash save "message" #> It creates a stash that allows we pull code and then merge the code without commit it, like config files
git stash apply stash@{[stash number]} #> Get back the files stashed
git stash list #> Get list of stashes
git stash pop stash@{[stash number]} #> Return your repository to status before the stash (you will lose changes)
git stash drop stash@{[stash number]} #> Delete stash
git stash apply $stash_rash #> Get back the files from a deleted stash
git stash save --keep-index "message" #> Stashing files that are not in the staging area
git stash save -p "message" #> Stashing changes per hunk (if you change 5 peaces in a file it will show a dialog to you choose)
    #Dialog: Stash this hunk [y,n,q,a,d,e,?]?
    #y - stash this hunk
    #n - do not stash this hunk
    #q - quit; do not stash this hunk or any of the remaining ones
    #a - stash this hunk and all later hunks in this file
    #d - do not stash this hunk or any of the later hunks in the file
    #e - manually edit the current hunk
    #? - print help
git stash show #> show files that changed
git stash show -p #> show the full diff in the files that changed
git reset HEAD filename/*/. #> Removing files from staging area and back to current working directory
git reset --hard HEAD~1 #> Removing a commit (You will lose the changes in that commit)
git reset --soft HEAD^ #> It will reset the index to HEAD^ (previous commit), however, it will leave the changes in stage area
git reset --hard HEAD^ #> rollback changes
git commit --amend -m #> Update message of the last commit
git push --force #> In case you have already pushed code you reset to remote branch
git log --autor=[your name] #> To see your commits
git log #> To list all commits
git log --oneline #> To list all commits in one line
git gc --prune=now #> Way to delete data that has accumulated in Git but is not being referenced by anything
git remote prune origin #> Way to delete data that has accumulated in Git but is not being referenced by anything
git checkout [commit id] #> it goes back back to its commit temporarily
git mv [old name] [new name] #> Basic rename (for case sensitive it has to be different, e.g. in this file)
git merge [branch name] #> Merge items from a branch into your branch
git merge origin/[branch name] #> Merge items from a remote branch into your local branch
git grep "[your search]" #> Search the working directory for anything you need
git commit --amend -m "[new commit message]" #> Amending the most recent commit message (you need to force (-f) the next push if it is on remote already)
git cherry-pick [commit id] #> Copying a commit from one branch to another
:wq #> Exit merge screen in git bash

#Pull Request: (You have done stashes, commits and it's ready to push)
git checkout [branch root]
git pull
git checkout [branchWhereDevelopmentWasDone]
git merge [branchRoot]
git push --set-upstream origin [branchWhereDevelopmentWasDone]
```

<hr>

### Front-end JavaScript

#### What are JavaScript Data Types?
Number, String, Boolean, Object, Undefined.

#### What is the use of ```isNaN``` function?
```isNan``` function returns true if the argument is not a number otherwise it is false.

#### What are undeclared and undefined variables?
- **Undeclared variables** are those that do not exist in a program and are not declared. If the program tries to read the value of an undeclared variable, then a runtime error is encountered.
- **Undefined variables** are those that are declared in the program but have not been given any value. If the program tries to read the value of an undefined variable, an undefined value is returned.

#### What are global variables? How are these variable declared and what are the problems associated with using them?

Global variables are those that are available throughout the length of the code, that is, these have no scope. The ```var``` keyword is used to declare a local variable or object. If the var keyword is omitted, a global variable is declared.<br>
The problems faced by using global variables are the clash of variable names of local and global scope. Also, it is difficult to debug and test the code that relies on global variables.
```
globalVariable = "Test"; // it is global 
var test = "Test" // it is local
```

#### What is ```this``` keyword in JavaScript?
```this``` keyword refers to the object from where it was called.

#### Explain the working of timers in JavaScript? Also elucidate the drawbacks of using the timer, if any?
Timers are used to execute a piece of code at a set time or also to repeat the code in a given interval of time. This is done by using the functions setTimeout, setInterval and clearInterval:
- ```setTimeout(function, delay)```, function is used to start a timer that calls a particular function after the mentioned delay.
- ```setInterval(function, delay)```, function is used to repeatedly execute the given function in the mentioned delay and only halts when cancelled.
- ```clearInterval(id)```, function instructs the timer to stop.
The drawbacks, timers are operated within a single thread, and thus events might queue up, waiting to be executed.

#### What is the difference between ViewState and SessionState?
- ViewState is specific to a page in a session.
- SessionState is specific to user specific data that can be accessed across all pages in the web application.

#### What is ```===``` operator?
```===``` is called as strict equality operator which returns true when the two operands are having the same value without any type conversion.

#### Explain how can you submit a form using JavaScript?
```document.form[0].submit();```

#### What is called Variable typing in Javascript?
Variable typing is used to assign a number to a variable and the same variable can be assigned to a string.
```
// This is variable typing
i = 10;
i = "string"; 
```
#### How can you convert the string of any base to integer in JavaScript?
The ```parseInt()``` function is used to convert numbers between different bases. ```parseInt()``` takes the string to be converted as its first parameter, and the second parameter is the base of the given string. In order to convert 4F (of base 16) to integer, the code used will be ```parseInt ("4F", 16);```

#### Explain the difference between ```==``` and ```===```?
```==```checks only for equality in value whereas ```===``` is a stricter equality test and returns false if either the value or the type of the two variables are different.

#### What would be the result of 3 + 2 + "7"?
Since 3 and 2 are integers, they will be added numerically. And since 7 is a string, its concatenation will be done. So the result would be 57.

#### What do mean by ```null``` in Javascript?
The ```null``` value is used to represent no value or no object. It implies no object or null string, no valid boolean value, no number and no array object.

#### What is an undefined value in JavaScript?
Undefined value means the: 
- Variable used in the code doesn't exist.
- Variable is not assigned to any value.
- Property doesn't exist.

#### What are all the types of Pop up boxes available in JavaScript?
Alert, Confirm and Prompt.

#### Explain what is ```pop()``` method in JavaScript?
The ```pop()``` method is similar as the ```shift()`` method but the difference is that the Shift method works at the start of the ```array```. Also the ```pop()``` method take the last element off of the given array and returns it. The array on which is called is then altered.
```
var cloths = ["Shirt", "Pant", "TShirt"];
cloths.pop(); // now, cloths = ["Shirt", "Pant"];
```

#### Whether JavaScript has concept level scope?
No. JavaScript does not have concept level scope. The variable declared inside the function has scope inside the function.

#### What are the different types of errors in JavaScript?
There are three types of errors:
- **Load time errors**: Errors which come up when loading a web page like improper syntax errors are known as Load time errors and it generates the errors dynamically.
- **Run time errors**: Errors that come due to misuse of the command inside the HTML language.
- **Logical Errors**: These are the errors that occur due to the bad logic performed on a function which is having different operation.

#### What is the ```strict``` mode in JavaScript and how can it be enabled?
Strict Mode adds certain compulsions to JavaScript. Under the strict mode, JavaScript shows errors for a piece of codes, which did not show an error before, but might be problematic and potentially unsafe. Strict mode also solves some mistakes that hamper the JavaScript engines to work efficiently.<br>
Strict mode can be enabled by adding the string literal ```use strict``` above the file. This can be illustrated by the given example:
```
function myfunction() {
    "use strict";
    var v = "This is a strict mode function";
}
```

#### What is the difference between ```ex = 1;``` and ```var ex = 1;``` in JavaScript?
If you're in the global scope there is no difference but if you are in a function ```var``` will create a local variable whereas ```ex = 1;``` will look up the scope until it finds the variable or hits the global scope. 

<hr>

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

#### Explain the differences between Angular and jQuery?
The single biggest difference between Angular and jQuery is that while the former is a JS frontend framework, the latter is a JS library. Despite this, there are some similarities between the two, such as both features DOM manipulation and provides support for animation.<br>
Nonetheless, notable differences between Angular and jQuery are:
- Angular has two-way data binding, jQuery does not
- Angular provides support for RESTful API while jQuery doesn't
- jQuery doesn't offer deep linking routing though Angular supports it
- There is no form validation in jQuery whereas it is present in Angular

#### What is Angular Material?
It is a UI component library. Angular Material helps in creating attractive, consistent, and fully functional web pages as well as web applications. It does so while following modern web design principles, including browser portability and graceful degradation.

#### What is Data Binding? How many ways it can be done?
In order to connect application data with the DOM (Data Object Model), data binding is used. It happens between the template (HTML) and component (TypeScript). There are 3 ways to achieve data binding:
- **Event Binding**: Enables the application to respond to user input in the target environment
```
import { Component } from '@angular/core';    
@Component({    
  selector: 'app-root',    
  templateUrl: './app.component.html',    
  styleUrls: ['./app.component.css']    
})    
export class AppComponent {      
  onSave($event){    
    console.log("Save button is clicked!", $event);    
  }    
}    

<button (click)="onSave($event)">Save</button> <!--Event Binding-->  
```
- **Property Binding**: Enables interpolation of values computed from application data into the HTML
```
import { Component } from '@angular/core';    
@Component({    
  selector: 'app-root',    
  templateUrl: './app.component.html',    
  styleUrls: ['./app.component.css']    
})    
export class AppComponent {    
  title = "Data binding using Property Binding";      
  imgUrl="https://xxxxxx.png";    
}   

<h2>{{ title }}</h2> <!-- String Interpolation -->    
<img [src]="imgUrl" /> <!-- Property Binding -->   
```
- **Two-way Binding**: Changes made in the application state gets automatically reflected in the view and vice-versa. The ```ngModel``` directive is used for achieving this type of data binding.
```
import { Component } from "@angular/core";    
@Component({    
  selector: "app-root",    
  templateUrl: "./app.component.html",    
  styleUrls: ["./app.component.css"]    
})    
export class AppComponent {    
  fullName: string = "Hello JavaTpoint";    
}    

<h2>Two-way Binding Example</h2>    
   <input [(ngModel)]="fullName" /> <br/><br/>  <!-- Two-Way Binding -->    
<p> {{fullName}} </p>  
```

#### What is ```ngOnInit()```? How to define it?
```ngOnInit()``` is a lifecycle hook that is called after Angular has finished initializing all data-bound properties of a directive.

#### How to generate a Component in Angular using CLI?
```ng generate component MyComponentName```

#### How to generate a Service in Angular using CLI?
```ng generate service MyServiceName --module=app```

#### Samples of Component and Service in Angular
- Component TypeScript clients.component.ts
```
import { Component, OnInit } from '@angular/core';
import { ClientService } from '../../services/client.service';

import { Client } from '../../models/Client';

@Component({
    selector: 'app-clients',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {
    clients: Client[];
    totalOwed: number;

    constructor(private clientService: ClientService) { }

    ngOnInit() {
        this.clientService.getClients().subscribe(clients => {
            this.clients = clients;
            this.getTotalOwed();
        });
    }

    getTotalOwed() {
        this.totalOwed = this.clients.reduce((total, client) => {
            return total + parseFloat(client.balance.toString());
        }, 0);
    }
}
```
- Component Template clients.component.html
```
<div class="row">
    <div class="col-md-6">
        <h2>
            <i class="fa fa-users"></i> Clients</h2>
    </div>
    <div class="col-md-6">
        <h5 class="text-right text-secondary">Total Owed: {{ totalOwed | currency:"USD":"symbol" }}</h5>
    </div>
</div>
<table *ngIf="clients?.length > 0;else noClients" class="table table-striped">
    <thead class="thead-inverse">
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Balance</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <tr *ngFor="let client of clients">
            <td>{{ client.firstName }} {{ client.lastName }}</td>
            <td>{{ client.email }}</td>
            <td>{{ client.balance | currency:"USD":"symbol" }}</td>
            <td>
                <a routerLink="client/{{ client.id }}" class="btn btn-secondary btn-sm">
                    <i class="fa fa-arrow-circle-o-right"></i> Details</a>
            </td>
        </tr>
    </tbody>
</table>

<ng-template #noClients>
    <hr>
    <h5>There are no clients in the system</h5>
</ng-template> 
```
- Model Client.ts
```
export interface Client {
    id?: string;
    firstName?: string;
    lastName?: string;
    email?: string;
    phone?: string;
    balance?: number;
}
```
- Service client.service.ts
```
import { Injectable } from '@angular/core';
import { AngularFirestore, AngularFirestoreCollection, AngularFirestoreDocument } from 'angularfire2/firestore';
import { Observable } from 'rxjs/Observable';

import { Client } from '../models/Client';

@Injectable()
export class ClientService {
    clientsCollection: AngularFirestoreCollection<Client>;
    clientDoc: AngularFirestoreDocument<Client>;
    clients: Observable<Client[]>;
    client: Observable<Client>;

    constructor(private afs: AngularFirestore) {
        this.clientsCollection = this.afs.collection('clients', ref => ref.orderBy('lastName', 'asc'));
    }

    getClients(): Observable<Client[]> {
        // Get clients with the id
        this.clients = this.clientsCollection.snapshotChanges().map(changes => {
            return changes.map(action => {
                const data = action.payload.doc.data() as Client;
                data.id = action.payload.doc.id;
                return data;
            });
        });

        return this.clients;
    }

    newClient(client: Client) {
        this.clientsCollection.add(client);
    }

    getClient(id: string): Observable<Client> {
        this.clientDoc = this.afs.doc<Client>(`clients/${id}`);
        this.client = this.clientDoc.snapshotChanges().map(action => {
            if (action.payload.exists === false) {
                return null;
            } else {
                const data = action.payload.data() as Client;
                data.id = action.payload.id;
                return data;
            }
        });

        return this.client;
    }

    updateClient(client: Client) {
        this.clientDoc = this.afs.doc(`clients/${client.id}`);
        this.clientDoc.update(client);
    }

    deleteClient(client: Client) {
        this.clientDoc = this.afs.doc(`clients/${client.id}`);
        this.clientDoc.delete();
    }
}
```

#### app.routing.ts Sample
```
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './guards/auth.guard';
import { RegisterGuard } from './guards/register.guard';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AddClientComponent } from './components/add-client/add-client.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { ClientDetailsComponent } from './components/client-details/client-details.component';
import { SettingsComponent } from './components/settings/settings.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

const routes: Routes = [
    { path: '', component: DashboardComponent, canActivate:[AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent, canActivate:[RegisterGuard] },
    { path: 'client/add', component: AddClientComponent, canActivate:[AuthGuard] },
    { path: 'client/edit/:id', component: EditClientComponent, canActivate:[AuthGuard] },
    { path: 'client/:id', component: ClientDetailsComponent, canActivate:[AuthGuard] },
    { path: 'settings', component: SettingsComponent, canActivate:[AuthGuard] },
    { path: '**', component: DashboardComponent }
];

@NgModule({
    exports: [RouterModule],
    imports: [
        RouterModule.forRoot(routes)
    ],
    providers: [AuthGuard, RegisterGuard]
})
export class AppRoutingModule { }
```

#### app.component.html Sample
```
<app-navbar></app-navbar>
<div class="container">
    <flash-messages></flash-messages>
    <router-outlet></router-outlet>
</div>
```

#### app.component.ts Sample
```
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
}
```

#### app.module.ts Sample
```
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/Forms';
import { FlashMessagesModule } from 'angular2-flash-messages';
import { environment } from '../environments/environment';
import { AngularFireModule } from 'angularfire2';
import { AngularFirestoreModule } from 'angularfire2/firestore';
import { AngularFireAuthModule } from 'angularfire2/auth';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ClientsComponent } from './components/clients/clients.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { AddClientComponent } from './components/add-client/add-client.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { ClientDetailsComponent } from './components/client-details/client-details.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { SettingsComponent } from './components/settings/settings.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { AppRoutingModule } from './/app-routing.module';
import { ClientService } from './services/client.service';
import { AuthService } from './services/auth.service';
import { SettingsService } from './services/settings.service';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        DashboardComponent,
        ClientsComponent,
        SidebarComponent,
        AddClientComponent,
        EditClientComponent,
        ClientDetailsComponent,
        LoginComponent,
        RegisterComponent,
        SettingsComponent,
        NotFoundComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        FlashMessagesModule.forRoot(),
        AppRoutingModule,
        AngularFireModule.initializeApp(environment.firebase, 'clientpanel'),
        AngularFirestoreModule,
        AngularFireAuthModule
    ],
    providers: [ClientService, AuthService, SettingsService],
    bootstrap: [AppComponent]
})
export class AppModule { }
```

<hr>

### Architecture

#### [Principles](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles)
