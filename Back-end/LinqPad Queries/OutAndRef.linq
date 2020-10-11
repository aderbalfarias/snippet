<Query Kind="Program" />

static void Main() 
{
    int valueOut;
    OutMethod(out valueOut);
    Console.WriteLine(valueOut);
	//Output: 10
	
	int valueRef = 1;
    RefMethod(ref valueRef);
    Console.WriteLine(valueRef);
    //Output: 11
}


static void OutMethod(out int i) 
{
    i = 10;
}

static void RefMethod(ref int i) 
{
    i = i + 10;
}