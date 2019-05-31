public class MergeSort
{
	// Merge two sorted sub-arrays array[low .. mid] and array[mid + 1 .. high]
	public static void Merge(int[] array, int[] aux, int low, int mid, int high)
	{
		int k = low;
		int i = low;
		int j = mid + 1;

		// While there are elements in the left and right runs
		while (i <= mid && j <= high)
		{
			if (array[i] < array[j])
				aux[k++] = array[i++];
			else
				aux[k++] = array[j++];
		}

		// Copy remaining elements
		while (i <= mid)
			aux[k++] = array[i++];

		// Don't need to copy second half

		// copy back to the original array to reflect sorted order
		for (i = low; i <= high; i++)
			array[i] = aux[i];
	}

	// Sort array array [low..high] using auxiliary array aux
	public static void MergeSortV1(int[] array, int[] aux, int low, int high)
	{
		// Base case
		if (high == low) // if run size == 1
			return;

		// find mid point
		int mid = (low + ((high - low) >> 1));

		// recursively split runs into two halves until run size == 1,
		// then merge them and return back up the call chain

		MergeSortV1(array, aux, low, mid);	  // split / merge left  half
		MergeSortV1(array, aux, mid + 1, high); // split / merge right half

		Merge(array, aux, low, mid, high);	// merge the two half runs
	}

	// Function to check if array is sorted in ascending order or not
	public static bool IsSorted(int[] array)
	{
		int prev = array[0];
		
		for (int i = 1; i < array.Length; i++) 
		{
			if (prev > array[i]) 
			{
				Console.WriteLine("MergeSort Fails!");
				return false;
			}
			
			prev = array[i];
		}

		return true;
	}

	// Implementation of Merge Sort Algorithm in C#
	public static void Main(String[] args)
	{
		int[] array = { 12, 3, 18, 24, 0, 5, -2 };
		int[] aux =  new int[array.Length]; 
		
		Array.Copy(array, aux, array.Length);

		// sort array 'array' using auxiliary array 'aux'
		MergeSortV1(array, aux, 0, array.Length - 1);

		if (IsSorted(array))
			Console.WriteLine($"Output: [ { string.Join(", ", array) } ]");
	}
}