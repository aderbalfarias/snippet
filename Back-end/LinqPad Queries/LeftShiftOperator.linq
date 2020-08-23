void Main()
{
	int i1 = 1024;

    int i2 = i1 << 1; 
    int i3 = i1 << 2;  
    int i4 = i1 << 10; 
	
	Console.WriteLine(i1); // 1024
	Console.WriteLine(i2); // 2048
	Console.WriteLine(i3); // 4096
	Console.WriteLine(i4); // 1048576
}
