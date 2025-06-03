using System;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed record DemoPageImage(
    String? PageTitle,
    String ImagePath
) : DemoPage(PageTitle);
