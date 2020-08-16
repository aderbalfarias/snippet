class Program  
{  
    static void Main(string[] args)  
    {  
        string str = string.Empty;   
		
        Console.WriteLine("Enter a Word");  
		
        // Getting String(word) from Console  
        str = Console.ReadLine();  
		
        // Displaying the reverse word  
        Console.WriteLine($"Reverse word is {ReverseWord(str)}");  
        Console.ReadLine();  
    }  
	
	public static string ReverseWord(string str)
	{
		string reverse = string.Empty;   
		
		// Calculate length of string str  
		int length = str.Length - 1;  
		for(int i = length; i >= 0; i--)
			reverse = reverse + str[i];  
			
		return reverse;
	}
}  
