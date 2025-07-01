namespace NFM.Domain.DemoLifetime;

public class ServerConnectionDb
{
    private static int count = 0;
    public Node Node { get; }

    public ServerConnectionDb(TransientA a)
    {
        count++;
        Node = new Node
        {
            Name = $"{nameof(ServerConnectionDb)}_{count}",
            Children = new List<Node>
            {
                a.Node
            }
        };
        Console.WriteLine($"Constructor of: {nameof(ServerConnectionDb)} called {count}");
    }
}