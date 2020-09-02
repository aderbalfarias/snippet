public class GetBinarianLength
{
	public static void Main(string[] args)
	{
		Console.WriteLine(Calc("011100")); //Should be 7
		Console.WriteLine(Calc("111")); //Should be 5
		Console.WriteLine(Calc("1111010101111")); //Should be 22
		
		string test = "";
		for (int i = 0; i < 400000; i++)
		test = test + "1";
		
		Console.WriteLine(Calc(test)); //Should be 799999
	}
		
    public static int Calc(string S)
    {
        // Check if it is a valid binary but I guess there is no need 
        var binary = new Regex("^[01]{1,32}$");
        var isMatch = binary.Match(S).Success;

        int count = 0;

        // In case it is a valid binary
        if (isMatch)
        {
            // Convert to integer
            var numbem = Convert.ToInt32(S, 2);

            // Basic checks
            if (numbem == 0) return 0;
            if (numbem == 1) return 1;

            // Performe the main logic
            while (numbem > 0)
            {
                // using right shift operator to divede 
                numbem = numbem % 2 == 0 ? numbem >> 1 : numbem - 1;
                count++;
            }
        }
        else
        {
            // In case it is not possible to convert because it is too long
            return S.Length * 2 - 1;
        }

        return count;
    }
}