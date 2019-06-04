// data structure to store graph edges
public class Edge
{
	public int source, dest;

	public Edge(int source, int dest) 
	{
		this.source = source;
		this.dest = dest;
	}
};

// class to represent a graph object
public class Graph
{
	// A List of Lists to represent an adjacency list
	public List<List<int>> adjList = null;

	// Constructor
	public Graph(List<Edge> edges, int N)
	{
		var adjList = new ArrayList(N);

		for (int i = 0; i < N; i++) {
			adjList.Add(i, new ArrayList());
		}

		// add edges to the undirected graph
		for (int i = 0; i < edges.Count(); i++)
		{
			int src = edges[i].source;
			int dest = edges[i].dest;

			adjList[src].Add(dest);
			adjList[dest].Add(src);
		}
	}
}

public class BFS
{
	// Perform BFS on graph starting from vertex v
	public static void BFSImplementation(Graph graph, int v, bool[] discovered)
	{
		// create a queue used to do BFS
		Queue<int> q = new Queue<int>();

		// mark source vertex as discovered
		discovered[v] = true;

		// push source vertex into the queue
		q.Enqueue(v);

		// run till queue is not empty
		while (!q.Any())
		{
			// pop front node from queue and print it
			v = q.Peek();
			Console.WriteLine(v + " ");

			// do for every edge (v -> u)
			foreach (int u in graph.adjList[v]) 
			{
				if (!discovered[u]) 
				{
					// mark it discovered and push it into queue
					discovered[u] = true;
					q.Enqueue(u);
				}
			}
		}
	}
}

// Iterative C# implementation of Breadth first search
public static void Main()
{
	// List of graph edges as per above diagram
	var edges = new List<Edge>
	{
		new Edge(1, 2), 
		new Edge(1, 3), 
		new Edge(1, 4),
		new Edge(2, 5), 
		new Edge(2, 6), 
		new Edge(5, 9),
		new Edge(5, 10), 
		new Edge(4, 7), 
		new Edge(4, 8),
		new Edge(7, 11), 
		new Edge(7, 12)
		// vertex 0, 13 and 14 are single nodes
	};

	// Set number of vertices in the graph
	int n = 15;

	// create a graph from edges
	Graph graph = new Graph(edges, n);

	// stores vertex is discovered or not
	bool[] discovered = new bool[n];

	// Do BFS traversal from all undiscovered nodes to
	// cover all unconnected components of graph
	for (int i = 0; i < n; i++)
		if (discovered[i] == false) 
			BFS.BFSImplementation(graph, i, discovered); // start BFS traversal from vertex i
}
