using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public record AspxContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}