public class Stack 
{ 
	static readonly int max = 1000; 
	int top = -1; 
	int[] stack = new int[max]; 

	bool IsEmpty() 
	{ 
		return (top < 0); 
	} 
	
	public bool Push(int data) 
	{ 
		if (top >= max) 
		{ 
			Console.WriteLine("\nStack Overflow"); 
			return false; 
		} 
		else
		{ 
			stack[++top] = data; 
			return true; 
		} 
	} 

	public int Pop() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("\nStack Underflow"); 
			return 0; 
		} 
		else
		{ 
			int value = stack[top--]; 
			return value; 
		} 
	} 

	public void Peek() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("\nStack Underflow"); 
			return; 
		} 
		else
			Console.WriteLine($"\nThe topmost element of stack: { stack[top] }"); 
	} 

	public void PrintElements() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("Stack Underflow"); 
			return; 
		} 
		else
		{ 
			Console.WriteLine("Items in the stack:"); 
			for (int i = top; i >= 0; i--) 
				Console.WriteLine(stack[i]); 
		} 
	} 
} 

public class TestStack
{
	public static void Main() 
	{ 
		Stack myStack = new Stack(); 
	
		myStack.Push(1); 
		myStack.Push(2); 
		myStack.Push(3); 
		myStack.Push(4); 
		myStack.PrintElements(); 
		
		myStack.Peek(); 
		Console.WriteLine($"\nItem popped from stack: { myStack.Pop() }"); 
		Console.WriteLine($"\nItem popped from stack: { myStack.Pop() }"); 
		
		myStack.PrintElements(); 
	} 
}