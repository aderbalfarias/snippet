// Command interface
public interface ICommand
{
    void Execute();
}

// Concrete command classes
public class LightOnCommand : ICommand
{
    private readonly Light _light;

    public LightOnCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
    {
        _light.TurnOn();
    }
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;

    public LightOffCommand(Light light)
    {
        _light = light;
    }

    public void Execute()
    {
        _light.TurnOff();
    }
}

// Receiver class
public class Light
{
    public void TurnOn()
    {
        Console.WriteLine("Light turned on");
    }

    public void TurnOff()
    {
        Console.WriteLine("Light turned off");
    }
}

// Invoker class
public class RemoteControl
{
    private ICommand _command;

    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void PressButton()
    {
        _command.Execute();
    }
}

// Client code
class Program
{
    static void Main(string[] args)
    {
        // Create receiver
        Light light = new Light();

        // Create concrete command objects and bind them to the receiver
        ICommand lightOnCommand = new LightOnCommand(light);
        ICommand lightOffCommand = new LightOffCommand(light);

        // Create invoker
        RemoteControl remoteControl = new RemoteControl();

        // Set commands to the invoker
        remoteControl.SetCommand(lightOnCommand);

        // Press the button to execute the command
        remoteControl.PressButton();

        // Change the command
        remoteControl.SetCommand(lightOffCommand);

        // Press the button again to execute the new command
        remoteControl.PressButton();
    }
}
