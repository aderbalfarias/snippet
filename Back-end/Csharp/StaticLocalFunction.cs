// Static local functions
// You can now add the static modifier to local functions to ensure that local 
// function doesn't capture (reference) any variables from the enclosing scope. 
// Consider the following code. The local function LocalFunction accesses the variable y, 
// declared in the enclosing scope (the method M). Therefore, LocalFunction 
// can't be declared with the static modifier:

void Main()
{
    StaticLocalFunction();
}

int StaticLocalFunction()
{
    int y = 5;
    int x = 7;
    return Add(x, y);

    static int Add(int left, int right) => left + right;
}