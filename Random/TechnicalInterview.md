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
<p>*When to use that*?</p>
<p>Action is useful if we don’t want to return any result. But if we want to return result, we could use Func. Predicate is mainly to used to validate any condition.</p>