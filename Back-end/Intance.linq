<Query Kind="Program" />

class Program 
{
    static void Main() 
    {
        A obj = new B();
        obj.Foo();
    }
}

class A 
{
    public void Foo() 
    {
        Console.WriteLine(2);
    }
}

class B : A
{
    public new void Foo() 
    {
        Console.WriteLine(4);
    }
}

//2