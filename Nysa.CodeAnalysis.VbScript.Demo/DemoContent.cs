using System;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public record DemoContent(
    IReadOnlyList<DemoPage> Pages,
    Observable<String> SampleVbScript
);