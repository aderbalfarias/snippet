<Query Kind="Program" />

//C#7
void Main()
{
	int[] array = { 1, 15, -39, 0, 7, 14, -12 };
	ref int place = ref Find(7, array); // aliases 7's place in the array
	place = 9; // replaces 7 with 9 in the array
	array[4].Dump();// prints 9
}

public ref int Find(int number, int[] numbers)
{
    for (int i = 0; i < numbers.Length; i++)
    {
		//numbers[i].Dump("n");
        if (numbers[i] == number) 
        {
			//numbers[i].Dump("n2");
			//i.Dump("i");
            return ref numbers[i]; // return the storage location, not the value
        }
    }
    throw new IndexOutOfRangeException($"{nameof(number)} not found");
}
