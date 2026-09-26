using System.Diagnostics;

//State
public interface IState
{
    void Handle(ReactionMachine machine);
}
//Context
public class ReactionMachine
{
    private IState currentState;
    public ReactionMachine()
    {
        currentState = new ReadyState();
    }

    public void SetState(IState state)
    {
        currentState = state;
    }

    public void Run()
    {
        while (!(currentState is ResultState))
        {
            currentState.Handle(this);
        }

        currentState.Handle(this);
    }

}
//Concrete State 1: ReadyState
public class ReadyState : IState
{
    public void Handle(ReactionMachine machine)
    {
        Console.WriteLine("\n=== READY ===");
        Console.WriteLine("Press ENTER to start the reaction test.");

        Console.ReadLine();

        machine.SetState(new WaitingState());
    }
}
//Concrete State 2: WaitingState
public class WaitingState : IState
{
    public void Handle(ReactionMachine machine)
    {
        Console.WriteLine("\nGet ready...");
        Console.WriteLine("Wait for GO!");

        Random random = new Random();
        int delay = random.Next(2000, 5000);

        Thread.Sleep(delay);

        machine.SetState(new RunningState());
    }
}
//Concrete State 3: RunningState
public class RunningState : IState
{
    public void Handle(ReactionMachine machine)
    {
        Console.WriteLine("\nGO!");
        Console.WriteLine("Press ENTER as quickly as possible!");

        Stopwatch stopwatch = Stopwatch.StartNew();

        Console.ReadLine();

        stopwatch.Stop();

        double reactionTime = stopwatch.Elapsed.TotalSeconds;

        machine.SetState(new ResultState(reactionTime));
    }
}
//Concrete State 4: ResultState
public class ResultState : IState
{
    private readonly double reactionTime;

    public ResultState(double reactionTime)
    {
        this.reactionTime = reactionTime;
    }

    public void Handle(ReactionMachine machine)
    {
        Console.WriteLine($"\nYour reaction time was: {reactionTime:F3} seconds");

        Console.WriteLine("Game Over.");
    }
}
//Main method
class Program
{
    static void Main()
    {
        ReactionMachine machine = new ReactionMachine();

        machine.Run();

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}