//Indices and ranges

//Ranges and indices provide a succinct syntax for specifying subranges in an array, Span<T>, or ReadOnlySpan<T>.

//You can specify an index from the end. You specify from the end using the ^ operator. 
//You are familiar with array[2] meaning the element "2 from the start". Now, array[^2] means 
//the element "2 from the end". The index ^0 means "the end", or the index that follows the last element.

//You can specify a range with the range operator: ... For example, 0..^0 specifies the entire range 
//of the array: 0 from the start up to, but not including 0 from the end. Either operand may use 
//"from the start" or "from the end". Furthermore, either operand may be omitted. The defaults 
//are 0 for the start index, and ^0 for the end index.

// Let's look at a few examples. Consider the following array, 
// annotated with its index from the start and from the end:

var words = new string[]
{
                // index from start    index from end
    "The",      // 0                   ^9
    "quick",    // 1                   ^8
    "brown",    // 2                   ^7
    "fox",      // 3                   ^6
    "jumped",   // 4                   ^5
    "over",     // 5                   ^4
    "the",      // 6                   ^3
    "lazy",     // 7                   ^2
    "dog"       // 8                   ^1
};

//The index of each element reinforces the concept of "from the start", 
//and "from the end", and that ranges are exclusive of the end of the range. 
//The "start" of the entire array is the first element. 
//The "end" of the entire array is past the last element.
//You can retrieve the last word with the ^1 index:
Console.WriteLine($"The last word is {words[^1]}");
// writes "dog"


//The following code creates a subrange with the words "quick", "brown", and "fox". It includes words[1] through words[3]. The element words[4] is not in the range.
var quickBrownFox = words[1..4];


//The following code creates a subrange with "lazy" and "dog". It includes words[^2] and words[^1]. The end index words[^0] is not included:
var lazyDog = words[^2..^0];


//The following examples create ranges that are open ended for the start, end, or both:
var allWords = words[..]; // contains "The" through "dog".
var firstPhrase = words[..4]; // contains "The" through "fox"
var lastPhrase = words[6..]; // contains "the, "lazy" and "dog"


//Declaring ranges as variables:
Range phrase = 1..4;


//The range can then be used inside the [and] characters:
var text = words[phrase];