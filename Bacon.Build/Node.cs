namespace Bacon.Build;

internal class Node<T>(T data)
{
    private readonly HashSet<Node<T>> _edges = new();
    private int _incomingEdgeCount;

    public T Data { get; } = data;
    public bool HasIncomingEdge => _incomingEdgeCount > 0;

    public void AddEdgeTo(Node<T> node)
    {
        if (_edges.Add(node))
        {
            ++node._incomingEdgeCount;
        }
    }

    public int Remove(Stack<Node<T>> empty)
    {
        int count = 0;

        foreach (var node in _edges)
        {
            if (--node._incomingEdgeCount == 0)
            {
                ++count;
                empty.Push(node);
            }
        }

        return count;
    }
}