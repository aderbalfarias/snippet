public class GetBinarianLength
{
	public static void Main(string[] args)
	{
        Console.WriteLine(Q3.solution(new int[] { 1, 0, 2, 0, 0, 2 })); // Should be 3
	}
		
    public static int Calc(string S)
    {
            // 0(A) time

            int count = 0;

            double binarian = 0;
            for (int i = 0; i < A.Length; i++)
            {
                // Exponantial
                binarian += Math.Pow(2, A[i]); 
            }

            for (int i = A.Length - 1; i >= 0; i--)
            {
                var pow = Math.Pow(2, i);

                // If the binarian found, break the loop
                if (pow == binarian)
                {
                    count++;
                    break;
                }

                // Check until value lower than binarian found
                else if (pow < binarian)
                {
                    count++;
                    binarian = binarian - pow;
                }
            }

            return count;
    }
}