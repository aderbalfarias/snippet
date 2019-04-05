<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
</Query>

//C# 3
//public static class ExtensionMethods
//{
//    public static string UppercaseFirstLetter(this string value)
//    {
//        // Uppercase the first letter in the string.
//        if (value.Length > 0)
//        {
//            char[] array = value.ToCharArray();
//            array[0] = char.ToUpper(array[0]);
//            return new string(array);
//        }
//        return value;
//    }
//}
//
//class Program
//{
//    static void Main()
//    {
//        // Use the string extension method on this value.
//        string value = "test";
//        value = value.UppercaseFirstLetter();
//        Console.WriteLine(value);
//    }
//}


static class Extensions
{
    public static int MultiplyBy(this int value, int mulitiplier)
    {
        // Uses a second parameter after the instance parameter.
        return value * mulitiplier;
    }
}

class Program
{
    static void Main()
    {
        // Ten times 2 is 20.
        // Twenty times 2 is 40.
        int result = 10.MultiplyBy(2).MultiplyBy(2);
        Console.WriteLine(result);
    }
}
