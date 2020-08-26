class Program  
{  
    static void Main(string[] args)  
    {  
        string str = string.Empty;   
		
        Console.WriteLine("Enter a Word");  
		
        // Getting String(word) from Console  
        str = Console.ReadLine();  
		
        // Output  
        Console.WriteLine($"String: {str}");  
        Console.WriteLine($"Reverse: {ReverseString(str)}");  
        Console.WriteLine($"Reverse: {ReverseStringKeepingSpecialCharacters(str)}");  
        Console.ReadLine();  
    }  
	
	public static string ReverseString(string str)
	{
		string reverse = string.Empty;   
		
		// Calculate length of string str  
		int length = str.Length - 1;  
		for(int i = length; i >= 0; i--)
			reverse = reverse + str[i];  
			
		return reverse;
	}

    public static string ReverseStringKeepingSpecialCharacters(string str)  
    {  
		char []strChar = str.ToCharArray();
        
        // Initialize left and right pointers  
        int r = strChar.Length - 1, l = 0;  
  
        // Traverse string from both ends until  
        // 'l' and 'r'  
        while(l < r)  
        {  
            // Ignore special characters  
            if (!char.IsLetterOrDigit(strChar[l]))  
                l++;  
            else if(!char.IsLetterOrDigit(strChar[r]))  
                r--;  
  
            // Both strChar[l] and strChar[r] are not spacial  
            else
            {  
                char tmp = strChar[l];  
                strChar[l] = strChar[r];  
                strChar[r] = tmp;  
                l++;  
                r--;  
            }  
        }  
		
		return new string(strChar);
	}
}  
