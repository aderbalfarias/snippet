class Program  
{  
    static void Main(string[] args)  
    {  
        string str = "", reverse = "";  
        int Length = 0;  
		
        Console.WriteLine("Enter a Word");  
		
        //Getting String(word) from Console  
        str = Console.ReadLine();  
		
        //Calculate length of string str  
        Length = str.Length - 1;  
		
        while(Length >= 0)  
        {  
            reverse = reverse + str[Length];  
            Length--;  
        }  
		
        //Displaying the reverse word  
        Console.WriteLine("Reverse word is {0}",reverse);  
        Console.ReadLine();  
    }  
}  
