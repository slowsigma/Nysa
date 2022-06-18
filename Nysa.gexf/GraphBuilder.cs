namespace Nysa.gexf;

public class GraphBuilder
{

    private EdgeTypes                           _Type;
    private Int32                               _NextNodeId;
    private Int32                               _NextEdgeId;
    private Dictionary<Int32, Node>             _Nodes;
    private Dictionary<Int32, Edge>             _Edges;
    private Dictionary<Int32, HashSet<Int32>>   _SourcesToTargets;

    public GraphBuilder()
    {
        this._NextNodeId        = 0;
        this._NextEdgeId        = 0;
        this._Nodes             = new Dictionary<Int32, Node>();
        this._Edges             = new Dictionary<Int32, Edge>();
        this._SourcesToTargets  = new Dictionary<Int32, HashSet<Int32>>();
    }

    public Node NewNode(String label)
    {
        var newId = this._NextNodeId++;

        this._Nodes.Add(newId, new Node(new NodeId(newId), label));

        return this._Nodes[newId];
    }

    public Edge? NewEdge(Node source, Node target)
    {
        if (!this._Nodes.ContainsKey(source.Id.Value) || !this._Nodes[source.Id.Value].Equals(source))
            throw new Exception("The source argument does not belong to this GraphBuilder.");
        if (!this._Nodes.ContainsKey(target.Id.Value) || !this._Nodes[target.Id.Value].Equals(target))
            throw new Exception("The target argument does not belong to this GraphBuilder.");

        if (this._Type == EdgeTypes.undirected)
        {
            if (source.Id.Value > target.Id.Value)
            {
                var hold = source;
                source = target;
                target = hold;
            }
        }

        if (!this._SourcesToTargets.ContainsKey(source.Id.Value))
            this._SourcesToTargets.Add(source.Id.Value, new HashSet<Int32>());

        if (!this._SourcesToTargets[source.Id.Value].Contains(target.Id.Value))
        {
            var newId = this._NextEdgeId++;

            this._Edges.Add(newId, new Edge(new EdgeId(newId), source.Id, target.Id));
            this._SourcesToTargets[source.Id.Value].Add(target.Id.Value);

            return this._Edges[newId];
        }
        else
        {
            return null;
        }
    }

    public Graph ToGraph(ModeTypes mode = ModeTypes.@static)
        => new Graph(mode, this._Type, this._Nodes.Values.ToList(), this._Edges.Values.ToList());
}