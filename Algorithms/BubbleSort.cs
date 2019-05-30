//Note: Comments on the bottom of this file
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

 
// Bubble sort is a stable, in-place sorting algorithm that is named for the way smaller or larger 
// elements "bubble" to the top of the list. Although the algorithm is simple, it is too slow and 
// impractical for most problems even when compared to insertion sort and is not recommended when n is large.

// The only significant advantage that bubble sort has over most other implementations, even quicksort, 
// but not insertion sort, is that the ability to detect if the list is already sorted. When the list 
// is already sorted (best-case), the complexity of bubble sort is only O(n).
  
// How it works?
// Each pass of bubble sort steps through the list to be sorted, compares each pair of adjacent items 
// and swaps them if they are in the wrong order. At the end of each pass, the next largest element 
// will "Bubble" up to its correct position. These passes through the list are repeated until no 
// swaps are needed, which indicates that the list is sorted. In worst case, we might end up making 
// n – 1 passes where n is the size of the input.

//               3     5     8     4     1     9    -2 
// pass 1        3     5     4     1     8    -2     9
// pass 2        3     4     1     5    -2     8     9
// pass 3        3     1     4    -2     5     8     9
// pass 4        1     3    -2     4     5     8     9
// pass 5        1    -2     3     4     5     8     9
// pass 6       -2     1     3     4     5     8     9