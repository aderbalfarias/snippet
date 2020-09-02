/*
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
		var rangeList = new List<int[]>{new int[]{0, 16}, new int[]{31, 64}, new int[]{65, 128}, new int[]{129, 256} };
		
		Console.WriteLine(Intersects(rangeList, 12));
		Console.WriteLine(Intersects(rangeList, 17));
	}
	
	static bool Intersects(List<int[]> rangeList, int n)
	{
	    for(int i = 0; i < rangeList.Count(); i++)
	        if(rangeList[i][0] <= n && rangeList[i][1] >= n)
	            return true;        
	    
	    return false;
	}
}
