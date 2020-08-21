public class Node
{
    public int Value;
    public Node Next;
}

public class Queue
{
    private Node Head;
    private int Size;

    public Queue(){}

    public void Enqueue(int n)
    {
        if(Head == null) // queue is empty
        {
            Head = new Node
			{
                Value = n,
                Next = null
            };
        }
        else // queue has items
        {
            var oldHead = Head;
            Head = new Node
            {
                Value = n,
                Next = oldHead
            };
        }
		
        Size++;
    }

    public int? Dequeue()
    {
        if (Size == 0)
            return null;

        var node = Head;
        Node previous = node;
		
        while (node.Next != null)
        {
            previous = node;
            node = node.Next;
        }
		
        previous.Next = null;
        Size--;
		
        return node.Value;
    }

    public int Count
    {
        get { return Size; }
    }

    public string PrintElements()
    {
        var node = Head;
        int[] elements = new int[Size];
        int i = 0;
		
        while (node != null)
        {
            elements[i++] = node.Value;
            node = node.Next;
        }
		
        return string.Join(", ", elements);
    }
}

public class TestQueue
{
	public static void Main() 
	{ 
		Queue myQueue = new Queue(); 
	
		myQueue.Enqueue(1); 
		myQueue.Enqueue(2); 
		myQueue.Enqueue(3); 
		myQueue.Enqueue(4); 
		Console.WriteLine($"Items in the queue: { myQueue.PrintElements() }");
		// Output: Items in the queue: 4, 3, 2, 1

        myQueue.Dequeue();
		Console.WriteLine($"Items in the queue: { myQueue.PrintElements() }");
		// Output: Items in the queue: 4, 3, 2
		
        myQueue.Dequeue();
		Console.WriteLine($"Item in the queue: { myQueue.PrintElements() }");
		// Output: Items in the queue: 4, 3
	} 
}