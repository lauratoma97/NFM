namespace NFM.Domain.DemoLifetime;

public class TransientA
{
    private static int count = 0;
    public Node Node { get; }

    public TransientA()
    {
        count++;
        Node = new Node()
        {
            Name = $"{nameof(TransientA)}_{count}",
        };
        Console.WriteLine($"Constructor of: {nameof(TransientA)} called {count}");
    }
}