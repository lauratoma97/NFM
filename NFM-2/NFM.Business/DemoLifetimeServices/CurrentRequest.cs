namespace NFM.Business.DemoLifetimeServices;

public class CurrentRequest
{
    private static int count = 0;
    public Node Node { get; }

    public CurrentRequest(ServerConnectionDb server, TransientA a)
    {
        count++;
        Node = new Node()
        {
            Name = $"{nameof(CurrentRequest)}_{count}",
            Children =
            [
                server.Node,
                a.Node
            ]
        };
        Console.WriteLine($"Constructor of: {nameof(CurrentRequest)} called {count}");
    }

}