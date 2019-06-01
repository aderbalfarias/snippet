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

	public static void Main()
	{
		int[] array = { 2, 5, 6, 8, 9, 10 };
		int key = 5;

		int index = BinarySearchV1(array, key);

		if (index != -1)
			Console.WriteLine($"Element found at index { index }");
		else
			Console.WriteLine("Element not found in the array");
	}
}