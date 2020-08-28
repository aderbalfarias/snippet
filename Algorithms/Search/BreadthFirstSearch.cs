public class Algorithms 
{
    public HashSet<T> BFS<T>(Graph<T> graph, T start) 
    {
        var visited = new HashSet<T>();

        if (!graph.AdjacencyList.ContainsKey(start))
            return visited;
            
        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0) 
        {
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex))
                continue;

            visited.Add(vertex);

            foreach(var neighbor in graph.AdjacencyList[vertex])
                if (!visited.Contains(neighbor))
                    queue.Enqueue(neighbor);
        }

        return visited;
    }
	
	public Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start) 
	{
	    var previous = new Dictionary<T, T>();
	        
	    var queue = new Queue<T>();
	    queue.Enqueue(start);
	
	    while (queue.Count > 0) 
		{
	        var vertex = queue.Dequeue();
	        foreach(var neighbor in graph.AdjacencyList[vertex]) 
			{
	            if (previous.ContainsKey(neighbor))
	                continue;
	            
	            previous[neighbor] = vertex;
	            queue.Enqueue(neighbor);
	        }
	    }
	
	    Func<T, IEnumerable<T>> shortestPath = v => 
		{
	        var path = new List<T>{};
	
	        var current = v;
	        while (!current.Equals(start)) 
			{
	            path.Add(current);
	            current = previous[current];
	        };
	
	        path.Add(start);
	        path.Reverse();
	
	        return path;
	    };
	
	    return shortestPath;
	}
}

public class Graph<T> 
{
    public Graph() {}

    public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T,T>> edges) 
    {
        foreach(var vertex in vertices)
            AddVertex(vertex);

        foreach(var edge in edges)
            AddEdge(edge);
    }

    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

    public void AddVertex(T vertex) 
    {
        AdjacencyList[vertex] = new HashSet<T>();
    }

    public void AddEdge(Tuple<T,T> edge) 
    {
        if (AdjacencyList.ContainsKey(edge.Item1) 
            && AdjacencyList.ContainsKey(edge.Item2)) 
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
            AdjacencyList[edge.Item2].Add(edge.Item1);
        }
    }
}

// Iterative C# implementation of Breadth first search
public static void Main()
{
	var edges = new[]
	{
		Tuple.Create(1, 2), 
		Tuple.Create(1, 3), 
		Tuple.Create(1, 4),
		Tuple.Create(2, 5), 
		Tuple.Create(2, 6), 
		Tuple.Create(5, 9),
		Tuple.Create(5, 10), 
		Tuple.Create(4, 7), 
		Tuple.Create(4, 8),
		Tuple.Create(7, 11), 
		Tuple.Create(7, 12)
	};

	// Set number of vertices in the graph
	int n = 12;    
    int[] vertices = new int[n];
    
    for (int i = 1; i <= n; i++)
        vertices[i - 1] = i;     
	
	var graph = new Graph<int>(vertices, edges);
    var algorithms = new Algorithms();

    Console.WriteLine(string.Join(", ", algorithms.BFS(graph, 1)));
	
	var shortestPath = algorithms.ShortestPathFunction(graph, 1);
	
    foreach (var vertex in vertices)
        Console.WriteLine($"shortest path to { vertex }: { string.Join(", ", shortestPath(vertex)) }");
}
