<Query Kind="Program" />

//Memento: Capture and restore an object's internal state. 
//Without violating encapsulation, capture and externalize an object’s 
//internal state so that the object can be restored to this state later.

/// <summary>
/// MainApp startup class for Structural 
/// Memento Design Pattern.
/// </summary>
class MainApp
{
    /// <summary>
    /// Entry point into console application.
    /// </summary>
    static void Main()
    {
        Originator o = new Originator();
        o.State = "On";

        //Store internal state
        Caretaker c = new Caretaker();
        c.Memento = o.CreateMemento();

        //Continue changing originator
        o.State = "Off";

        //Restore saved state
        o.SetMemento(c.Memento);
    }
}

/// <summary>
/// The 'Originator' class
/// </summary>
class Originator
{
    private string _state;

    //Property
    public string State
    {
        get { return _state; }
        set
        {
            _state = value;
            Console.WriteLine($"State = {_state}");
        }
    }

    //Creates memento 
    public Memento CreateMemento()
    {
        return (new Memento(_state));
    }

    //Restores original state
    public void SetMemento(Memento memento)
    {
        Console.WriteLine("Restoring state...");
        State = memento.State;
    }
}

/// <summary>
/// The 'Memento' class
/// </summary>
class Memento
{
    private string _state;
    
    public Memento(string state)
    {
        this._state = state;
    }

    //Gets or sets state
    public string State
    {
        get { return _state; }
    }
}

/// <summary>
/// The 'Caretaker' class
/// </summary>
class Caretaker
{
    private Memento _memento;

    //Gets or sets memento
    public Memento Memento
    {
        set { _memento = value; }
        get { return _memento; }
    }
}