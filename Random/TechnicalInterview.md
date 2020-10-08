### C# Versus

#### Abstract Class vs Interface
An abstract class allows you to create functionalities that subclasses can implement or override and it also can have have constructors. An interface only allows you to define functionalities, not implement it (however from C# 8.0 on you can have default methods and you also can change modifiers). And whereas a class can extend only one abstract class, it can take advantage of multiple interfaces.

#### System.String vs System.StringBuilder
System.String: It is immutable, it means when a string object is created you cannot modify and you have always to create a new object string type in memory.<br>  
```
    string x = "hi";
    x += "how are you?"; // it's a new string instance, we can't change the old one 
```
System.StringBuilder: It is mutable, means if you create string builder object then you can perform any operation like insert, replace or append without creating new instance for every time. It will update string at one place in memory doesn't create new space in memory.<br>
```
    StringBuilder strb = new StringBuilder("hi");
    strb.Append("how are you?");
    string x = strb.ToString();
```

#### String vs string
Essentially, there is no difference between string and String in C#.<br>
String is a class in the .NET framework in the System namespace. The fully qualified name is System.String. Whereas, the lower case string is an alias of System.String.

#### Action vs Func vs Predicate
Action: Delegate (pointer) to a method that takes zero, one or more input parameters but doesn't return anything.<br>
Func: Delegate (pointer) to a method that takes zero, one or more input parameters and returns a value or reference.<br>
Predicate: A special form of Func and mainly used to validate something and return bool. It is mainly used with collections to whether the item in the collection is valid or not. Basically, its a wrapper of Func like ```Func<T, bool>```.<br>
*When to use that*?<br>
Action is useful if we don’t want to return any result. But if we want to return result, we could use Func. Predicate is mainly to used to validate any condition.

#### Deferred Execution vs Immediate Execution
Deferred Execution: It simply means that the query is not executed at the time it's specified. Specifically, this is accomplished by assigning the query to a variable. When this is done the query definition is stored in the variable but the query ins't executed until the query variable is interated.
```
    var x = from product in context.Products where product.Type == "y" select product;
    var x = context.Products.Where(w => w.Type == "y");
    foreach(var item x) { Console.WriteLine(item.Name); } // Query executes at x point
```
Immediate Execution: Query is executed at the point of its declaration. It can be useful if the database is being updated frequently in order to ensure the results where returned at the point the database query is specified. It often uses methods such as ```First(), Avarage(), Sum(), Count(), ToList(), ToArray(), ToDictionary()```.   
```
    var x = (from product in context.Products where product.Type == "y" select product).ToList();
    var x = context.Products.Where(w => w.Type == "y").ToList();
```

#### Stack vs Heap
In short, in the **Stack** are stored value types (types inherited from System.ValueType like bool, int, long, decimal, float, short) and in the **Heap** are stored reference types (types inherited from System.Object such as string, object, dynamic).<br>
Stack is responsible for keeping track what is actually executing and where each executing thread is (each thread has its own stack).<br>
Heap is responsible for keeping track of the data, or more precise objects.

#### Class vs Object
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
   
#### Object.ToString() vs Convert.ToString()
```Object.ToString()``` cannot handle ```null``` values which means the *Null reference exception* will be thrown when trying to use ```.ToString()``` on a object which is ```null```, in the other hand ```Convert.ToString()``` can handle ```null``` values it won't generate *Null reference exception*.
