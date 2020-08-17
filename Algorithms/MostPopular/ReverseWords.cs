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
        Console.WriteLine($"New String: {ReverseWordsUsingSplit(str)}");  
        Console.WriteLine($"New String: {ReverseWordsNoMethods(str)}");  
        Console.WriteLine($"New String: {ReverseWordsKeepingSpecialCharacter(str)}");  
    }  
	
	public static string ReverseWordsUsingSplit(string words)
	{
		string reverseWords = string.Empty;   
		
		var wordList = words.Split(' ');
		
		for(int i = wordList.Length - 1; i >= 0; i--)
			reverseWords += wordList[i] + (i > 0 ? " " : "");	
			
		return reverseWords;
	}
	
	public static string ReverseWordsNoMethods(string words)
	{
		string reverseWords = string.Empty;
	    var wordList = new List<string>();
		int indexBegin = 0;
		int indexEnd = words.Length - 1;
		
		for(int i = words.Length - 1; i >= 0; i--)
		{
			if (words[i].Equals(' ') || i == 0)
			{
				indexBegin = i + (i == 0 ? 0 : + 1);
				
				var word = "";
				for (int j = indexBegin; j <= indexEnd; j++)
					word = word + words[j];

				reverseWords += word + (i > 0 ? " " : "");
				
				indexEnd = i - 1;
			}		
		}
		
		return reverseWords;
	}
	
	public static string ReverseWordsKeepingSpecialCharacter(string words)
	{
		string reverseWords = string.Empty;
	    
		// To be implmented
		
		return reverseWords;
	}
}