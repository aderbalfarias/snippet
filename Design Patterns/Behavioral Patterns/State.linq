<Query Kind="Expression" />

//State: Alter an object's behavior when its state changes. 
//Allow an object to alter its behavior when its internal state changes. 
//The object will appear to change its class.


/// <summary>
/// MainApp startup class for Structural
/// State Design Pattern.
/// </summary>
class MainApp
{
    /// <summary>
    /// Entry point into console application.
    /// </summary>
    static void Main()
    {
        //Setup context in a state
        Context c = new Context(new ConcreteStateA());

        //Issue requests, which toggles state
        c.Request();
        c.Request();
        c.Request();
        c.Request();
    }
}

/// <summary>
/// The 'State' abstract class
/// </summary>
abstract class State
{
    public abstract void Handle(Context context);
}

/// <summary>
/// A 'ConcreteState' class
/// </summary>
class ConcreteStateA : State
{
    public override void Handle(Context context)
    {
        context.State = new ConcreteStateB();
    }
}

/// <summary>
/// A 'ConcreteState' class
/// </summary>
class ConcreteStateB : State
{
    public override void Handle(Context context)
    {
        context.State = new ConcreteStateA();
    }
}

/// <summary>
/// The 'Context' class
/// </summary>
class Context
{
    private State _state;
    
    public Context(State state)
    {
        this.State = state;
    }

    //Gets or sets the state
    public State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Console.WriteLine($"State: {_state.GetType().Name}");
        }
    }

    public void Request()
    {
        _state.Handle(this);
    }
}