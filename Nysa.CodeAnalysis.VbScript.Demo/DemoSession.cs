using System;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public record DemoSession(
    String BasePath,
    Grammar Grammar,
    VbScriptColorKey ColorKey,
    String VbScriptsPath,
    String BackgroundsPath,
    String GrammarPath,
    String OtherCodePath
);