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



#### What is Inversion of Control?
Inversion of control is a *Principle in softeware engineering* by which the control of objects or portions of a program is transferred to a container or framework. It is most oftem used in the context of OOP.

### C# Design Patterns

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

#### What garbage collection is and how it works. Provide example of how you can enforce it in .NET?
Garbage collection is a low-priority process that serves as an automatic memory manager which manages the allocation and release of memory for the applications. Each time a new object is created, the CLR allocates memory for that object from the managed **heap**. As long as free memory space is available in the managed heap the runtime continuos to allocate space for new objects. However memory is not infinite and when heap memory is full garbage collection comes to free some memory.<br>
When **Garbage Collector** performs a collection it checks for objects in the managed heap that are no longer being used by the applications and performs the necessary operations to relcaim the memory, it will stop all running threads and find the objects in the heap that aren't being accessed by the main program and delete them, then reorganize all the objects left in the heap in order to make space and adjust all pointers to these objects in the heap and the stack.<br>
It can be implemented by using the ```IDisposable interface```.<br>
```System.GC.Collect() // Force garbage collection```

