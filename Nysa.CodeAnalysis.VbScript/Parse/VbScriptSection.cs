using System;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript
{

    public record VbScriptSection(String Source, String Value) : VbScriptContent(Source, String.Empty, Value);

}