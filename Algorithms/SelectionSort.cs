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

	public static void Main()
	{
		int[] array = { 3, 5, 8, 4, 1, 9, -2 };

		SelectionSortV1(array);

		// print the sorted array
        Console.WriteLine(string.Join(",", array));
	}
}