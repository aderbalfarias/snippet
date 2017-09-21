<Query Kind="Program" />

//C#3
void Main()
{
 	// int is the type argument
    GenericList<int> list1 = new GenericList<int>();
    GenericList<string> list2 = new GenericList<string>();

    for (int x = 0; x < 10; x++)
    {
        list1.AddHead(x);
    }
	
	list2.AddHead("One");
	list2.AddHead("Two");
	list2.AddHead("Three");
	
	foreach (int i in list1)
    {
        System.Console.Write(i + " ");
    }
	
	foreach (string i in list2)
    {
        System.Console.Write(i + " ");
    }
	
    System.Console.WriteLine("\nDone");
}

public class GenericList<T> 
{
    // The nested class is also generic on T.
    private class Node
    {
        // T used in non-generic constructor.
        public Node(T t)
        {
            next = null;
            data = t;
        }

        private Node next;
        public Node Next
        {
            get { return next; }
            set { next = value; }
        }
        
        // T as private member data type.
        private T data;

        // T as return type of property.
        public T Data  
        {
            get { return data; }
            set { data = value; }
        }
    }

    private Node head;
    
    // constructor
    public GenericList() 
    {
        head = null;
    }

    // T as method parameter type:
    public void AddHead(T t) 
    {
        Node n = new Node(t);
        n.Next = head;
        head = n;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node current = head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }
}