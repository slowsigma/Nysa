using System.Xml.Linq;

namespace Nysa.gexf;

public static class XmlExtensions
{
    private static readonly String UriGexf  = "http://gexf.net/1.3";
    private static readonly String UriXsi   = "http://www.w3.org/2001/XMLSchema-instance";
    private static readonly XNamespace NsGexf   = (XNamespace)UriGexf;
    private static readonly XNamespace NsXsi    = (XNamespace)UriXsi;

    private static XElement ToXml(this Meta @this)
        => new XElement("meta", new XAttribute("lastmodifieddate", @this.LastModified.ToString("yyyy-MM-dd")),
                                new XElement("creator", new XText(@this.Creator)),
                                new XElement("description", new XText(@this.Description)));

    public static XElement ToXml(this Node @this)
        => new XElement("node", new XAttribute("id", @this.Id.Value.ToString()),
                                new XAttribute("label", @this.Label));

    public static XElement ToXml(this Edge @this)
        => new XElement("edge", new XAttribute("id", @this.Id.Value.ToString()),
                                new XAttribute("source", @this.Source.Value.ToString()),
                                new XAttribute("target", @this.Target.Value.ToString()));

    public static XElement ToXml(this Graph @this)
        => new XElement("graph", new XElement("nodes", @this.Nodes.Select(n => n.ToXml())),
                                 new XElement("edges", @this.Edges.Select(e => e.ToXml())));

    public static XDocument ToXml(this Meta @this, Graph graph)
        => new XDocument(new XDeclaration("1.0", "UTF-8", null),
                         new XElement(NsGexf + "gexf",
                                      new XAttribute("xmlns", UriGexf),
                                      new XAttribute(XNamespace.Xmlns + "xsi", UriXsi),
                                      new XAttribute(NsXsi + "schemaLocation", "http://gexf.net/1.3 http://gexf.net/1.3/gexf.xsd"),
                                      new XAttribute("version", "1.3"),
                                      @this.ToXml(),
                                      graph.ToXml()));

}