//Note: Comments on the bottom of this file
public class InsertionSort
{
	// Function to perform insertion sort on array[]
	public static void InsertionSortV1(int[] array)
	{
		// (element at index 0 is already sorted)
		// Start from the second element
        for (int i = 1; i < array.Length; i++)
		{
			int value = array[i];
			int j = i;

			// Find the index j within the sorted subset array[0..i-1]
			// where element array[i] belongs
			while (j > 0 && array[j - 1] > value)
			{
				array[j] = array[j - 1];
				j--;
			}

			// Note that sub-array array[j..i-1] is shifted to
			// the right by one position i.e. array[j+1..i]
			array[j] = value;
		}
	}

	// Recursive function to perform insertion sort on sub-array arr[i..n]
	public static void InsertionSortV2(int[] array, int i, int n)
	{
		int value = array[i];
		int j = i;

		// Find index j within the sorted subset array[0..i-1]
		// where element array[i] belongs
		while (j > 0 && array[j - 1] > value)
		{
			array[j] = array[j - 1];
			j--;
		}

		array[j] = value;

		// Note that subarray array[j..i-1] is shifted to
		// the right by one position i.e. array[j+1..i]
		if (i + 1 <= n)
			InsertionSortV2(array, i + 1, n);
	}

	public static void Main()
	{
        //Method 1
		int[] arrayInput = { 3, 8, 5, 4, 1, 9, -2 }; 

		InsertionSortV1(arrayInput);        
		Console.WriteLine($"Normal method: \n [ { string.Join(",", arrayInput) } ] \n");

        // Method 2
		int[] arrayRecursiveInput = { 3, 8, 5, 4, 1, 9, -2 }; 

        // Start from second element (element at index 0 is already sorted)
		InsertionSortV2(arrayRecursiveInput, 1, arrayRecursiveInput.Length - 1);
		Console.WriteLine($"Recursive method: \n[ { string.Join(",", arrayRecursiveInput) } ]");
	}
}

// Insertion sort is stable, in-place sorting algorithm that builds the final sorted array 
// one item at a time. It is not very best in terms of performance but it is more efficient 
// in practice than most other simple O(n2) algorithms such as selection sort or bubble sort. 
// Insertion sort is also used is used in Hybrid sort which combines different sorting 
// algorithms to improve performance.

// It is also a well known online algorithm as it can sort a list as it receives it. 
// In all other algorithms we need all elements to be provided to the sorting algorithm 
// before applying it. But an insertion sort allows we to start with partial set of elements, 
// sorts it (called as partially sorted set), and if we want, we can insert more elements 
// (these are the new set of elements that were not in memory when the sorting started) and 
// sorts these elements too. In real world, data to be sorted is usually not static, rather dynamic. 
// If even one additional element is inserted during the sort process, other algorithms don't respond 
// easily. But only this algorithm is not interrupted and can respond well with the additional element.

// How it works?
// The idea is to divide the array into two subsets – sorted subset and unsorted subset. 
// Initally sorted subset consists of only one first element at index 0. Then for each iteration, 
// insertion sort removes next element from the unsorted subset, finds the location it belongs 
// within the sorted subset, and inserts it there. It repeats until no input elements remain. 

// Image on Images/InsertionSort.png
// i = 1    [  3  8  5  4  1  9  -2 ]
// i = 2    [  3  8  5  4  1  9  -2 ]           
// i = 3    [  3  5  8  4  1  9  -2 ]              
// i = 4    [  3  4  5  8  1  9  -2 ]
// i = 5    [  1  3  4  5  8  9  -2 ]
// i = 6    [  1  3  4  5  8  9  -2 ]
//          [ -2  1  3  4  5  8   9 ] Final output
 