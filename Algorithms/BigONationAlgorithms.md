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

One common example of a logarithmic time algorithm is the [binary search](I am gonna implement it) algorithm. 

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




![Image of Algorithms](/Algorithms/Images/AlgorithmsPerformance.png)
