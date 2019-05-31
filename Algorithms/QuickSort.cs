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