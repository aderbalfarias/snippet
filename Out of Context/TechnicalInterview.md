### .NET Core and .NET

#### Explain what is a Middleware?
Middleware is software that's assembled into an app pipeline to handle requests and responses. ASP.NET Core provides a rich set of built-in middleware components, but in some scenarios, you might want to write a custom middleware. Middleware should follow the Explicit Dependencies Principle by exposing its dependencies in its constructor. Middleware is constructed once per application lifetime, it is possible to create a middleware pipeline with ```IApplicationBuilder``` inside the method ```public void Configure(IApplicationBuilder app)```. The ASP.NET Core request pipeline consists of a sequence of request delegates, called one after the other.<br>
The incoming requests are passes through this pipeline where all middleware is configured, and middleware can perform some action on the request before passes it to the next middleware. Same as for the responses, they are also passing through the middleware but in reverse order.

#### What are the advantages of ASP.NET Core over ASP.NET?
There are following advantages of ASP.NET Core over ASP.NET:
- It is cross-platform, so it can be run on Windows, Linux, and Mac.
- There is no dependency on framework installation because all the required dependencies are ship with our application.
- ASP.NET Core can handle more request than the ASP.NET.
- Multiple deployment options available with ASP.NET Core.

#### Program.cs file on .NET 6:
```
using CoreApp.Api.Extensions;
using CoreApp.Api.Middlewares;
using CoreApp.Api.Options.Authorization;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
var corsOrigin = builder.Configuration.GetSection("CorsOrigin").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
    });
});

// Dependency Injection
builder.Services.Services();
builder.Services.Repositories();
builder.Services.Databases(builder.Configuration.GetConnectionString("DemoConnection"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

var authenticationOption = builder.Configuration
    .GetSection(nameof(ApplicationOptions.Authentication))
    .Get<AuthenticationOptions>();

builder.Services.AddSingleton(authenticationOption);

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DemoConnection"));

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
builder.Services.AddSwagger(authenticationOption);

var oidc = builder.Configuration
    .GetSection(nameof(ApplicationOptions.OidcAuthorizationServer))
    .Get<OidcAuthorizationServerOptions>();

builder.Services.AddSingleton(oidc);
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(new string[] { JwtBearerDefaults.AuthenticationScheme, "ADFS" })
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Server", "DENY");

    await next();
});

app.UseAuthentication();

app.Logger.LogInformation($"In {app.Environment.EnvironmentName} environment");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("../swagger/v3/swagger.json", "Core App .NET v6");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
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

app.Run();
```
Key items:
- ```var builder = WebApplication.CreateBuilder(args);```
- ```builder.Services```
- ```var app = builder.Build();```
- ```app.Run();```

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
- **CONNECT**, Establishes tunnels. Proxy server to tunnel the TCP connection to the desired destination.
- **PATCH**, Making partial changes to an existing resource.

#### Explain constraints on type parameters?
Constraints inform the compiler about the capabilities a type argument must have. Without any constraints, the type argument could be any type. Constraints are specified by using the ```where``` contextual keyword. The following table lists the various types of constraints:
- ```where T : struct``` The type argument must be a non-nullable value type. Because all value types have an accessible parameterless constructor, the struct constraint implies the ```new()``` constraint and can't be combined with the ```new()``` constraint. You can't combine the ```struct``` constraint with the ```unmanaged``` constraint
- ```where T : class```	The type argument must be a reference type. This constraint applies also to any ```class```, ```interface```, ```delegate```, or ```array``` type. T must be a non-nullable reference type.
- ```where T : class?``` The type argument must be a reference type, either nullable or non-nullable. This constraint applies also to any ```class```, ```interface```, ```delegate```, or ```array``` type.
- ```where T : notnull``` The type argument must be a non-nullable type. The argument can be a non-nullable reference type or value type.
- ```where T : unmanaged``` The type argument must be a non-nullable unmanaged type. The unmanaged constraint implies the struct constraint and can't be combined with either the ```struct``` or ```new()``` constraints.
- ```where T : new()```	The type argument must have a ```public``` parameterless constructor. When used together with other constraints, the ```new()``` constraint must be specified last. The ```new()``` constraint can't be combined with the ```struct``` and ```unmanaged``` constraints.
- ```where T : <base class name>``` The type argument must be or derive from the specified base ```class```. T must be a non-nullable reference type derived from the specified base ```class```.
- ```where T : <base class name>?``` The type argument must be or derive from the specified base ```class```. T may be either a nullable or non-nullable type derived from the specified base ```class```.
- ```where T : <interface name>``` The type argument must be or implement the specified ```interface```. Multiple ```interface``` constraints can be specified. The constraining interface can also be generic. T must be a non-nullable type that implements the specified ```interface```.
- ```where T : <interface name>?``` The type argument must be or implement the specified ```interface```. Multiple ```interface``` constraints can be specified. The constraining interface can also be generic. T may be a nullable reference type, a non-nullable reference type, or a value type. T may not be a nullable value type.
- ```where T : U``` The type argument supplied for T must be or derive from the argument supplied for U. In a nullable context, if U is a non-nullable reference type, T must be non-nullable reference type. If U is a nullable reference type, T may be either nullable or non-nullable.
```
public class EmployeeList<T> where T : Employee, IEmployee, System.IComparable<T>, new() {}

public class GenericList<T> where T : Employee {}

public static void OpEqualsTest<T>(T s, T t) where T : class {}

// Multiple parameters, and multiple constraints to a single parameter
class Base { }
class Test<T, U>
    where U : struct
    where T : Base, new()
{ }

// T is a type constraint in the context of the Add method, and an unbounded type parameter in the context of the List class
public class List<T>
{
    public void Add<U>(List<U> items) where U : T {/*...*/}
}
```

<hr>

### C# Versus

#### ```abstract class``` vs ```interface```
Abstract classes and interfaces are both mechanisms for defining contracts and promoting code reuse in object-oriented programming but thy have some distinct characteristics:
- Abstract Class:
	- Is a class that cannot be instantiated and serves as a blueprint for derived classes.
	- It can contain both implemented and abstract methods.
	- Abstract methods are declared without an implementation and must be overridden by the derived classes.
	- It can have fields, properties, constructors, and non-abstract methods.
	- It can provide a default implementation for some or all of its members.
	- A class can inherit from only one abstract class.
	- It is used when you want to provide a common base with some default behavior for derived classes to extend and override.
- Interface:
	- Is a contract that defines a set of methods and properties that a class must implement.
	- It contains only method and property declarations without any implementation.
	- All methods and properties in an interface are implicitly abstract and public.
	- A class can implement multiple interfaces, allowing for multiple inheritance of behavior.
	- It is used when you want to define a common set of functionality that can be implemented by unrelated classes.
	- Since C# 8.0 you can have default methods and you also can change modifiers.

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

#### ```Thread``` vs ```Task```
Both the ```Thread``` class and the ```Task``` class are used for parallel programming in C#. A ```Thread``` is a lower-level implementation while a ```Task``` is a higher-level implementation. It takes resources while a ```Task``` does not. It also provides more control than the ```Task``` class. A ```Thread``` should be preferred for any long-running operations, while a ```Task``` should be preferred for any other asynchronous operations.

#### ```String``` vs ```string```
Essentially, there is no difference between string and String in C#.<br>
```String``` is a ```class``` in the .NET framework in the System namespace. The fully qualified name is ```System.String```. Whereas, the lower case ```string``` is an alias of ```System.String```.

#### ```Action``` vs ```Func``` vs ```Predicate```
- ```Action```: Delegate (pointer) to a method that takes zero, one or more input parameters but doesn't return anything.<br>
- ```Func```: Delegate (pointer) to a method that takes zero, one or more input parameters and returns a value or reference.<br>
- ```Predicate```: A special form of Func and mainly used to validate something and return a bool. It is mainly used with collections to whether the item in the collection is valid or not. Basically, its a wrapper of Func like ```Func<T, bool>```.<br>
*When to use that*?<br>
Action is useful if we don’t want to return any result. But if we want to return a result, we could use Func. The predicate is mainly used to validate any condition.

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
In short, in the **Stack** are stored value types (types inherited from ```System.ValueType``` like ```bool, int, long, decimal, float, short, struct```) and in the **Heap** are stored reference types (types inherited from ```System.Object``` such as ```string, object, dynamic```).<br>
Stack is responsible for keeping track what is actually executing and where each executing thread is (each thread has its own stack).<br>
Heap is responsible for keeping track of the data, or more precise objects.<br>
Note: You can use boxing and unboxing in order to move objects between stack and heap.

#### ```class``` vs Object
In short, a ```class``` is the definition of an object, and an object is an instance of a ```class```.<br>
The ```class``` in c# is nothing but a collection of various data members (fields, properties, events, etc.) and member functions. The object in c# is an instance of a ```class``` to access the defined properties and methods.

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
```Object.ToString()``` cannot handle ```null``` values which means the *Null reference exception* will be thrown when trying to use ```.ToString()``` on an object which is ```null```, in the other hand ```Convert.ToString()``` can handle ```null``` values it won't generate *Null reference exception*.

#### ```while``` vs ```for```
The difference is that the ```for``` loop is used when you know how many times you need to iterate through the code, on the other hand, the ```while``` loop is used when you need to repeat something until a given statement is true.
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

#### Value Type vs Reference Type
- A value type holds a data value within its own memory space. ```int x = 30;```
- Reference type stores the address of the object where the value is being stored. It is a pointer to another memory location. ```string y = "Hello World!";```

#### Constants vs Readonly Variables 
- Constants can be declared in methods or global context they are declared with ```const``` modifier, it is used for immutable values, they are evaluated at compile time, user-defined types, including classes, structs, and arrays, cannot be ```const```.
- Readonly cannot be delcared in methods, they use ```readonly``` modifier, they are evaluated at runtime, it also can hold reference-type variables, it is mostly used when its actual value is unknown before the runtime and it can only be inilialised at the time of declaration or in a constructor.

#### ```public``` vs ```static``` vs ```void``` 
- ```public``` declared variables or methods are accessible anywhere in the application. 
- ```static``` declared variables or methods are globally accessible without creating an instance of the ```class```. ```static``` member are by default not globally accessible it depends upon the type of access modified used. The compiler stores the address of the method as the entry point and uses this information to begin execution before any objects are created. 
- ```void``` is a type modifier that states that the method or variable does not return any value.

#### ```ref``` vs ```out``` parameters
An argument passed as ```ref``` must be initialized before passing to the method whereas ```out``` parameter needs not to be initialized before passing to a method.

#### ```Array``` vs ```Arraylist```
In an ```array```, we can have items of the same type only. The size of the ```array``` is fixed when compared. To an ```arraylist``` is similar to an array, but it doesn't have a fixed size.

#### ```System.Array.CopyTo()``` and ```System.Array.Clone()```
Using ```Clone()``` method, we creates a new array object containing all the elements in the original Array and using ```CopyTo()``` method all the elements of existing array copies into another existing array. Both methods perform a shallow copy.

#### ```is``` vs ```as``` operators
- ```is``` operator is used to check the compatibility of an object with a given type, and it returns the result as ```Boolean```.
- ```as``` operator is used for casting of an object to a type or a ```class```

#### ```throw``` vs ```throw ex```
```throw``` statement preserves original error stack whereas ```throw ex``` have the stack trace from their ```throw``` point. It is always advised to use ```throw``` because it provides more accurate error information.

#### Sync vs Async
- Synchronous, data is sent in form of blocks or frames. Full duplex type, compulsory sincronization.
- Asynchronous, data is sent in form of byte or character. Half duplex transmission, start bits and stop bits are added with data it does not require synchronization.

#### ```break``` vs ```continue```
```break``` leaves the loop completely and executes the statements after the loop. Whereas ```continue``` leaves the current iteration and executes with the next value in the loop. ```break``` completely exits the loop. ```continue``` skips the statements after the ```continue``` statement and keeps looping.

#### ```struct``` vs ```class```
 
Class and struct are both user-defined data types, but have some major differences:
##### ```struct```:
- The struct is a value type in C# and it inherits from System.Value type.
- Struct is usually used for smaller amounts of data.
- Struct can't be inherited from other types.
- A structure can't be abstract.
- No need to create an object with a new keyword.
- Do not have permission to create any default constructor.
##### ```class```:
- The class is a reference type in C# and it inherits from the System.Object type.
- Classes are usually used for large amounts of data.
- Classes can be inherited from other classes.
- A class can be an abstract type.
- We can create a default constructor.

<hr>

### C# Keywords

#### Explain the keyword ```lock``` in C#?
The ```lock``` keyword ensures that one thread is executing a piece of code at one time. The lock keyword ensures that one thread does not enter a critical section of code while another thread is in that critical section. Lock is a keyword shortcut for acquiring a lock for the piece of code for only one thread.
```
static readonly object _object = new object();  
lock (_object)  
{  
	Thread.Sleep(100); // In order to test 
	Console.WriteLine(Environment.TickCount);  
}  
```

#### Explain the keyword ```virtual```?
The ```virtual``` keyword is used to modify a method, property, indexer, or event declaration and allow for it to be overridden in a derived class. You cannot use the virtual modifier with the ```static, abstract, private, override``` modifiers.

#### Explain what happen if a method has the ```abstract``` keyword?
It must be overridden.

#### Explain the keyword ```volatile```?
The ```volatile``` keyword indicates that a field might be modified by multiple threads that are executing at the same time. The compiler, the runtime system, and even hardware may rearrange reads and writes to memory locations for performance reasons. Fields that are declared volatile are not subject to these optimizations.

#### Explain the keyword ```using```?
The using keyword has three major uses:<br>
- The using statement defines a scope at the end of which an object will be disposed.
- The using directive creates an alias for a namespace or imports types defined in other namespaces.
- The using static directive imports the members of a single ```class```.

#### Explain the keyword ```unsafe```?
The ```unsafe``` keyword denotes an unsafe context, which is required for any operation involving pointers. You can use the ```unsafe``` modifier in the declaration of a type or a member. The entire textual extent of the type or member is therefore considered an unsafe context

#### Explain the keyword ```sealed```?
When applied to a ```class```, the ```sealed``` modifier prevents other classes from inheriting from it. Like, ```class``` **B** inherits from ```class``` **A**, but no ```class``` can inherit from ```class``` **B**
```
class A {}
sealed class B : A {}
```

<hr>

### C# General 

#### What is ```delegate``` and how to use it?
A ```delegate``` is a type that represents references to methods with a particular parameter list and return type. When you instantiate a delegate, you can associate its instance with any method with a compatible signature and return type. You can invoke (or call) the method through the delegate instance.<br>
*Delegates are used to pass methods as arguments to other methods*. Event handlers are nothing more than methods that are invoked through delegates.<br>
*See versus section ```Action``` vs ```Func``` vs ```Predicate```* 

#### Does C# support multiple inheritance?
No, however, you can implement multiple interfaces.

#### When ```break``` is used inside two nested ```for``` loops (for inside for), the ```break``` is inside second loop, what does happen? 
It breaks from the inner loop only.

#### Explain what LINQ is?
LINQ in an acronym for Language Integrated Query, it allow data manipulation, regardless of the data source which means that it supports many data providers like .NET Framework collections, SQL Server databases, MySql databases, ADO.NET databases, XML documents, and any collection of objects that support ```IEnumerable``` or generic ```IEnumerable<T>``` interfaces, *In short, LINQ bridges the gap between the world of objects and the world of data*.

#### Explain the difference between Task and Thread in .NET?
- Thread represents an actual OS-level thread, with its own stack and kernel resources. Thread allows the highest degree of control; you can ```Abort()``` or ```Suspend()``` or ```Resume()``` a thread, you can observe its state, and you can set thread-level properties like the stack size, apartment state, or culture. ThreadPool is a wrapper around a pool of threads maintained by the CLR.
- The ```Task class``` from the Task Parallel Library offers the best of both worlds. Like the ThreadPool, a task does not create its own OS thread. Instead, tasks are executed by a TaskScheduler; the default scheduler simply runs on the ThreadPool. Unlike the ThreadPool, Task also allows you to find out when it finishes, and (via the generic Task) to return a result.

#### What garbage collection is and how it works. Provide example of how you can enforce it in .NET?
Garbage collection is a low-priority process that serves as an automatic memory manager which manages the allocation and release of memory for the applications. Each time a new object is created, the CLR allocates memory for that object from the managed **heap**. As long as free memory space is available in the managed heap the runtime continuos to allocate space for new objects. However memory is not infinite and when heap memory is full garbage collection comes to free some memory.<br>
When **Garbage Collector** performs a collection it checks for objects in the managed heap that are no longer being used by the applications and performs the necessary operations to relcaim the memory, it will stop all running threads and find the objects in the heap that aren't being accessed by the main program and delete them, then reorganize all the objects left in the heap in order to make space and adjust all pointers to these objects in the heap and the stack.<br>
It can be implemented by using the ```IDisposable interface```.<br>
```System.GC.Collect() // Force garbage collection```<br>
It's important to note that manually enforcing garbage collection is generally not necessary and should be used sparingly. The .NET runtime's garbage collector is highly optimized and performs automatic garbage collection based on various heuristics, balancing memory reclamation with performance considerations.

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

#### Can we use ```this``` keyword within a static method?
No, We can't use ```this``` in a static method because we can only use static variables/methods in a static method.

#### How to use extension methods?
An extension method is a ```static``` method of a ```static class```, where the ```this``` modifier is applied to the first parameter. 
The type of the first parameter will be the type that is extended.

#### Can a ```private virtual``` method can be overridden?
No, because they are not accessible outside the ```class```.

#### How can we sort the elements of the Array in descending order?
Using ```Sort()``` methods followed by ```Reverse()``` method.

#### What are generics in C#?
Generics are used to make reusable code classes to decrease the code redundancy, increase type safety, and performance. Using generics, we can create collection classes. To create generic collection, ```System.Collections.Generic``` namespace should be used instead of classes such as ```ArrayList``` in the ```System.Collections``` namespace. Generics promotes the usage of parameterized types.
```
public class List<T> { ... }
public class MyClass<T> where T : IComparable<T> { ... }
```

#### What are the access modifiers in C# and list them?
All types and type members have an accessibility level. The accessibility level controls whether they can be used from other code in your assembly or other assemblies. Use the following access modifiers to specify the accessibility of a type or member when you declare it:
- ```public```: The type or member can be accessed by any other code in the same assembly or another assembly that references it.
- ```private```: The type or member can be accessed only by code in the same ```class``` or ```struct```.
- ```protected```: The type or member can be accessed only by code in the same ```class```, or in a ```class``` that is derived from that ```class```.
- ```internal```: The type or member can be accessed by any code in the same assembly, but not from another assembly.
- ```protected internal```: The type or member can be accessed by any code in the assembly in which it's declared, or from within a derived ```class``` in another assembly.
- ```private protected```: The type or member can be accessed only within its declaring assembly, by code in the same ```class``` or in a type that is derived from that ```class```.

#### What are circular references?
Circular reference is situation in which two or more resources are interdependent on each other causes the lock condition and make the resources unusable.

#### List down the commonly used types of exceptions
- ```NullReferenceException```
- ```ArgumentException```
- ```ArgumentNullException```
- ```ArithmeticException```
- ```OverflowException``` 
- ```IndexOutOfRangeException``` 
- ```InvalidCastException```
- ```OutOfMemoryException```
- ```StackOverflowException```

#### What are the different ways a method can be overloaded?
Methods can be overloaded using different data types for a parameter, different order of parameters, and different number of parameters.

#### Explain property in C#?
A property is a member that provides a flexible mechanism to read, write, or compute the value of a private field. Properties can be used as if they are public data members, but they are actually special methods called accessors. This enables data to be accessed easily and still helps promote the safety and flexibility of methods.

#### What is Regular Expression?
A regular expression is a sequence of characters that specifies a search pattern.<br>
In C#, Regular Expression/Regex is a pattern which is used to parse and check whether the given input text is matching with the given pattern or not.

#### What is ```enum``` in C#? 
An enum is a value type with a set of related named constants often referred to as an enumerator list. The enum keyword is used to declare an enumeration. It is a primitive data type that is user-defined.<br>
An enum type can be an integer (float, int, byte, double, etc.). But if you use it beside int it has to be cast.<br>
Some points about enum:
- Enums are enumerated data types in c#.
- By default, the first enumerator has the value 0, and the value of each successive enumerator is increased by 1.
- The default type is int, and the approved types are byte, sbyte, short, ushort, uint, long, and ulong.
- Enums are strongly typed constant. They are strongly typed, i.e. an enum of one type may not be implicitly assigned to an enum of another type even though the underlying value of their members is the same.

#### Explain Hashtable in C#?
The Hashtable is a non-generic collection that stores key-value pairs, similar to generic `Dictionary<TKey, TValue>` collection. It optimizes lookups by computing the hash code of each key and stores it in a different bucket internally and then matches the hash code of the specified key at the time of accessing values.<br>
Some key points about Hashtables in C#:
- A Hashtable stores data as a collection of **key-value pairs**, where each key must be unique within the Hashtable
- The Hashtable internally uses a hash function to generate a hash code for each key
- Hashtables automatically resize themselves as more elements are added, ensuring efficient performance
- Hashtables rely on the GetHashCode() and Equals() methods of the keys to determine equality
- The elements in a Hashtable are not ordered
- Hashtable is not thread-safe for concurrent read/write operations
```
Hashtable hashtable = new Hashtable();

// Adding key-value pairs to the Hashtable
hashtable.Add("Name", "John");
hashtable.Add("Age", 30);
hashtable["City"] = "New York";

// Accessing values using keys
Console.WriteLine(hashtable["Name"]);  // Output: John
Console.WriteLine(hashtable["Age"]);   // Output: 30
Console.WriteLine(hashtable["City"]);  // Output: New York

// Checking if a key exists
bool containsKey = hashtable.ContainsKey("Name");

// Removing a key-value pair
hashtable.Remove("Age");

// Iterating over key-value pairs
foreach (DictionaryEntry entry in hashtable)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}
```
Note: it's generally recommended to use Dictionary instead of Hashtable, as Dictionary provides type safety, better performance, and better integration with generics.

#### Explain asynchronous in C#?
Asynchronous programming in C# allows you to **write code that can run concurrently, performing tasks without blocking the main execution thread**. It enables you to execute operations in the background and efficiently utilize system resources while keeping your application responsive. Asynchronous programming is especially useful when dealing with I/O operations, network requests, or long-running tasks. In C#, you can achieve asynchronous programming using the ```async``` and ```await``` keywords, see definitions:
- `async` keyword: The `async` keyword is used to define an asynchronous method. An asynchronous method can contain the `await` keyword to indicate points where the method can yield control back to the caller without blocking the thread.
- `await` keyword: The `await` keyword is used within an `async` method to pause the execution of the method until a particular asynchronous operation completes. The `await` keyword ensures that the method doesn't block the calling thread and allows it to perform other tasks.
- Asynchronous Methods: An asynchronous method typically returns a `Task` or `Task<T>` representing the asynchronous operation. The method is marked with the `async` keyword, and it can contain `await` expressions to await other asynchronous operations.<br>

By using asynchronous programming, you can achieve the following benefits:
- Improved Responsiveness: Asynchronous operations allow your application to remain responsive and handle other tasks while waiting for long-running operations to complete.
- Efficient Resource Utilization: Asynchronous programming enables efficient use of system resources, as threads are not blocked while waiting for I/O or other operations.
- Scalability: Asynchronous programming enables handling a large number of concurrent operations without requiring a high number of threads.
<hr>

### Object-oriented programming (OOP)

#### What is OOP and how does it relate to the .NET Frameworks?
OOP allows .NET developers to create classes containing methods, properties, fields, events and other logical modules. It also lets developers create modular programs with they can assemble as applications and reuse code. OOP has four basic features: encapsulation, abstraction, polymorphism and inheritance.

#### What is Encapsulation?
It is one of the four basic features of OOP and refers to an object's ability to hide data and behaviour that are not necessary to its user. Encapsulation helps to keep data from unwanted access through biding code and data in an object which is the basic single self-contained unit of a system. Encapsulation is used to restrict access to the members of a ```class``` so as to prevent the user of a given ```class``` from manipulating objects in ways that are not intended by the designer, it also has the principle of hiding the state of an object by using private or protected modifiers. Benefits of it:
- Protection of data from accidental corruption.
- Specification of the accessibility of each of the members of a ```class``` to the code outside the ```class```.
- Flexibility and extensibility of the code and reduction in complexity.
- Lower coupling between objects and hence improvement in code maintainability.
- Less likely that other objects can modify the state or behaviour of the object in question.
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
Abstraction is a principle of OOP and it is used to hide the implementation details and display only essential features of the object. The word abstract means a concept or an idea not associated with any specific instance. We apply the same meaning of abstraction by making classes not associated with any specific instance. Abstraction is needed when we need to only inherit from a certain ```class```, but do not need to instantiate objects of that ```class```. In such a case the base ```class``` can be regarded as "Incomplete". Such classes are known as an "```abstract``` Base ```class```". 
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
Polymorphism means providing an ability to take more than one form, polymorphism provides an ability for the classes to implement different methods that are called through the same name and it also provides an ability to invoke the methods of a derived ```class``` through base ```class``` reference during runtime based on our requirements.
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
Inheritance is one of the core concepts of OOP languages. It is a mechanism where you can to derive a ```class``` from another ```class``` for a hierarchy of classes that share a set of attributes and methods. It allows developers to reuse, extend and modify the behaviour defined in other classes, **the ```class``` whose members are inherited is called the base ```class``` and the ```class``` that inherit those members is called the derived ```class```**. Inheritance allows you to define a base class that provides specific functionality (data and behaviour) and to define derived classes that either inherit or override that functionality.<br>
*e.g.:* A base ```class``` called ```Vehicle```, and then derived classes called ```Truck, Car, Motorcycle``` all of which inherit the attributes of vehicle.
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
Inversion of control is a *Principle in software engineering* by which the control of objects or portions of a program is transferred to a container or framework. It is most often used in the context of OOP.

<hr>

### Design Patterns

#### Repository
Repositories are classes or components that encapsulate the logic required to access data sources. They centralize common data access functionality, providing better maintainability and decoupling the infrastructure or technology used to access databases from the domain model layer.<br>
The Repository pattern is a well-documented way of working with a data source. In the book Patterns of Enterprise Application Architecture, Martin Fowler describes a repository as follows:<br>
*A repository performs the tasks of an intermediary between the domain model layers and data mapping, acting in a similar way to a set of domain objects in memory. Client objects declaratively build queries and send them to the repositories for answers. Conceptually, a repository encapsulates a set of objects stored in the database and operations that can be performed on them, providing a way that is closer to the persistence layer. Repositories, also, support the purpose of separating, clearly and in one direction, the dependency between the work domain and the data allocation or mapping.*<br>
Basically, a repository allows you to populate data in memory that comes from the database in the form of the domain entities. Once the entities are in memory, they can be changed and then persisted back to the database through transactions.<br>
A Repository Pattern can be implemented in the following ways:<br>
- **One repository per entity (non-generic)**: This type of implementation involves the use of one repository ```class``` for each entity. For example, if you have two entities Order and Customer, each entity will have its own repository.
- **Generic repository**: A generic repository is the one that can be used for all the entities, in other words, it can be either used for Order or Customer or any other entity.

#### Dependency Injection (DI)
Dependency Injection is a software design pattern that allows us to develop loosely coupled code, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies. DI reduces tight coupling between software components and also enables us to better manage future changes and other complexity in software. The purpose of DI is to make code maintainable.<br>
Advantages:
- Reduces ```class``` coupling.
- Increases code reusability.
- Improves code maintainability.
- Make unit testing possible.

The Dependency Injection pattern involves 3 types of classes:
- Client ```class```: The client ```class``` (dependent ```class```) is a ```class``` which depends on the service ```class```
- Service ```class```: The service ```class``` (dependency) is a ```class``` that provides service to the client ```class```.
- Injector ```class```: The injector ```class``` injects the service ```class``` object into the client ```class```.

Types of Dependency Injection: 
- Constructor Injection: In the constructor injection, the injector supplies the service (dependency) through the client ```class``` constructor.
- Property Injection: In the property injection (aka (also known as) the Setter Injection), the injector supplies the dependency through a public property of the client ```class```.
- Method Injection: In this type of injection, the client ```class``` implements an interface which declares the method(s) to supply the dependency and the injector uses this interface to supply the dependency to the client ```class```.

Dependency Lifetimes<br>
At registration time, dependencies require a lifetime definition. The service lifetime defines the conditions under which a new service instance will be created. Below are the lifetimes defined by the ASP.NET DI framework. The terminology may be different if you choose to use a different framework.<br>
- **Transient** – Created every time they are requested
- **Scoped** – Created once per scope. Most of the time, scope refers to a web request. But this can also be used for any unit of work, such as the execution of an Azure Function.
- **Singleton** – Created only for the first request. If a particular instance is specified at registration time, this instance will be provided to all consumers of the registration type.

#### [Chain of Responsibility](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Behavioral%20Patterns/ChainOfResponsibility.linq)
Chain of Responsibility is a **Behavioural Pattern** that simplifies object interconnections. Instead of senders and receivers maintaining references to all candidate receivers, each sender keeps a single reference to the head of the chain, and each receiver keeps a single reference to its immediate successor in the chain.<br>
The derived classes know how to satisfy Client requests. If the "current" object is not available or sufficient, then it delegates to the base ```class```, which delegates to the "next" object, and the circle of life continues.

#### [Observer](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Behavioral%20Patterns/Observer.linq)
Observer is a **Behavioural Pattern** in which an object, called the subject, maintains a list of its dependents, called observers, and notifies them automatically of any state changes, usually by calling one of their methods.<br>
Observer pattern is used when there is a one-to-many relationship between objects such as if one object is modified, its dependent objects are to be notified automatically. Observer pattern falls under behavioural pattern category.

#### [Abstract Factory](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Creational%20Patterns/AbstractFactory.linq)
Abstract Factory is a **Creational Pattern** that lets you produce families of related objects without specifying their concrete classes.<br>
Provide a level of indirection that abstracts the creation of families of related or dependent objects without directly specifying their concrete classes. The "factory" object has the responsibility for providing creation services for the entire platform family. Clients never create platform objects directly, they ask the factory to do that for them.

#### [Singleton](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Creational%20Patterns/Singleton.linq)
Singleton is a **Creational Pattern** which makes the ```class``` of the single instance object responsible for the creation, initialization, access, and enforcement. Declare the instance as a private static data member. Provide a public static member function that encapsulates all initialization code, and provides access to the instance.<br>
The Singleton pattern can be extended to support access to an application-specific number of instances.<br>
The Singleton pattern ensures that a ```class``` has only one instance and provides a global point of access to that instance. It is named after the singleton set, which is defined to be a set containing one element. The office of the President of the United States is a Singleton.

#### [Facade](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Structural%20Patterns/Facade.linq)
Facade is a **Structural Pattern** that hides the complexities of the system and provides an interface to the client using which the client can access the system. This pattern involves a single ```class``` which provides simplified methods required by client and delegates calls to methods of existing system classes.<br>
The facade pattern is appropriate when you have a complex system that you want to expose to clients in a simplified way, or you want to make an external communication layer over an existing system which is incompatible with the system. Facade deals with interfaces, not implementation. Its purpose is to hide the internal complexity behind a single interface that appears simple on the outside.

#### [Adapter](https://github.com/AderbalFarias/snippet/blob/master/Design%20Patterns/Structural%20Patterns/Adapter.linq)
Adapter is a **Structural Pattern** which converts the interface of a ```class``` into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.<br>
Suppose you have a Bird ```class``` with ```fly()```, and ```makeSound()``` methods. And also a ToyDuck ```class``` with ```squeak()``` method. Let’s assume that you are short on ToyDuck objects and you would like to use Bird objects in their place. Birds have some similar functionality but implement a different interface, so we can't use them directly. So we will use adapter pattern.

<hr>

### SOLID

#### S - Single responsibility Principle
**Definition**: The Single responsibility principle states that every module or ```class``` should have responsibility for a single part of the functionality provided by the software.<br>
**Example**: If we have two reasons to change a ```class```, we have to split the functionality into two classes. Each ```class``` will handle only one responsibility lets say you have ```class Client``` which has register client and sends email functionalities, that is wrong, they should be splitted in two different classes (```class Client``` and ```class Email```) each one responsible for its own functionality.<br>
**Why**: If we put more than one functionality in one ```class``` then it introduces coupling between two functionalities. So, if we change one functionality there is a chance we broke coupled functionality, which requires another round of testing to avoid any bug in the production environment. It reduces bug fixes and testing time once an application goes into the maintenance phase.<br>
**Benefits**:
- Reduction in complexity of a code. A code is based on its functionality. A method holds logic for a single functionality or task. So, it reduces the code complexity.
- Increased readability, extensibility, and maintenance. Each method has a single functionality so it is easy to read and maintain. 
- Reusability and reduced error. As code separates based functionality so if the same functionality uses somewhere else in an application then don't write it again.
- Better testability. In the maintenance, when a functionality changes then we don't need to test the entire model.
- Reduced coupling. It reduced the dependency code. A method's code doesn't depend on other methods.

#### O - Open-Closed Principle
**Definition**: The open/closed principle states that software entities (classes, modules, functions) should be open for extensions, but closed for modification.<br>
**Example**: An implementation (of a ```class``` or ```function```), once its logic and/or functionality is created, should be closed for further modification, you may use code refactoring in order to resolve errors of implementation, in the other hand the implementation (of a ```class``` or function) is open for extension of its logic and/or functionality.
- Wrong Implementation for Mortgage ```class```:
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
So if there is a new mortgage type added there is no need to modify the logic of exiting classes, just extend the functionality by inheriting an interface (Mortgage interface in this case and create a new ```class``` which will have the implementation for ```CalculateInterest()``` method. I am using an interface but it could be an ```abstract class``` as well.<br>
**Why**: When a single change to a program results in a cascade of changes to dependent modules, that program exhibits the undesirable attributes that we have come to associate with 'bad' design. The program becomes fragile, rigid, unpredictable, and unreusable. A function that is doing too many things outside the realm of its responsibilities creates unnecessary entanglements that make it harder to read and debug.<br>
**Benefits**:
- Extensibility. Where the design modules never change. When requirements change, you extend the behaviour of such modules by adding new code, not by changing old code that already works in order to prevent cascade changes to dependent modules.
- Maintainability. The main benefit of this approach is that an interface introduces an additional level of abstraction which enables loose coupling. The implementations of an interface are independent of each other and don't need to share any code.
- It makes the code readable, testable, changeable, and reusable.

#### L - Liskov Substitution Principle
**Definition**: The Liskov substitution principle states that if S is a subtype of T, then objects of type T may be replaced (or substituted) with objects of type S.<br>
We can formulate this mathematically as:
- Let ϕ(x) be a property provable about objects x of type T.
- Then ϕ(y) should be valid for objects y of type S, where S is a subtype of T. <br> 
##### More generally, it states that objects in a program should be replaceable with instances of their subtypes without altering the correctness of that program.
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
It's not always true that one must make changes in the inheritance tree but making changes at the ```class``` and method level also resolves problems, but the solution above was done by changing the inheritance tree.<br>
**Why**: Places in implementation (```class```/```function```) that use a base ```class``` (in other words consume a service of a base ```class```), must work correctly when the base ```class``` object is replaced by a child ```class``` (derived class) object.<br>
**Benefits**:
- Compatibility. It enables the binary compatibility between multiple releases and patches. In other words, It keeps the client code away from being impacted.
- Type Safety. It's the easiest approach to handle type safety with inheritance, as types are not allowed to vary when inheriting.
- Maintainability. Code that adheres to Liskov substitution is loosely dependent on each other, makes the right abstraction, and encourages code reusability.

#### I - Interface Segregation Principle
**Definition**: The interface segregation principle states not to force a client to depend on methods it does not use. Do not add additional functionality to an existing interface by adding new methods. Instead, create a new interface and let your ```class``` implement multiple interfaces if needed.<br>
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
**Why**: Do not design a big fat interface that forces the client to implement a method that is not required by it, instead of designing a small interface. So by doing this ```class``` only implement the required set of interface(s).<br>
**Benefits**:
- Faster Compilation. If you have violated interface segregation i.e. stuffed methods together in the interface, and when method signature changes, you need to recompile all the derived classes.
- Reusability. "Fat interfaces" (interfaces with additional useless methods) lead to inadvertent coupling between classes. Thus, an experienced dev knows coupling is the bane of reusability.
- Maintainability. By avoiding unneeded dependencies, the system becomes easier to understand, lighter to test, quicker to change. Similarly, to the reader of your code, it would be harder to get an idea of what your ```class``` does from the ```class``` declaration line. So, if dev sees only the one god-interface that may have inherited other interfaces it will likely not be obvious. Compare ```MyMachine : IMachine``` to ```MyMachine : IPrinter, IScanner, IFaxer```. The latter tells you a lot, the first makes you guess at best.

#### D - Dependency Inversion Principle (Dependency injection)
**Definition**: The dependency inversion principle is a way to decouple software modules. This principle states that:
- High-level modules should not depend on low-level modules. Both should depend on abstractions.
- Abstractions should not depend on details. Details should depend on abstractions.<br>
##### 
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
**Why**: The principle says that high-level modules should depend on abstraction, not on the details, of low-level modules, in other words not the implementation of the low-level module. Abstraction should not depend on details. Details should depend on abstraction. In simple words the principle says that there should not be a tight coupling among components (in other words two modules, two classes) of software and to avoid that, the components should depend on abstraction, in other words, a contract (```interface``` or ```abstract class```).<br>
**Benefits**:
- Reusability. Effectively, the dependency inversion reduces coupling between different pieces of code. Thus we get reusable code.
- Maintainability. It is also important to mention that changing already implemented modules is risky. By depending on abstraction and not on concrete implementation, we can reduce that risk by not having to change high-level modules in our project. It also gives us flexibility and stability at the level of the entire architecture of our application. Our application will be able to evolve more securely and become stable and robust.

<hr>

### Sql Server

#### What is SQL Profiler?
SQL Profiler is a tool which allows system administrator to monitor events in the SQL server. This is mainly used to capture and save data about each event of a file or a table for analysis.

#### What is recursive stored procedure?
SQL Server supports recursive stored procedure which calls by itself. A recursive stored procedure can be defined as a method of problem-solving wherein the solution arrives repetitively. It can nest up to 32 levels.
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
- Global temporary tables are visible to all users and are deleted when the connection that created it is closed ```CREATE TABLE ##[table name]```

#### Can we check locks in the database? If so, how can we do this lock check?
Yes, we can check locks in the database. It can be achieved by using an in-built stored procedure called sp_lock.

#### What is a Trigger?
Triggers are used to execute a batch of SQL code when insert or update or delete commands are executed against a table. Triggers are automatically triggered or executed when the data is modified. It can be executed automatically on insert, delete and update operations.

#### What is an IDENTITY column in insert statements?
IDENTITY column is used in table columns to make that column as Auto incremental number or a surrogate key.

#### What is the difference between ```UNION``` and ```UNION ALL```?
- ```UNION```: To select related information from two tables ```UNION``` command is used. It is similar to ```JOIN``` command.
- ```UNION ALL```: The ```UNION ALL``` command is equal to the ```UNION``` command, except that ```UNION ALL``` selects all values. It will not remove duplicate rows, instead, it will retrieve all rows from all tables.
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

#### What is ```ISNULL()``` operator?
```ISNULL``` function is used to check whether value given is ```NULL``` or not ```NULL``` in sql server. This function also provides to replace a value with the ```NULL```.

#### What is the difference between COMMIT and ROLLBACK?
Every statement between ```BEGIN``` and ```COMMIT``` becomes persistent to database when the ```COMMIT``` is executed. Every statement between ```BEGIN``` and ```ROOLBACK``` are reverted to the state when the ```ROLLBACK``` was executed.

#### Explain ```nolock``` in SQL?
using explicit table hints frequently is considered as a bad practice that you should generally avoid. For the NOLOCK table hint specifically, reading uncommitted data that could be rolled back after you have read it can lead to a **Dirty read** which can occur when reading the data that is being modified or deleted during the uncommitted data read.

#### What is the use of ```@@SPID```?
A ```@@SPID``` returns the session ID of the current user process.

#### What is the difference between Clustered and Non-Clustered Indexes in SQL Server?
Indexes are used to speed up the query process in SQL Server, resulting in high performance. Without indexes, a database has to go through all the records in the table in order to retrieve the desired results. **This process is called table-scanning and is extremely slow**. On the other hand, if you create indexes, the database goes to that index first and then retrieves the corresponding table records directly.<br>
To see all the indexes on a particular table execute ```EXECUTE sp_helpindex [Table name]``` or viewing directly by going to *Object Explorer -> Databases -> Database Name -> Tables -> Table Name -> Indexes* <br>
- There can be only one clustered index per table. However, you can create multiple non-clustered indexes on a single table.
- Clustered indexes only sort tables. Therefore, they do not consume extra storage. Non-clustered indexes are stored in a separate place from the actual table claiming more storage space.
- Clustered indexes are faster than non-clustered indexes since they don't involve any extra lookup step.
######
**Clustered Index**<br>
A clustered index defines the order in which data is physically stored in a table. Table data can be sorted in only one way, therefore, there can be **only one clustered index per table (it may use more than one column which we call "composite index")**. In SQL Server, the primary key constraint automatically creates a clustered index on that particular column. 
```
CREATE TABLE Test
(
    Id int PRIMARY KEY, -- by default it will be created as a clustered index
    Name varchar(50) NOT NULL,
    Gender varchar(10) NOT NULL,
    Score int NOT NULL
)
```
The clustered index stores the records in the table following the ascending order of the ```Id```. Therefore, if the inserted record has the ```Id``` of 5, the record will be inserted in the 5th row of the table instead of the first row. Similarly, if the fourth record has an ```Id``` of 3, it will be inserted in the third row instead of the fourth row. This is because the clustered index has to maintain the physical order of the stored records according to the indexed column.<br>
You can create your own custom index as well the default clustered index. To create a new clustered index on a table you first have to delete the previous index. See below the script to create a new clustered index, using a **composite index** in this case:<br>
```
-- CREATE CLUSTERED INDEX IX_Test_Gender ON Test(Gender ASC) -- basic clustered index
CREATE CLUSTERED INDEX IX_Test_Gender_Score ON Test(Gender ASC, Score DESC) -- composite index
```
The above index first sorts all the records in the ascending order of the gender. If gender is the same for two or more records, the records are sorted in the descending order of the values in their ```Score``` column.<br>
In order to create a clustered index, you have to use the keyword ```CLUSTERED``` before ```INDEX```.<br>
An index that is created on more than one column is called "composite index".
######
**Non-Clustered Index**<br>
The syntax for creating a non-clustered index is similar to a clustered index. However, in case of a non-clustered index keyword ```NONCLUSTERED``` is used instead of ```CLUSTERED```. Take a look at the following script:
```
CREATE NONCLUSTERED INDEX IX_Test_Name ON Test(Name ASC)
```
The above script creates a non-clustered index on the ```Name``` column of the Test table. The index sorts by name in ascending order. The table data and index will be stored in different places. The table records will be sorted by a clustered index if there is one then the non-clustered index will be sorted according to its definition and will be stored separately from the table.<br>
The non-clustered index is a copy of the data(of the column reference) which stores both values and a pointer to actual row that holds the data.<br>
**IndexNewData(Name, Row Address (Pointer))** it means that every row has a column that stores the address of the row to which the name belongs what we call pointer. So if a query is issued to retrieve the gender and score of the test named "xxxx", the database will first search the name "xxxx" inside the index. It will then read the row address of "xxxx" and will go directly to that row in the test table to fetch gender and score of "xxxx".<br>


#### What is filtered Index?
Filtered Index is used to filter some portion of rows in a table to improve query performance, index maintenance and reduces index storage costs. When the index is created with ```WHERE``` clause, then it is called filtered Index.

#### What is constraints in SQL?
SQL constraint is used to specify rules for the data in a table. Constraints are used to limit the type of data that can go into a table. Constraints can be column level or table level

#### What are Noncorrelated and Correlated Subqueries?
Subqueries can be categorized into two types:<br>
- A noncorrelated (simple) subquery obtains its results independently of its containing (outer) statement.
```
	SELECT name, street, city, state FROM addresses WHERE state IN (SELECT state FROM states)
	SELECT COUNT(*) FROM SubQ1 GROUP BY SubQ1.a HAVING SubQ1.a = (SubQ1.a & (SELECT y from SubQ2)
```
- A correlated subquery requires values from its outer query in order to execute.<br>
```
	SELECT name, street, city, state FROM addresses
		WHERE EXISTS (SELECT * FROM states WHERE states.state = addresses.state)
```

#### What is a user-defined function in SQL?
SQL Server user-defined functions are routines that accept parameters, perform an action, such as a complex calculation, and return the result of that action as a value. The return value can either be a single scalar value or a result set.

#### What are aggregate and scalar function?
Aggregate functions operate against a collection of values and return a single summarizing value. Scalar functions return a single value based on scalar input arguments. Some scalar functions, such as CURRENT_TIME, do not require any arguments.

#### What operator is used to pattern matching?
LIKE

#### How to identify slow queries on SQL Server database?
Approaches to identify slow queries:
- Query Execution Times: Monitor the execution times of queries using the **SQL Server Profiler or Extended Events**. Capture the duration of each query execution and identify queries that take a long time to complete.
- SQL Server Dynamic Management Views (DMVs): Utilize DMVs like `sys.dm_exec_query_stats` and `sys.dm_exec_requests` to gather information about query execution and resource usage. These views provide details about query duration, CPU usage, and I/O statistics, allowing you to identify slow-running queries.
- SQL Server Execution Plans: Analyze the execution plans of queries using SQL Server Management Studio (SSMS). Execution plans provide insights into how the database engine executes a query and can help identify inefficient operations, missing indexes, or other optimization opportunities.
- SQL Server Profiler: Use SQL Server Profiler to capture a trace of the database activity. You can filter and analyze the captured data to identify queries with longer durations or high resource consumption.
- Query Store: Enable the Query Store feature in SQL Server to automatically capture query performance information. Query Store tracks query execution statistics, execution plans, and query regressions over time. It provides reports and insights to help you identify and troubleshoot slow queries.
- Performance Monitoring Tools: Utilize performance monitoring tools like SQL Server Performance Monitor or third-party monitoring solutions like Datadog
- Indexing and Tuning Advisor: Use the SQL Server Indexing and Tuning Advisor to analyze query workloads and suggest index optimizations. It can provide recommendations to improve query performance based on the captured workload.

Once the slow queries are identified, you can further optimize their performance by considering techniques such as indexing, query optimization, rewriting queries, or redesigning database schema if needed. It's important to carefully analyze the execution plans, identify potential bottlenecks, and apply appropriate optimizations.

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
- ```clearInterval(id)```, function instructs the timer to stop.<br>
######
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
The ```pop()``` method is similar as the ```shift()``` method but the difference is that the Shift method works at the start of the ```array```. Also the ```pop()``` method take the last element off of the given array and returns it. The array on which is called is then altered.
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

#### SASS vs CSS
Sass is a meta-language on top of CSS that's used to describe the style of a document cleanly and structurally, with more power than flat CSS allows. Sass both provides a simpler, more elegant syntax for CSS and implements various features that are useful for creating manageable stylesheets. More specifically, Sass is an extension of CSS3.

<hr>

### Front-end TypeScript

#### What is TypeScript, and what are its advantages over JavaScript?
TypeScript is a superset of JavaScript that adds static typing and additional features to JavaScript. Its advantages include:
- Improved code maintainability.
- Enhanced tooling and autocompletion.
- Early error detection.
- Better code organization with modules, and improved collaboration in large codebases.

#### What are the basic types in TypeScript?
TypeScript provides several basic types, including `number`, `string`, `boolean`, `object`, `array`, `tuple`, `enum`, `any`, `void`, `null`, and `undefined`.

#### What are interfaces in TypeScript?
Interfaces in TypeScript define a contract that an object should conform to. They describe the shape of an object by specifying the property names, types, and optionally, methods that the object should have. Interfaces enable type checking and help ensure that objects adhere to a certain structure.

#### What is the difference between interface and type in TypeScript?
Both interface and type can be used to define object shapes, but there are subtle differences. interface is often used for object shape definitions and can be extended or implemented. type can define not only object shapes but also union types, intersection types, and other advanced types.

#### What are generics in TypeScript, and why are they useful?
Generics in TypeScript allow the creation of reusable components that can work with multiple types. They enable the creation of flexible and type-safe functions, classes, and interfaces that can handle different data types without sacrificing type safety.

#### How does TypeScript support inheritance and polymorphism?
TypeScript supports class-based inheritance where a derived class can extend a base class. It also supports method overriding and polymorphism, allowing a derived class to override methods defined in the base class and provide its own implementation.

#### What are access modifiers in TypeScript, and what do they mean?
Access modifiers (`public`, `private`, and `protected`) define the visibility and accessibility of class members (properties and methods):
- `public`: Means the member is accessible from anywhere
- `private`: Restricts access to within the class
- `protected`: Allows access within the class and its derived classes.

#### How do you handle asynchronous operations in TypeScript?
TypeScript provides built-in support for promises, async/await, and the async modifier. But in Angular it is possible to use RxJS library which allow to use Observables.

#### How do you compile TypeScript code into JavaScript?
TypeScript code is transpiled into JavaScript using the TypeScript compiler (tsc). The TypeScript compiler reads .ts files, checks types, and produces corresponding .js files. The compiled JavaScript files can then be executed by any JavaScript runtime environment.

#### How can you configure TypeScript in a project?
Using a tsconfig.json file, which specifies compiler options, file inclusion/exclusion patterns, module resolution settings, and more.

<hr>

### Front-end Angular

#### How to communication between components angular?
- Parent-to-Child Communication (Input Binding): Use input properties to pass data from a parent component to a child component. In the parent component's template, bind a property to the child component's input property using square brackets:
```
<!-- html file -->
<!-- Parent Component -->
<app-child [data]="parentData"></app-child>
```
```
// typescript file
// Parent Component
export class ParentComponent {
  parentData: string = "Hello from parent";
}

// Child Component
export class ChildComponent {
  @Input() data: string;
}

//The child component can access the data passed from the parent component through the @Input decorator.
```
- Child-to-Parent Communication (Event Emitter): Use output properties and event emitters to emit events from a child component to its parent component. In the child component, define an output property with the @Output decorator and an instance of the EventEmitter class. When an event occurs in the child component, emit the event using the emit() method:
```
<!-- html file -->
<!-- Child Component -->
<button (click)="sendMessage()">Send Message</button>
```
```
// Child Component
import { Output, EventEmitter } from '@angular/core';

export class ChildComponent {
  @Output() messageSent = new EventEmitter<string>();

  sendMessage() {
    this.messageSent.emit("Hello from child");
  }
}

// Parent Component
export class ParentComponent {
  receiveMessage(message: string) {
    console.log(message);
  }
}

// In the parent component's template, bind the output property to a method that handles the emitted event using parentheses
```
- Service: Use a shared service as a communication medium between components. Create a service with methods and properties that can be accessed by components. Components can inject the service and use its methods to share data or trigger actions. The shared service can use subjects, observables, or event emitters to facilitate communication between components.
- ViewChild/ViewChildren: Use the `@ViewChild` and `@ViewChildren` decorators to get a reference to a child component or a collection of child components, respectively. This allows the parent component to directly access properties or invoke methods of the child component(s).

#### What is a Directive?
At the core, a directive is a function that executes whenever the Angular compiler finds it in the DOM. Angular directives are used to extend the power of the HTML by giving it new syntax. Each directive has a name, either one from the Angular predefined like ```ng-repeat```, or a custom one which you can name as you prefer. There are 3 types of directives:
- **Component Directives** These form the main ```class``` having details of how the component should be processed, instantiated and used at runtime.
- **Structural Directives** basically deals with manipulating the DOM elements. Structural directives have a * sign before the directive. For example, *ngIf and *ngFor
- **Attribute Directives** they deal with changing the look and behaviour of the dom element. You can create your own directives. For example, `ngClass` and `ngStyle`

#### What are Lifecycle hooks in Angular? Explain some of them
Angular components enter its lifecycle from the time it is created to the time it is destroyed. Angular hooks provide ways to tap into these phases and trigger changes at specific phases in a lifecycle.
- ```ngOnChanges()```: Respond when Angular sets or resets data-bound input properties. The method receives a SimpleChanges object of **current and previous property values**. Called before ```ngOnInit()``` and whenever one or more data-bound input properties change. If your component has no inputs or you use it without providing any inputs it won't be called.
- ```ngOnInit()```: Initialize the directive or component after Angular first displays the data-bound properties and sets the directive or component's input properties as well as initialization tasks like retrieving data from a service, setting up subscriptions, or initializing component-specific variables. Called once, after the first ```ngOnChanges()```. 
- ```ngDoCheck()```: Detect and act upon changes that Angular can't or won't detect on its own. Called immediately after ```ngOnChanges()``` on every change detection run, and immediately after ```ngOnInit()``` on the first run.
- ```ngAfterContentInit()```: Respond after Angular projects external content into the component's view, or into the view that a directive is in. Called once after the first ```ngDoCheck()```.
- ```ngAfterContentChecked()```: Respond after Angular checks the content projected into the directive or component. Called after ```ngAfterContentInit()``` and every subsequent ```ngDoCheck()```.
- ```ngAfterViewInit()```: Respond after Angular initializes the component's views and child views, or the view that contains the directive. Called once after the first ```ngAfterContentChecked()```.
- ```ngAfterViewChecked()```: Respond after Angular checks the component's views and child views, or the view that contains the directive. Called after the ```ngAfterViewInit()``` and every subsequent ```ngAfterContentChecked()```.
- ```ngOnDestroy()```: Cleanup just before Angular destroys the directive or component. Unsubscribe Observables and detach event handlers to avoid memory leaks. Called immediately before Angular destroys the directive or component.

#### Explain Dependency Injection in Angular?
Dependency injection is an application design pattern that is implemented by Angular and forms the core concepts of Angular. <br>
Dependencies in Angular are services which have a functionality. Various components and directives in an application can need these functionalities of the service. Angular provides a smooth mechanism by which these dependencies are injected into components and directives.
```
constructor(private userService: UserService) { }
```

#### How to make Api calls in Angular?
Using HttpClient request which is service available as an injectable class.

```
// Standard
import { HttpClient } from '@angular/common/http';
constructor(private httpClient: HttpClient) { }

// Make an HTTP GET request:
// Similarly, you can use other HTTP methods like post(), put(), delete()
this.httpClient.get('https://api.example.com/data').subscribe((response) => {
  // Handle the response data here
}, (error) => {
  // Handle any errors here
});

// Handle the response
// Inside the subscribe() method, you can handle the response data returned by the API

// Other examples:
return this.httpClient.get<ApiResponse>(url, { params: httpParams, withCredentials: true });
return this.httpClient.post(url, new type().post(object, type), { withCredentials: true });
return this.httpClient.delete(url, { withCredentials: true });

get<T extends IApiModel>(apiRequest: ApiRequest, type: new () => T): Observable<T> {
	return this.apiRequest
	.pipe(
		map((response: ApiResourceResponse) => {
			if (response.errors != null) {
				console.log(response.errors);
				return new type();
			}
			if (response.resource !== null) {
				return new type().parse(response.resource, type);
			} else {
				return new type().parse(response, type);
			}
		}),
		catchError((error: any) => {
			return of(new type());
		})
	);
}
  
getList<T extends IApiModel>(): Observable<T[]> { return new type().parseArray(response.resource, type); }

save<T extends IApiModel>(apiRequest: ApiRequest, object: T, type: new () => T, message?: string): Observable<number> {
	return this.erpSaveService.apiRequest.pipe(
		filter((x) => x),
		take(1),
		switchMap(() => {
			return this.httpClient.post(url, new type().post(object, type), { withCredentials: true });
		}),
		map((response: ApiResponse) => {
			if (response.errors != null) {
				console.log(response.errors);
				return null;
			} 
			if (message.lenght > 0) { console.log(message); }
			return +response.resource;
		}),
		catchError((error: any) => { return; }),
		finalize(() => { console.log('test'); })
    );
  }
}

//request with setStream

obs$: Observable<MyModel[]>;

constructor(private readonly baseService: BaseService) {
	this.setStream();
}

private mySubject = new Subject();
private mySubject = new Subject<MyModel[]>();

request() {
	myRequest.next();
}

setStream()
{
	this.obs$ = myRequest
		.pipe(
			takeUntil(unsubscribe) or take(1),
			debounceTime(400),
			//switchMap(x => {
			//  return this.get<xModel>(x);
			//})
		)
		.subscribe(result => { this.mySubject.next(result)}); 
	});
}

//calling it 
requestServiceInjected.request();
//or 
combineLatest([requestService.obs$, n1$, n2$]
	.pipe(takeUntil(this.unsubscribe))
	.subscribe(data => {
		const x1 = data[0];
		const x2 = data[1];
		const x3 = data[2];
	});
```

#### Describe the MVVM architecture?
MVVM architecture removes tight coupling between each component. The MVVM architecture comprises of three parts:
- **Model**: It represents the data and the business logic of an application, or we may say it contains the structure of an entity. It consists of the business logic - local and remote data source, model classes, repository.
- **View**: View is a visual layer of the application, and so consists of the UI Code(in Angular-HTML template of a component.). It sends the user action to the ViewModel but does not get the response back directly. It has to subscribe to the observables which ViewModel exposes to it to get the response. 
- **ViewModel**: It is an abstract layer of the application and acts as a bridge between the View and Model(business logic). It does not have any clue which View has to use it as it does not have a direct reference to the View. View and ViewModel are connected with data-binding so, any change in the View the ViewModel takes note and changes the data inside the Model. It interacts with the Model and exposes the observable that can be observed by the View.

#### How to navigating between different routes in an Angular app?
- Define routes: using the `RouterModule.forRoot()` method in the app's root module (app.module.ts). Configure the routes and associate them with the corresponding components.
- Perform navigation: To navigate to a specific route, you can use the `navigate()` method of the Router instance. Provide the route path as a parameter.
- Pass route parameters: using `navigate()` method.
- Programmatic navigation: You can also navigate programmatically in response to events or user actions. For example, you can navigate on a button click event or after a form submission.
- RouterLink directive: It creates links that navigate to different routes.
```
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { AboutComponent } from './about.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

// Component 
import { Router } from '@angular/router';
constructor(private router: Router) { }

// Navigate to the 'about' route
this.router.navigate(['/about']);

// Navigate to the 'product' route with a parameter
this.router.navigate(['/product', productId]);

<!-- Use RouterLink directive for navigation -->
<a routerLink="/">Home</a>
<a routerLink="/about">About</a>
```

Another snippet demonstrate on how to implement that:
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

#### Could you explain services in Angular?
Services act as a central place for storing and managing data, performing business logic, and interacting with external APIs or backend services. They facilitate the separation of concerns and promote reusable and maintainable code.<br>
The primary intent of an Angular service is to organize as well as share business logic, models, or data and functions with various components of an Angular application.<br> 
The functions offered by an Angular service can be invoked from any Angular component, such as a controller or directive.
- Singleton Instances: Services are typically created as singleton instances.
- Injectable Decorator: To use a service, you need to inject it into the components or other services that depend on it. By applying the `@Injectable()` decorator to a service class and registering it in the Angular dependency injection system.
- Business Logic: Services are responsible for encapsulating business logic, such as data manipulation, API calls, state management, and other operations.
- Reusability: Services promote code reusability by allowing you to centralize and share common functionality across multiple components.
- Dependency Injection: Services can depend on other services or Angular built-in services, which enables you to compose complex functionality by injecting the required dependencies.
- Communication Between Components: Services can act as intermediaries for communication between components.

#### How to generate a Service in Angular using CLI?
```ng generate service MyServiceName --module=app```

#### How to generate a ```class``` in Angular using CLI?
```ng generate class MyClassName [options]```

#### How to generate a component in Angular using CLI?
```ng generate component component-name```

#### What is string interpolation in Angular?
Also referred to as moustache syntax, string interpolation in Angular refers to a special type of syntax that makes use of template expressions in order to display the component data. These template expressions are enclosed within double curly braces i.e. ```{{ }}```

#### Can you explain the concept of scope hierarchy in Angular?
The scope hierarchy in Angular is established based on the component's structure and the relationships defined through component nesting or routing. Each component has its own scope that encapsulates its data and functionality. The scope hierarchy allows components to communicate and share data with their parent or child components.

#### How do Observables differ from Promises?
Observables and Promises are both used in JavaScript to handle asynchronous operations and manage the flow of data. However, there are some key differences between them:
- **Promise**
	- Handles a single event when an async operation completes or fails.
	- Single Value: Promises can only resolve once and provide a single value.
	- Eager Execution: promises are eager and start executing immediately when created.
	- Cancelation: Promises, once initiated, cannot be canceled.
	- Native Browser Support: Promises are native to JavaScript and have good browser support.
- **Observable** 
	- More flexible allows to pass zero or more events where the callback is called for each event.
	- Multiple Values: Observables can emit multiple values over time and it is capable of representing streams of data, enabling continuous updates and real-time data handling.
	- Lazy: Observables are lazy by nature, meaning they don't execute unless explicitly subscribed to. They are activated and start emitting values only when a subscription is made.
	- Cancelation: Observables support cancelation by unsubscribing from the stream.
	- Time Independence: Observables have the ability to handle time-related operations through operators like `debounceTime`, `throttleTime`, and `interval`.
	- Stream Transformation: Observables provide a rich set of operators to `transform`, `filter`, `merge`, and `combine` streams of data. These operators allow powerful data manipulation and composition.
	- Native Browser Support: bservables, specifically RxJS Observables used in Angular, require an additional library (RxJS).

#### What are Route Guards in Angular?
Route guards are interfaces provided by Angular which, when implemented, allow us to control the accessibility of a route based on conditions provided in class implementation of that interface. It tells the router whether or not it should allow navigation to a requested route. They make this decision by looking for a boolean return value from a class which implements the given guard interface. The guards are: 
- `CanActivate`
- `CanActivateChild`
- `CanDeactivate`
- `CanLoad`
- `Resolve`

#### Could you explain the concept of templates in Angular?
Written with HTML, templates in Angular contain Angular-specific attributes and elements. Combined with information coming from the controller and model, templates are then further rendered to cater the user with the dynamic view.

#### Explain the difference between an Annotation and a Decorator in Angular?
- **Annotations** are used for creating an annotation array. They are only metadata set of the ```class``` using the Reflect Metadata library. Examples of annotations in Angular include `@Component`, `@Directive`, `@Injectable`, `@Input`, `@Output`
- **Decorators** are design patterns used for separating decoration or modification of some ```class``` without changing the original source code. Decorators in Angular are typically defined using the `@DecoratorName` syntax. Decorators can be used for various purposes, such as logging, performance monitoring, caching, access control, and more.

#### What are the building blocks of Angular?
Angular is built upon several core building blocks that work together to create robust and scalable applications. The main building blocks of Angular are:
- **Modules**: Modules are the fundamental building blocks of Angular applications. They organize the application into cohesive units by grouping related components, directives, pipes, and services together. Modules help with code organization, encapsulation, and dependency management.
- **Components**: Components are the basic UI building blocks in Angular. They represent parts of the user interface and encapsulate their own logic, template, and styles. Components define the structure and behavior of a specific part of the application, making it reusable and modular.
- **Templates**: Templates define the visual part of a component. They use HTML syntax and can include Angular-specific syntax and directives to render dynamic content. Templates are responsible for displaying data, handling user interactions, and providing the visual representation of the component.
- **Directives**: Directives extend the functionality of HTML elements or components. There are three types of directives in Angular: structural directives, attribute directives, and components. Structural directives, such as `ngIf` and `ngFor`, change the structure of the DOM. Attribute directives, such as `ngClass` and `ngStyle`, modify the behavior or appearance of an element.
- **Services**: Services are responsible for providing shared functionality and data across multiple components. They encapsulate business logic, data access, and other common operations. Services are typically injected into components or other services using Angular's dependency injection mechanism.
- **Dependency Injection**: Dependency Injection (DI) is a key feature of Angular that allows for loose coupling and modular design. It enables the injection of dependencies into components and services, rather than creating them directly. DI promotes reusability, testability, and maintainability by facilitating the management of dependencies and their configuration.
- **Routing**: Angular's routing module allows for navigation and routing within the application. It enables the creation of multiple views, each associated with a specific route or URL. Routing allows users to navigate between different components and views within the application without a full page reload.
- **Forms**: Angular provides powerful features for working with forms, including template-driven forms and reactive forms. Forms allow for user input validation, data binding, and handling form submissions. Angular provides a robust set of form controls, validators, and form-related directive.
- **Data Binding**: The mechanism by which parts of a template coordinates with parts of a component is known as data binding. In order to let Angular know how to connect both sides (template and its component), the binding markup is added to the template HTML.

#### What is Angular Material?
It is a UI component library. Angular Material helps in creating attractive, consistent, and fully functional web pages as well as web applications. It does so while following modern web design principles, including browser portability and graceful degradation.

#### What is Data Binding? How many ways it can be done?
Data binding in Angular is a mechanism that establishes a connection between the application's data and the user interface. It allows you to synchronize and update data values between the model (data) and the view (UI) automatically. Data binding reduces the need for manual DOM manipulation and simplifies the development process.<br>
Angular provides three categories of data binding according to the direction of data flow:
- From source to view:
	- Interpolation Binding (One-way Binding):
		- Denoted by double curly braces `{{}}`
		- Syntax: `{{expression}}`
		- Example: `<h1>{{pageTitle}}</h1>`
		- It allows you to bind a component's property or expression directly into the HTML template. The property value is dynamically evaluated and inserted into the template.
	- Property Binding (One-way Binding): 
		- Denoted by square brackets `[]`
		- Syntax: `[target]="expression"`
		- Example: `<input [value]="name">`
		- It allows you to bind a property of an HTML element to a property of a component. It binds the component's property to the HTML element's property, and any changes in the component's property will reflect in the HTML element.
- From view to source 
	- Event Binding (One-way Binding):
		- Denoted by parentheses `()`
		- Syntax: `(target)="statement"`
		- Example: `<button (click)="onButtonClick()">Click Me</button>`
		- Event binding allows you to bind an event of an HTML element to a method in the component. It allows you to respond to user interactions such as button clicks, mouse movements, and keyboard events. When the specified event occurs, the corresponding method in the component is executed.
- In a two-way sequence of view to source to view:
	- Two-way Binding:
		- Denoted by parentheses `[()]`
		- Syntax: `[(target)]="expression"`
		- Example: `<input [(ngModel)]="name">`
		- Two-way binding allows for synchronization of data in both directions, from the component to the template and from the template to the component.

#### What is ```ngOnInit()```? How to define it?
```ngOnInit()``` is a lifecycle hook that is called after Angular has finished initializing all data-bound properties of a directive.

#### What is RxJS? 
RxJS is a library for composing asynchronous and event-based programs by using observable sequences. It provides one core type, the Observable, satellite types (Observer, Schedulers, Subjects) and operators inspired by Array#extras (map, filter, reduce, every, etc) to allow handling asynchronous events as collections.<br>
Observable: represents the idea of an invokable collection of future values or events.<br>
Observer: is a collection of callbacks that knows how to listen to values delivered by the Observable.

#### What is Lazy-loading in Angular?
The lazy load is implemented in the routes using # in the route of the special keyword loadChildren
```
{
	path: 'xxxx'
	loadChildren: 'app/xxxx/xxxx.module#child'
}
```

#### What is View Encapsulation?
In Angular, component CSS styles are encapsulated into the component's view and don't affect the rest of the application. To control how this encapsulation happens on a per component basis, you can set the view encapsulation mode in the component metadata.<br>
To set the component's encapsulation mode, use the encapsulation property in the component metadata encapsulation as `ShadowDom`, `Emulated` or `None`
```
import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-example',
  templateUrl: 'example.component.html',
  styleUrls: ['example.component.css'],
  encapsulation: ViewEncapsulation.Emulated
})
export class ExampleComponent {
  // Component logic here
}
```
- ```Emulated``` (Default): In this mode, also known as "Shadow DOM," Angular emulates the shadow DOM behavior by appending a unique attribute to the HTML elements of the component's template. The styles defined in the component's CSS or SCSS file are scoped and applied only to the specific component and its children. It isolates the styles within the component, preventing them from leaking out and affecting other components.
- ```ShadowDom```: The "ShadowDom" mode uses the native Shadow DOM provided by the browser. It creates a shadow DOM subtree for the component and encapsulates the styles within that subtree. The styles defined in the component's CSS or SCSS file are scoped to the shadow DOM and don't affect the outer DOM or other components.
- ```None```: When using the "None" mode, the styles defined in the component's CSS or SCSS file are not encapsulated. They become global styles and can affect the entire application. This mode should be used with caution as it may lead to style conflicts and unintended side effects when multiple components define conflicting styles.

#### What the keyword `export` represents in Angular?
In Angular, the export keyword is used to make components, directives, services, or other entities available for use outside of their defining module. When a component or other entity is exported, it can be imported and used in other parts of the application.

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
                    <i class="fa fa-arrow-circle-o-right"></i> Details
	    	</a>
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
import { AddClientComponent } from './components/add-client/add-client.component';
import { EditClientComponent } from './components/edit-client/edit-client.component';
import { ClientDetailsComponent } from './components/client-details/client-details.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

const routes: Routes = [
    { path: '', component: DashboardComponent, canActivate:[AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'client/add', component: AddClientComponent, canActivate:[AuthGuard] },
    { path: 'client/edit/:id', component: EditClientComponent, canActivate:[AuthGuard] },
    { path: 'client/:id', component: ClientDetailsComponent, canActivate:[AuthGuard] },
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
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AppRoutingModule } from './/app-routing.module';
import { ClientService } from './services/client.service';
import { AuthService } from './services/auth.service';
import { SettingsService } from './services/settings.service';

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        ClientDetailsComponent,
        LoginComponent,
        NotFoundComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        FlashMessagesModule.forRoot(),
        AppRoutingModule
    ],
    providers: [ClientService, AuthService, SettingsService],
    bootstrap: [AppComponent]
})
export class AppModule { }
```

<hr>

### Unit Test and Integration Test 

#### What is sut? 
System under test.

#### Unit test vs integration test
**Unit tests** focus on a single part of a whole application in total isolation, usually, a single class or function. Ideally, the tested component is free of side effects so it is as easy to isolate by mocking it and test.<br>
**Integration test** there is no need to mock away parts of the application the main thing is to test how parts of the application work together as a whole so it considers side effects from the beginning it helps find issues that are not obvious by examining the application or a specific unit implementation.<br>
Note: Unit test you mock everything else on the other hand in Integration test you don't mock anything you set up the data only. 

#### Test structure
**Given = Arrange**: inputs and targets. Arrange steps should set up the test case like objects, special settings, database. Handle all of these operations at the start of the test.<br>
**When = Act**: on the target behavior. Act steps should cover the main thing to be tested. This could be calling a function or method, calling a REST API, or interacting with a web page. Keep actions focused on the target behavior.<br>
**Then = Assert**: expected outcomes. Act steps should elicit some sort of response. Assert steps verify the goodness or badness of that response.<br>
**Note:** BDD is follows Arrange-Act-Assert pattern by another name Given-When-Then.

#### NUnit vs XUnit vs MSTest
**NUnit:<br>**
- [SetUp] = There should be at least one method per test class. Marks a method that should be called before each test method.
- [TearDown] = 	There should be at least one method per test class. Marks a method that should be called after each test method.
- [TestFixture] = Marks a class that contains tests.
- [Test] = Marks a method, i.e., an actual test case in the test class.
- [TestCase] = Marks a method with parameters and provides the inline arguments.
- [TestFixtureSetUp] = Marks a method that is executed once before the execution of any test method in that fixture.
- [Category] = Specify the category for the test.<br>

**XUnit:<br>**
- [Fact] = Marks a test method, i.e., actual test in a class
- Assert.Throws Record Exception = Verify the raise and raise assert, irrespective of the place in the code where the problem occurs.
- Constructor = This is not an attribute but is an ideal replacement for the [SetUp] attribute. The constructor should be parameter-less.
- IDisposable.Dispose = This is not an attribute but is an ideal replacement for the [TearDown] attribute. This is where the code for performing necessary cleanup and de-initialization is included.
- [Trait] = Used to set arbitrary meta-data on a test.
- [Theory] = This attribute is used when data-driven tests have to be executed. In such cases, [Theory] has to be used instead of [Fact] attribute.
- [InlineData] = This attribute is used along with the [Theory] attribute to supply a subset of data against which parameterized tests will be executed.
- [ClassData] = This attribute is used when the parameters being passed to the [Theory] tests are not constants. `[Theory] [ClassData(typeof(some-data))]`
- [MemberData] = This attribute can be used to fetch data for [Theory] from a static method. The most common approach is to load the data from the property of a test class, i.e., using IEnumerable.<br>

**MSTest:<br>**
- [TestInitialize] = Marks a method that should be called before each test method. One such method should be present before each test class.
- [TestCleanup] = Marks a method that should be called after each test method. One such method should be present before each test class.
- [TestClass] = Marks a class that contains tests.
- [TestMethod] = Marks the method, i.e., the actual test case in the test class.
- [DataRow] = Allows setting the values of the parameters of the tests. Multiple [DataRow] annotations can be present in the code.
- [DataTestMethod] = It has the same functionality as the [TestMethod] attribute except that it is used when the [DataRow] attribute is used.
- [AssemblyInitialize] = Marks the method that should be called once before the execution of any method in the assembly code.
- [AssemblyCleanup] = Marks the method that should be called once after the execution of any method in the assembly code.
- [Ignore] = Marks a test method or test class that should be considered for execution, i.e., it is ignored.
- [TestCategory] = Specify the category for the test.
- [ClassInitialize] = Methods that will be called only once before executing any of the test methods present in that class.
- [ClassCleanup] = Methods that will be called only once after executing the test methods present in that class.

<hr>

### Architecture

#### Explain hexagonal architecture?
Also known as Ports and Adapters architecture or the Onion architecture, is a software architectural pattern that promotes a modular and flexible design for building applications. It emphasizes separation of concerns and isolates the core business logic from external dependencies.<br>
The key concept in hexagonal architecture is the notion of the application's core or the domain layer. This layer represents the heart of the application and encapsulates the business rules and domain-specific logic. It is surrounded by layers of adapters that handle the interaction with the external systems, such as the user interface, databases, external services, or third-party libraries. The main components in hexagonal architecture:
- Core/Domain: This layer contains the application's core logic and business rules. It represents the domain-specific concepts and entities. The core does not depend on any external systems or frameworks and should be decoupled from implementation details.
- Ports: Ports are interfaces or contracts that define the interaction points between the core and the external world. They represent the services or operations that the application provides or requires. Ports define the boundaries of the application and ensure loose coupling with external dependencies.
- Adapters: Adapters implement the ports and provide the necessary infrastructure to connect the application to external systems. There are two types of adapters:
	- Inbound Adapters: Also known as primary adapters, these handle the incoming requests from external systems, such as the user interface or API endpoints. They translate the external input into a format that the core can understand and invoke the corresponding operations.
	- Outbound Adapters: Also known as secondary adapters, these handle the communication with external systems, such as databases, third-party APIs, or message queues. They abstract the details of the external systems and provide a simplified interface for the core to interact with.

#### What is REST?
Representational state transfer (REST) is a software architectural style that was created to guide the design and development of the architecture for the World Wide Web. REST defines a set of constraints for how the architecture of an Internet-scale distributed hypermedia system, such as the Web, should behave. The REST architectural style emphasises the scalability of interactions between components, uniform interfaces, independent deployment of components. It provides operations (HTTP methods) such as GET, POST, PUT, and DELETE.

#### What is Bounded Context in DDD?
It is the delimited applicability of a particular model. Gives team members a clear and shared understanding of what has to be consistent and what can be developed independently. Bounded Context is a central pattern in Domain-Driven Design when dealing with large models and teams, it is used in order to divide the models of underlying domain implementing **ubiquitous language** to help the communication between devs domain experts.

#### What is Ubiquitous Language in DDD?
The ubiquitous language is a design approach, which consists notably of striving to use the vocabulary of a business domain in the all way into the product source code, which can achieve the practice of building up a common, rigorous communication between developers and users (domain experts).

#### What is Value Object in DDD?
Value object is an immutable type that is distinguished only by the state of its properties. In C# the type must have all of its state passed in at construction, any property must be read-only, which can be achieved using private setters ```public int n { get; private set; }```. **Value objects cannot be changed once they are created**. 

#### What is the difference between Layers and Tiers?
A layer is a part of your code (logical), if your application is a cake, this is a slice. <br>
A tier is a machine, a server (physical). A tier hosts one or more layers. <br>
- **Layers (logical)** are merely a way of organizing your code. Typical layers include Presentation, Business (Business Logic Layer, BLL) and Data (Database Access Layer DAL). The same as the traditional 3-tier model. But when we're talking about layers, we're only talking about logical organization of code. In no way is it implied that these layers might run on different computers or in different processes on a single computer or even in a single process on a single computer. All we are doing is discussing a way of organizing a code into a set of layers defined by specific function.
- **Tiers(physical)** are only about where the code runs. Specifically, tiers are places where layers are deployed and where layers run. In other words, tiers are the physical deployment of layers.

#### What is dead letter queue?
Dead letter queue is a service implementation to store messages that meet one or more of the following criteria:
- Message that is sent to a queue that does not exist.
- Queue length limit exceeded.
- Message length limit exceeded.
- Message is rejected by another queue exchange.
- Message reaches a threshold read counter number, because it is not consumed. Sometimes this is called a "back out queue".

#### What is the difference between Durable and Non-Durable queues?
- Durable queues keep the message around persistently for any suitable customer to consume them.
- Non-Durable queues handle just if the subscribe is online at the moment the message is sent/published.

#### Architectural principle: Separation of concerns
A guiding principle when developing is Separation of Concerns. This principle asserts that software should be separated based on the kinds of work it performs. For instance, consider an application that includes logic for identifying noteworthy items to display to the user, and which formats such items in a particular way to make them more noticeable. The behaviour responsible for choosing which items to format should be kept separate from the behaviour responsible for formatting the items, since these behaviours are separate concerns that are only coincidentally related to one another.<br>
Architecturally, applications can be logically built to follow this principle by separating core business behaviour from infrastructure and user-interface logic. Ideally, business rules and logic should reside in a separate project, which should not depend on other projects in the application. This separation helps ensure that the business model is easy to test and can evolve without being tightly coupled to low-level implementation details. Separation of concerns is a key consideration behind the use of layers in application architectures.

#### Architectural principle: Don't repeat yourself (DRY)
The application should avoid specifying behaviour related to a particular concept in multiple places as this practice is a frequent source of errors. At some point, a change in requirements will require changing this behaviour. It's likely that at least one instance of the behaviour will fail to be updated, and the system will behave inconsistently.<br>
Rather than duplicating logic, encapsulate it in a programming construct. Make this construct the single authority over this behaviour, and have any other part of the application that requires this behaviour using the new construct.
