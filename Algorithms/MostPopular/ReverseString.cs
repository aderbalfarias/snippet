class Program  
{  
    static void Main(string[] args)  
    {  
        string str = string.Empty;   
		
        Console.WriteLine("Enter a Word");  
		
        // Getting String(word) from Console  
        str = Console.ReadLine();  
		
        // Displaying the reverse word  
        Console.WriteLine($"String: {str}");  
        Console.WriteLine($"Reverse: {ReverseWord(str)}");  
        Console.WriteLine($"Reverse: {ReverseStringKeepingSpecialCharacters(str)}");  
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

    public static string ReverseStringKeepingSpecialCharacters(string setence)  
    {  
		char []str = setence.ToCharArray();
        // Initialize left and right pointers  
        int r = str.Length - 1, l = 0;  
  
        // Traverse string from both ends until  
        // 'l' and 'r'  
        while(l < r)  
        {  
            // Ignore special characters  
            if (!char.IsLetterOrDigit(str[l]))  
                l++;  
            else if(!char.IsLetterOrDigit(str[r]))  
                r--;  
  
            // Both str[l] and str[r] are not spacial  
            else
            {  
                char tmp = str[l];  
                str[l] = str[r];  
                str[r] = tmp;  
                l++;  
                r--;  
            }  
        }  
		
		return new string(str);
	}
}  
