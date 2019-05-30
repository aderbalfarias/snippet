public class BubbleSort
{
	// Utility function to swap values at two indices in the array
	public static void Swap(int[] array, int i, int j)
	{
		int temp = array[i];
		array[i] = array[j];
		array[j] = temp;
	}

	// Function to perform bubble sort on array
	public static void BubbleSortV1(int[] array)
	{
		// (array.Length - 1) pass
		for (int k = 0; k < array.Length - 1; k++)
		{
			// last k items are already sorted, so inner loop can
			// avoid looking at the last k items
			for (int i = 0; i < array.Length - 1 - k; i++)
				if (array[i] > array[i + 1])
					Swap(array, i, i + 1);

			// the algorithm can be stopped if the inner loop
			// didn't do any swap
		}
	}

    // Recursive function to perform bubble sort on subarray array[i..n]
	public static void BubbleSortV2(int[] array, int n)
	{
		for (int i = 0; i < n - 1; i++)
			if (array[i] > array[i + 1])
				Swap(array, i, i + 1);
		
		if (n - 1 > 1)
			BubbleSortV2(array, n - 1);
	}

	public static void Main(String[] args)
	{
		int[] array = { 3, 5, 8, 4, 1, 9, -2 };

		BubbleSortV1(array);
		// print the sorted array
		Console.WriteLine($"v1: [ { string.Join(", ", array) } ]");
		
		int[] arrayRecursive = { 3, 5, 8, 4, 1, 9, -2 };
		
		BubbleSortV2(arrayRecursive, arrayRecursive.Length);
		// print the sorted array
        Console.WriteLine($"v2: [ { string.Join(", ", arrayRecursive) } ]");
	}
}