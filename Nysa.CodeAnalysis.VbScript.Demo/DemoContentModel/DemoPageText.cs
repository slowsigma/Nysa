using System;
using System.Windows.Controls;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed record DemoPageText(
    String? PageTitle,
    TextBox Text
) : DemoPage(PageTitle);
