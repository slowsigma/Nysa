namespace Nysa.gexf;

public record Graph(ModeTypes ModeType, EdgeTypes DefaultEdgeType, IReadOnlyList<Node> Nodes, IReadOnlyList<Edge> Edges);
