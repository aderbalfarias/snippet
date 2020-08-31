public static class CompareIndexesWithValues
{
		
	static void Main(string[] args)
    {
        Console.WriteLine("Tests Algorithm");
        Console.WriteLine($"Should return 3: { Calc(new int[] { 2, 1, 3, 5, 4 }) }"); //Should be 3
        Console.WriteLine($"Should return 2: { Calc(new int[] { 2, 3, 4, 1, 5 }) }"); //Should be 2
        Console.WriteLine($"Should return 3: { Calc(new int[] { 1, 3, 4, 2, 5 }) }"); //Should be 3
    }
		
    public static int Calc(int[] a)
    {
        // Compare indexes and array values
        // start from 1 check the previous and sum
        // 0(a) time

        int count = 0;
        int target = 0;
        int sum = 0;

        for (int i = 1; i <= a.Length; i++)
        {
            sum += a[i - 1];
            target += i;

            if (sum == target)
                count++;
        }

        return count;
    }
}