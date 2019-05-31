public class SelectionSort
{
	// Utility function to swap values at two indices in the array
	public static void Swap(int[] array, int i, int j)
	{
		int temp = array[i];
		array[i] = array[j];
		array[j] = temp;
	}

	// Function to perform selection sort on array[]
	public static void SelectionSortV1(int[] array)
	{
		// run (array.Length - 1) times
		for (int i = 0; i < array.Length - 1; i++)
		{
			// find the minimum element in the unsorted sub-array[i..n-1]
			// and swap it with array[i]
			int min = i;

			for (int j = i + 1; j < array.Length; j++)
				if (array[j] < array[min]) // if array[j] element is less, then it is the new minimum
					min = j; // update index of min element

			// swap the minimum element in sub-array[i..n-1] with array[i]
			Swap(array, min, i);
		}
	}
	
		// Recursive function to perform selection sort on sub-array arr[i..n-1]
	public static void SelectionSortV2(int[] array, int i, int n)
	{
		// find the minimum element in the unsorted sub-array[i..n-1]
		// and swap it with array[i]
		int min = i;
		for (int j = i + 1; j < n; j++)
			// if array[j] element is less, then it is the new minimum
			if (array[j] < array[min])
				min = j; // update index of min element

		// swap the minimum element in sub-array[i..n-1] with array[i]
		Swap(array, min, i);

		if (i + 1 < n)
			SelectionSortV2(array, i + 1, n);
	}

	public static void Main()
	{
		int[] array = { 3, 5, 8, 4, 1, 9, -2 };

		SelectionSortV1(array);
		// print the sorted array
        Console.WriteLine($"v1: [ { string.Join(", ", array) } ]");
		
		int[] arrayRecursive = { 3, 5, 8, 4, 1, 9, -2 };
		
		SelectionSortV2(arrayRecursive, 0, arrayRecursive.Length);
		// print the sorted array
        Console.WriteLine($"v2: [ { string.Join(", ", arrayRecursive) } ]");
	}
}

// Selection sort is a unstable, in-place sorting algorithm known for its simplicity, 
// and it has performance advantages over more complicated algorithms in certain situations, 
// particularly where auxiliary memory is limited. It can be implemented as a stable sort. 
// It has O(n2) time complexity, making it inefficient to use on large lists. Among simple 
// average-case O(n2) algorithms, selection sort almost always outperforms bubble sort and generally 
// performs worse than the similar insertion sort.

// The biggest advantage of using selection sort is that we only requires maximum n swaps (memory write) 
// where n is the length of the input. insertion sort, on the other hand, takes O(n2) number of writes. 
// This can be very important if memory write operation is significantly more expensive than memory 
// read operation, such as with Flash memory, where every write lessens the lifespan of the memory.

// How it works?
// The idea is to divide the array into two subsets – sorted sublist and unsorted sublist. 
// Initially, the sorted sublist is empty and the unsorted sublist is the entire input list. 
// The algorithm proceeds by finding the smallest (or largest, depending on sorting order) 
// element in the unsorted sublist, swapping it with the leftmost unsorted element 
// (putting it in sorted order), and moving the sublist boundaries one element to the right. 

// Image on Images/SelectionSort.png
//       [  3  5  8  4  1  9 -2 ]
// i = 0 [ -2  5  8  4  1  9  3 ]
// i = 1 [ -2  1  8  4  5  9  3 ]
// i = 2 [ -2  1  3  4  5  9  8 ]
// i = 3 [ -2  1  3  4  5  9  8 ]
// i = 4 [ -2  1  3  4  5  9  8 ]
// i = 5 [ -2  1  3  4  5  8  9 ] Final output