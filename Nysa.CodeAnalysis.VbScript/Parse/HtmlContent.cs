using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public record HtmlContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}