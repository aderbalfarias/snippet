<Query Kind="Expression" />

//Observer: A way of notifying change to a number of classes. 
//Define a one-to-many dependency between objects so that when one object 
//changes state, all its dependents are notified and updated automatically.


/// <summary>
/// MainApp startup class for Structural 
/// Observer Design Pattern.
/// </summary>
class MainApp
{
    /// <summary>
    /// Entry point into console application.
    /// </summary>
    static void Main()
    {
        //Configure Observer pattern
        ConcreteSubject s = new ConcreteSubject();

        s.Attach(new ConcreteObserver(s, "X"));
        s.Attach(new ConcreteObserver(s, "Y"));
        s.Attach(new ConcreteObserver(s, "Z"));

        //Change subject and notify observers
        s.SubjectState = "ABC";
        s.Notify();
    }
}

/// <summary>
/// The 'Subject' abstract class
/// </summary>
abstract class Subject
{
    private List<Observer> _observers = new List<Observer>();

    public void Attach(Observer observer)
    {
        _observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer o in _observers)
        {
            o.Update();
        }
    }
}

/// <summary>
/// The 'ConcreteSubject' class
/// </summary>
class ConcreteSubject : Subject
{
    private string _subjectState;

    //Gets or sets subject state
    public string SubjectState
    {
        get { return _subjectState; }
        set { _subjectState = value; }
    }
}

/// <summary>
/// The 'Observer' abstract class
/// </summary>
abstract class Observer
{
    public abstract void Update();
}

/// <summary>
/// The 'ConcreteObserver' class
/// </summary>
class ConcreteObserver : Observer
{
    private string _name;
    private string _observerState;
    private ConcreteSubject _subject;
    
    public ConcreteObserver(ConcreteSubject subject, string name)
    {
        this._subject = subject;
        this._name = name;
    }

    public override void Update()
    {
        _observerState = _subject.SubjectState;
        Console.WriteLine($"Observer {_name}'s new state is {_observerState}");
    }

    //Gets or sets subject
    public ConcreteSubject Subject
    {
        get { return _subject; }
        set { _subject = value; }
    }
}