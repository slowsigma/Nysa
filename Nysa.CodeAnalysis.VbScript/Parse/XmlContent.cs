using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XmlContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}