public class QuickSort
{
	public static void Swap(int[] array, int i, int j) {
		int temp = array[i];
		array[i] = array[j];
		array[j] = temp;
	}

	// Partition using Lomuto partition scheme
	public static int Partition(int[] array, int start, int end)
	{
		// Pick rightmost element as pivot from the array
		int pivot = array[end];

		// elements less than pivot will be pushed to the left of pIndex
		// elements more than pivot will be pushed to the right of pIndex
		// equal elements can go either way
		int pIndex = start;

		// each time we finds an element less than or equal to pivot,
		// pIndex is incremented and that element would be placed 
		// before the pivot.
		for (int i = start; i < end; i++)
		{
			if (array[i] <= pivot) 
			{
				Swap(array, i, pIndex);
				pIndex++;
			}
		}

		// swap pIndex with Pivot
		Swap(array, end, pIndex);

		// return pIndex (index of pivot element)
		return pIndex;
	}

	// Quicksort routine
	public static void QuickSortV1(int[] array, int start, int end)
	{
		// base condition
		if (start >= end)
			return;

		// rearrange the elements across pivot
		int pivot = Partition(array, start, end);

		// recur on sub-array containing elements less than pivot
		QuickSortV1(array, start, pivot - 1);

		// recur on sub-array containing elements more than pivot
		QuickSortV1(array, pivot + 1, end);
	}

	// Implementation of quicksort algorithm in C#
	public static void Main()
	{
		int[] array = { 9, -3, 5, 2, 6, 8, -6, 1, 3 };

		QuickSortV1(array, 0, array.Length - 1);

		// print the sorted array
		Console.WriteLine($"Output: [ { string.Join(", ", array) } ]");
	}
}

// Quicksort is an efficient in-place sorting algorithm, which usually performs about two to three times 
// faster than merge sort and heapsort when implemented well. Quicksort is a comparison sort, meaning 
// that it can sort items of any type for which a less-than relation is defined. In efficient 
// implementations, it is usually not a stable sort.

// Quicksort on average takes O(nlog(n)) comparisons to sort n items. In the worst case, it makes O(n2) 
// comparisons, though this behavior is very rare.

// How Quicksort works?
// Quicksort is a divide and conquer algorithm. Like all divide and conquer algorithms, it first divides 
// a large array into two smaller sub-arrays and then recursively sort the sub-arrays. Basically, 
// three steps are involved in whole process:
//     1 Pivot selection: Pick an element, called a pivot, from the array (usually the leftmost or the 
//     rightmost element of the partition).
//     2 Partitioning: Reorder the array so that all elements with values less than the pivot come 
//     before the pivot, while all elements with values greater than the pivot come after it 
//     (equal values can go either way). After this partitioning, the pivot is in its final position.
//     3 Recur: Recursively apply the above steps to the sub-array of elements with smaller values than 
//     pivot and separately to the sub-array of elements with greater values than pivot.

// The base case of the recursion is arrays of size 1, which never need to be sorted.