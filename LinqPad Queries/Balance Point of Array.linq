<Query Kind="Program" />

static void Main(){
	//Balance point of vetor
	var input = new int[] {2, 7, 4, 5, -3, 8, 9, -1}; //Output: 3
	BalancePoint(input).Dump();
}

public static int BalancePoint(int[] array)
{
	int arraySum = 0;
    int leftSum = 0;
 
    for(int i = 0 ;i < array.Length; i++)
	{
        arraySum += array[i];
	}

    for(int i=0; i < array.Length; i++)
    {
        arraySum -= array[i];
            
        if(leftSum == arraySum)   
            return i;

    	leftSum += array[i];
    }

    return -1;
}