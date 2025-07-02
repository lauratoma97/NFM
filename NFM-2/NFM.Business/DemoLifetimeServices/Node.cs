namespace NFM.Business.DemoLifetimeServices;

public class Node
{
    public string Name { get; set; }
    public List<Node> Children { get; set; } = null;
}