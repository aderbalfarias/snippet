public class BinarySearch
{
	// find out if a key x exists in the sorted array array
	// or not using binary search algorithm
	public static int BinarySearchV1(int[] array, int x)
	{
		// search space is array[left..right]
		int left = 0, right = array.Length - 1;

		// till search space consists of at-least one element
		while (left <= right)
		{
			// we find the mid value in the search space and
			// compares it with key value

			int mid = (left + right) / 2;

			// overflow can happen. Use:
			// int mid = left + (right - left) / 2;
			// int mid = right - (right - left) / 2;

			// key value is found
			if (x == array[mid])
				return mid;		

			// discard all elements in the right search space
			// including the mid element
			else if (x < array[mid])
				right = mid - 1;

			// discard all elements in the left search space
			// including the mid element
			else
				left = mid + 1;
		}

		// x doesn't exist in the array
		return -1; 
	}
	
	// Recursive
    // Find out if a key x exists in the sorted array
	// A[left..right] or not using binary search algorithm
	public static int BinarySearchV2(int[] array, int left, int right, int x)
	{
		// Base condition (search space is exhausted)
		if (left > right)
			return -1;

		// we find the mid value in the search space and
		// compares it with key value

		int mid = (left + right) / 2;

		// overflow can happen. Use beleft
		// int mid = left + (right - left) / 2;

		// Base condition (key value is found)
		if (x == array[mid])
			return mid;

		// discard all elements in the right search space
		// including the mid element
		else if (x < array[mid])
			return BinarySearchV2(array, left,  mid - 1, x);

		// discard all elements in the left search space
		// including the mid element
		else
			return BinarySearchV2(array, mid + 1,  right, x);
	}

	public static void PrintResult(int index, string v)
	{
		Console.WriteLine(index != -1 
			? $"{v}: Element found at index { index }" 
			: $"{v}: Element not found in the array"
        );
	}
	
	public static void Main()
	{
		PrintResult(BinarySearchV1(new int[] { 2, 5, 6, 8, 9, 10 }, 5), "V1");
		
		var recursive = new int[] { 2, 5, 6, 8, 9, 10 };
		PrintResult(BinarySearchV2(recursive, 0, recursive.Length - 1, 5), "V2");		
	}
}