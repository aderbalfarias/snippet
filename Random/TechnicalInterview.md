### C# Versus

#### Abstract Class vs Interface
An abstract class allows you to create functionality that subclasses can implement or override and it also can have have constructors. An interface only allows you to define functionality, not implement it (however from C# 8.0 on you can have default methods and you also can change modifiers). And whereas a class can extend only one abstract class, it can take advantage of multiple interfaces.

#### String vs string
Essentially, there is no difference between string and String in C#.
String is a class in the .NET framework in the System namespace. The fully qualified name is System.String. Whereas, the lower case string is an alias of System.String.

#### Action vs Func vs Predicate
Action: Delegate (pointer) to a method that takes zero, one or more input parameters but doesn't return anything.
Func: Delegate (pointer) to a method that takes zero, one or more input parameters and returns a value or reference.
Predicate: A special form of Func and mainly used to validate something and return bool. It is mainly used with collections to whether the item in the collection is valid or not. Basically, its a wrapper of Func like ```Func<T, bool>```.
*When to use that*?
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