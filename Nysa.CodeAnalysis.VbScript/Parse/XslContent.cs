using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public record XslContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}