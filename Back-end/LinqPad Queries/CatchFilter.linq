<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\mscorlib.dll</Reference>
</Query>

//C# 6

void Main()
{
	try
    {
		throw new Exception("Fail");
		throw new ArgumentNullException("Fail");
	}	
    catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
    {
        Console.WriteLine("Exception 1");
    }
	catch (Exception ex) when (ex.Message.Contains("Fail"))
    {
        Console.WriteLine("Fail");
    }
}
