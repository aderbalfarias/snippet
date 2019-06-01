# Algorithms Performance

Big O Notation is an Intuition that study of the performance of algorithms – or algorithmic complexity – falls into the field of algorithm analysis. Algorithm analysis answers the question of how many resources, such as disk space or time, an algorithm consumes.


### Constant Time Algorithms – O(1)

How does this input size of an algorithm affect its running time? Key to understanding Big O is understanding the rates at which things can grow. The rate in question here is time taken per input size.

Consider this simple piece of code:

```csharp
void Main()
{
    int n = 1000;
    Console.WriteLine($"Your input: { n }");
}
```

Clearly, it doesn't matter what **n** is, above. This piece of code takes a constant amount of time to run. It's not dependent on the size of **n**.

Similarly:

```csharp
void Main()
{
    int n = 1000;
    Console.WriteLine($"Your input: { n }");
    Console.WriteLine($"Your input again: { n }");
    Console.WriteLine($"Your input one more time: { n }");
} 	
```

The above example is also constant time. Even if it takes 3 times as long to run, it doesn't depend on the size of the input, **n**. We denote constant time algorithms as follows: **O(1)**. Note that **O(2)**, **O(3)** or even **O(1000)** would mean the same thing.

>We don't care about exactly how long it takes to run, only that it takes constant time.

### Logarithmic Time Algorithms – O(log n)

Constant time algorithms are (asymptotically) the quickest. Logarithmic time is the next quickest. Unfortunately, they're a bit trickier to imagine.

One common example of a logarithmic time algorithm is the [binary search](BinarySearch.cs) algorithm. 

What is important here is that the running time grows in proportion to the logarithm of the input (in this case, log to the base 2):

```csharp
void Main()
{
    var n = 8;
    
    for (int i = 1; i < n; i = i * 2)
        Console.WriteLine($"Output: { i }");
} 
```

>Result:  
>Output: 1  
>Output: 2  
>Output: 4

The algorithm ran **log(8) = 3 times**.  

### Linear Time Algorithms – O(n)

After logarithmic time algorithms, we get the next fastest class: linear time algorithms.

If we say something grows linearly, we mean that it grows directly proportional to the size of its inputs.

Think of a simple for loop:

```csharp
void Main()
{
    int n = 10;
	
    for (int i = 0; i < n; i++) 
        Console.WriteLine($"Output: { i }");
}
```

How many times does this for loop run? n times, of course! We don't know exactly how long it will take for this to run – and we don't worry about that.

What we do know is that the simple algorithm presented above will grow linearly with the size of its input.

We'd prefer a run time of **0.1n than (1000n + 1000)**, but both are still linear algorithms; they both grow directly in proportion to the size of their inputs.

Again, if the algorithm was changed to the following:

```csharp
void Main()
{
    int n = 10;
    
    for (int i = 0; i < n; i++) 
    {
        Console.WriteLine($"Output: { i }");
        Console.WriteLine($"Output again: { i }");
        Console.WriteLine($"Output one more time: { i }");
    }
}
```

The runtime would still be linear in the size of its input, **n**. We denote linear algorithms as follows: O(n).

As with the constant time algorithms, we don't care about the specifics of the runtime. **O(2n+1) is the same as O(n)**, as Big O Notation concerns itself with growth for input sizes.


### N Log N Time Algorithms – O(n log n)

n log n is the next class of algorithms. The running time grows in proportion to n log n of the input:

```csharp
void Main()
{
    var n = 8;
	
    for (int i = 1; i <= n; i++)
        for(int j = 1; j < n; j = j * 2) 
            Console.WriteLine($"Output for i: { i } and j: { j }");
}
```

For example, if the **n is 8**, then this algorithm will run **8 * log(8) = 8 * 3 = 24 times**. Whether we have strict inequality or not in the for loop is irrelevant for the sake of a Big O Notation.

### Polynomial Time Algorithms – O(np)

Next up we've got polynomial time algorithms. These algorithms are even slower than n log n algorithms.

The term polynomial is a general term which contains quadratic (n2), cubic (n3), quartic (n4), etc. functions. What's important to know is that O(n2) is faster than O(n3) which is faster than O(n4), etc.

Let's have a look at a simple example of a quadratic time algorithm:

```csharp
void Main()
{
    var n = 8;
	
    for (int i = 1; i <= n; i++)
        for(int j = 1; j < n; j++) 
            Console.WriteLine($"Output for i: { i } and j: { j }");
}
```

This algorithm will run 64 times. Note, if we were to nest another for loop, this would become an O(n3) algorithm.


### Exponential Time Algorithms – O(kn)

Now we are getting into dangerous territory; these algorithms grow in proportion to some factor exponentiated by the input size.

For example, O(2n) algorithms double with every additional input. So, if n = 2, these algorithms will run four times; if n = 3, they will run eight times (kind of like the opposite of logarithmic time algorithms).

O(3n) algorithms triple with every additional input, O(kn) algorithms will get k times bigger with every additional input.

Let's have a look at a simple example of an O(2n) time algorithm:

```csharp
void Main()
{
    var n = 8;
	
    for (int i = 1; i <= Math.Pow(2, n); i++)
        Console.WriteLine($"Output i: { i }");
}
```

This algorithm will 256 times.

### Factorial Time Algorithms – O(n!)

In most cases, this is pretty much as bad as it'll get. This class of algorithms has a run time proportional to the factorial of the input size.

A classic example of this is solving the traveling salesman problem using a brute-force approach to solve it.

An explanation of the solution to the traveling salesman problem is beyond the scope of this article.

Instead, let's look at a simple **O(n!)** algorithm, as in the previous sections:

```csharp
public void Main()
{
    var n = 8;
    
    for (int i = 1; i <= Factorial(n); i++)
        Console.WriteLine($"Output i: { i }");
}

public int Factorial(int number)
{
    if (number == 1)
        return 1;
    else
        return number * Factorial(number - 1);
}
```

where factorial(n) simply calculates **n!**. If n is 8, **this algorithm will run 8! = 40320 times**.


![Image of Algorithms](/Algorithms/Images/AlgorithmsPerformance.png)

## Asymptotic Functions

Big O is what is known as an asymptotic function. All this means, is that it concerns itself with the performance of an algorithm at the limit – i.e. – when lots of input is thrown at it.

Big O doesn't care about how well your algorithm does with inputs of small size. It's concerned with large inputs (think sorting a list of one million numbers vs. sorting a list of 5 numbers).

Another thing to note is that there are other asymptotic functions. Big Θ (theta) and Big Ω (omega) also both describes algorithms at the limit (remember, the limit this just means for huge inputs).

To understand the differences between these 3 important functions, we first need to know that each of Big O, Big Θ, and Big Ω describes a set (i.e., a collection of elements).

