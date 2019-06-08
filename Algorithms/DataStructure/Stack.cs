public class Stack 
{ 
	static readonly int max = 1000; 
	int top = -1; 
	int[] stack = new int[max]; 

	bool IsEmpty() 
	{ 
		return (top < 0); 
	} 
	
	internal bool Push(int data) 
	{ 
		if (top >= max) 
		{ 
			Console.WriteLine("Stack Overflow"); 
			return false; 
		} 
		else
		{ 
			stack[++top] = data; 
			return true; 
		} 
	} 

	internal int Pop() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("Stack Underflow"); 
			return 0; 
		} 
		else
		{ 
			int value = stack[top--]; 
			return value; 
		} 
	} 

	internal void Peek() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("Stack Underflow"); 
			return; 
		} 
		else
			Console.WriteLine("The topmost element of Stack is : {0}", stack[top]); 
	} 

	internal void PrintStack() 
	{ 
		if (IsEmpty()) 
		{ 
			Console.WriteLine("Stack Underflow"); 
			return; 
		} 
		else
		{ 
			Console.WriteLine("Items in the Stack are :"); 
			for (int i = top; i >= 0; i--) 
			{ 
				Console.WriteLine(stack[i]); 
			} 
		} 
	} 

	public static void Main() 
	{ 
		Stack myStack = new Stack(); 
	
		myStack.Push(1); 
		myStack.Push(2); 
		myStack.Push(3); 
		myStack.Push(4); 
		myStack.PrintStack(); 
		myStack.Peek(); 
		Console.WriteLine($"Item popped from Stack : { myStack.Pop() }"); 
		Console.WriteLine($"Item popped from Stack : { myStack.Pop() }"); 
		myStack.PrintStack(); 
	} 
} 
