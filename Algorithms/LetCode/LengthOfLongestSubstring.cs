public class Solution 
{
	public static void Main()
	{
		Console.WriteLine(LengthOfLongestSubstring("abcabcbb")); // Output: 3
		Console.WriteLine(LengthOfLongestSubstring("bbbbb")); // Output: 1
		Console.WriteLine(LengthOfLongestSubstring("pwwkew")); // Output: 3
		Console.WriteLine(LengthOfLongestSubstring("")); // Output: 0
	}
	
    public static int LengthOfLongestSubstring(string s) 
    {
        var hs = new HashSet<char>();
        int n = s.Length, result = 0, i = 0, j = 0;
		
        while (i < n && j < n) 
        {
            if (!hs.Contains(s[j]))
			{
                hs.Add(s[j++]);
                result = Math.Max(result, j - i);
            }
            else
                hs.Remove(s[i++]);
            
        }
		
        return result;
    }
}


// Given a string s, find the length of the longest substring without repeating characters.

// Example 1:
// Input: s = "abcabcbb"
// Output: 3
// Explanation: The answer is "abc", with the length of 3.

// Example 2:
// Input: s = "bbbbb"
// Output: 1
// Explanation: The answer is "b", with the length of 1.

// Example 3:
// Input: s = "pwwkew"
// Output: 3
// Explanation: The answer is "wke", with the length of 3.
// Notice that the answer must be a substring, "pwke" is a subsequence and not a substring.

// Example 4:
// Input: s = ""
// Output: 0