void Main()
{
	int i1 = 1024;

    int i2 = i1 >> 1;  
    int i3 = i1 >> 2;  
    int i4 = i1 >> 10;  
	
	Console.WriteLine(i1); // 1024
	Console.WriteLine(i2); // 512
	Console.WriteLine(i3); // 256
	Console.WriteLine(i4); // 1
}