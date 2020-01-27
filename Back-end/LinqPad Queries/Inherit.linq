<Query Kind="Program">
  <Namespace>System</Namespace>
</Query>

class Program
{
    static void Main()
    {
        Base a = new Derived();
        a.Foo();
        ((Derived)a).Foo();
    }
}

class Base
{
    public virtual void Foo()
    {
        Console.WriteLine("Base.Foo");
    }
}

class Derived : Base
{
    public new void Foo()
    {
        Console.WriteLine("Derived.Foo");
    }
}

//Output:
//Base.Foo
//Derived.Foo