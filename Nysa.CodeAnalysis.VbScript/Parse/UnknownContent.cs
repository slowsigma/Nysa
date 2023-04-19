using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public record UnknownContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}