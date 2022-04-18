using System;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record VbScriptContent(String Source, String Hash, String Value) : Content(Source, Hash, Value);

}