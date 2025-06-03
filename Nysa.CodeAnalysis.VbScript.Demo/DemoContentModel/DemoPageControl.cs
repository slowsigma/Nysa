using System;
using System.Windows.Controls;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed record DemoPageControl(
    String? PageTitle,
    UserControl Control,
    Action? OnEnter,
    Action? OnLeave
) : DemoPage(PageTitle);
