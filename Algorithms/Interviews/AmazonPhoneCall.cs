/*
 *Given a number(N) and list of ranges find if the number intersects any of the entries in the list.
 *[ 0,16,
 *  31-64,
 *  65-128,
 *  129-256]
 *
 *N = 12 > True
 *N = 17 > False
**/

public class AmazonPhoneCall
{
	public static void Main()
	{
		var rangeList = GenerateListOfRanges();
		
		Console.WriteLine(IntersectsV1(rangeList, 1000000004));
		Console.WriteLine(IntersectsV1(rangeList, 1000000006));

		Console.WriteLine(IntersectsV2(rangeList, 1000000004));
		Console.WriteLine(IntersectsV2(rangeList, 1000000006));
	}
	
	// O(n) time
	public static bool IntersectsV1(List<int[]> rangeList, int n)
	{
	    for(int i = 0; i < rangeList.Count(); i++)
	        if(rangeList[i][0] <= n && rangeList[i][1] >= n)
	            return true;        
	    
	    return false;
	}

	// O(log n) time using pointers
	public static bool IntersectsV2(List<int[]> rangeList, int n)
	{
		var left = 0;
		var right = rangeList.Count - 1;

		while(left <= right)
		{
			if(rangeList[left][0] <= n && rangeList[left][1] >= n)
				return true;

			if (rangeList[right][0] <= n && rangeList[right][1] >= n)
				return true;

			left++;
			right--;
		}

		return false;
	}

	public static List<int[]> GenerateListOfRanges()
	{
		var list = new List<int[]>();

		for (int i = 0; i <= 1000000000; i += 10)
			list.Add(new int[] { i, i + 5 });

		return list;
	}
}
