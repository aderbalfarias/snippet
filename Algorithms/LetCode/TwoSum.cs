public class Solution {
    public static void Main()
	{
		Console.WriteLine(TwoSum(new int[] { 2, 7, 11, 15 }, 9));
		Console.WriteLine(TwoSum(new int[] { 3, 2, 4 }, 6));
		Console.WriteLine(TwoSum(new int[] { 3, 2, 3 }, 6));
		Console.WriteLine(TwoSum(new int[] { 3, 3 }, 6));
		Console.WriteLine(TwoSum(new int[] { 6 }, 6));
	}
    
    public int[] TwoSum(int[] nums, int target) {	
       int length = nums.Length;
		
		if (length > 1)
		{
			int sum = 0;
			
			for(int i = 0; i < length; i++)
				for(int j = i + 1; j < length; j++)
					if(i != j)
					{
						sum = nums[i] + nums[j];
						
						if (sum == target)
							return new int[2]{ i, j };
					}
		}
		
		return null;
    }
}