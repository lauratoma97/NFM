namespace NFM.Domain.DemoLifetime;

public class TransientB
{
    private static int count = 0;
    public Node Node { get; }

    public TransientB(CurrentRequest currentRequest, ServerConnectionDb server, TransientA transient2)
    {
        count++;
        Node = new Node()
        {
            Name = $"{nameof(TransientB)}_{count}",
            Children =
            [
                currentRequest.Node,
                server.Node,
                transient2.Node
            ]
        };
        Console.WriteLine($"Constructor of: {nameof(TransientB)} called {count}");
    }
}