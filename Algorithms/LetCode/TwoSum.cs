public class Solution 
{    
    public int[] TwoSum(int[] nums, int target) 
	{	
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

	public static void Main()
	{
		Console.WriteLine(TwoSum(new int[] { 2, 7, 11, 15 }, 9));
		Console.WriteLine(TwoSum(new int[] { 3, 2, 4 }, 6));
		Console.WriteLine(TwoSum(new int[] { 3, 2, 3 }, 6));
		Console.WriteLine(TwoSum(new int[] { 3, 3 }, 6));
		Console.WriteLine(TwoSum(new int[] { 6 }, 6));
	}
}

// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
// You may assume that each input would have exactly one solution, and you may not use the same element twice.
// You can return the answer in any order.

// Example 1:
// Input: nums = [2,7,11,15], target = 9
// Output: [0,1]
// Output: Because nums[0] + nums[1] == 9, we return [0, 1].

// Example 2:
// Input: nums = [3,2,4], target = 6
// Output: [1,2]

// Example 3:
// Input: nums = [3,3], target = 6
// Output: [0,1]