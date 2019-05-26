// Start by assuming A[0] is a pole. Then start walking the array; 
// comparing each element A[i] in turn against A[0], and also tracking the current maximum.
// As soon as you find an i such that A[i] < A[0], you know that A[0] can no longer be a pole, 
// and by extension, neither can any of the elements up to and including A[i]. 
// So now continue walking until you find the next value that's bigger than the current maximum. 
// This then becomes the new proposed pole.
// Thus, an O(n) solution!

public class BalancePointOfArray
{
	public static int BalancePoint(int[] array)
    {
        int arraySum = 0;
        int leftSum = 0;
    
        for(int i = 0 ;i < array.Length; i++)
            arraySum += array[i];

        for(int i=0; i < array.Length; i++)
        {
            arraySum -= array[i];
                
            if(leftSum == arraySum)   
                return i;

            leftSum += array[i];
        }

        return -1;
    }

    public static void Main()
    {
        //Balance point of an vetor
        var input = new int[] { 2, 7, 4, 5, -3, 8, 9, -1 }; //Output: 3
        Console.WriteLine(BalancePoint(input));
    }
}