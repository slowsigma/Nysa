using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript;
using Nysa.ComponentModel;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public static class DemoSessionFunctions
{
    public static T Bound<T>(this T @this, ModelObject viewModel)
        where T : UserControl
    {
        @this.DataContext = viewModel;

        return @this;
    }

    public static UserControl ContextSetControl(this UserControl @this, ModelObject @object)
    {
        @this.DataContext = @object;

        return @this;
    }

    public static TextBox ToSourceCodeTextBox(this String @this)
    {
        var codeTextBox = new TextBox();
        
        codeTextBox.FontFamily = new System.Windows.Media.FontFamily("Courier New");
        codeTextBox.FontSize = 16.0;
        codeTextBox.IsReadOnly = true;
        codeTextBox.Text = @this;

        return codeTextBox;
    }

    public static DemoSession CreateSession(String basePath)
    {
        var vbsPath = Path.Combine(basePath, "VbScripts");
        var pngPath = Path.Combine(basePath, "Backgrounds");
        var grmPath = Path.Combine(basePath, "Grammar");
        var othPath = Path.Combine(basePath, "OtherCode");

        return new DemoSession(basePath, VbScript.Language.Grammar, new VbScriptColorKey(), vbsPath, pngPath, grmPath, othPath);
    }

    public static DemoPage RawVbScriptSyntaxPage(this DemoSession @this)
        => new DemoPageText("Raw VBScript Syntax", File.ReadAllText(Path.Combine(@this.GrammarPath, "VBScript Grammar.txt")).ToSourceCodeTextBox());

    public static DemoPage VbScriptSynaxDemoPage(this DemoSession @this)
    {
        var syntaxControl   = new SyntaxControl();
        var syntaxControlVM = new SyntaxControlViewModel(@this.BasePath, (TextBlock)syntaxControl.FindName("_Reset"));

        return new DemoPageControl("VBScript Rule Expansion", syntaxControl.Bound(syntaxControlVM), null, null);
    }

    public static DemoPage LexingDemoPage(this DemoSession @this, Observable<String> source)
    {
        var lexControl   = new LexingControl();
        var lexControlVM = new LexingControlViewModel(source);

        return new DemoPageControl(null, lexControl.Bound(lexControlVM), null, lexControlVM.OnLeave);
    }

    public static (DemoPage Page, Listener<String> Listener) TokenSourceTrackingDemoPage(this DemoSession @this)
    {
        var tknControl = new TokensControl();
        var tknControlVM = new TokensControlViewModel((RichTextBox)tknControl.FindName("_RichTextBox"), @this.ColorKey);

        return (new DemoPageControl("Tokens", tknControl.Bound(tknControlVM), tknControlVM.OnEnter, null),
                tknControlVM.Listener);
    }

    public static (DemoPage Page, Listener<String> Listener) ParseTreeDemoPage(this DemoSession @this)
    {
        var parseTreeControl   = new ParseTreeControl();
        var parseTreeControlVM = new ParseTreeControlViewModel((RichTextBox)parseTreeControl.FindName("_RichTextBox"), @this.ColorKey);

        return (new DemoPageControl("Abstract Parse/Syntax Tree", parseTreeControl.Bound(parseTreeControlVM), parseTreeControlVM.OnEnter, null), parseTreeControlVM.Listener);
    }

    public static (DemoPage Page, Listener<String> Listener) RescriptDemoPage(this DemoSession @this)
    {
        var rescriptControl = new RescriptControl();
        var rescriptControlVM = new RescriptControlViewModel((RichTextBox)rescriptControl.FindName("_RichTextBox"),
                                                             (RichTextBox)rescriptControl.FindName("_RichTextBoxRescript"),
                                                             @this.ColorKey);

        return (new DemoPageControl("Rescripting From Semantic Tree", rescriptControl.Bound(rescriptControlVM), rescriptControlVM.OnEnter, null), rescriptControlVM.Listener);
    }

    public static DemoPage GrammarInCSharpPage(this DemoSession @this)
    {
        var langScript = File.ReadAllText(Path.Combine(@this.OtherCodePath, "Language.cs.txt"));
        var langDoc = CSharpCodeFunctions.Colorize(langScript);
        var langControl = new CodeControl();
        var langViewModel = new CodeControlViewModel((RichTextBox)langControl.FindName("_RichTextBox"), langDoc);

        return new DemoPageControl("https://github.com/slowsigma/Nysa/blob/master/Nysa.CodeAnalysis.VbScript/Language.cs", langControl.Bound(langViewModel), null, null);
    }

    public static DemoPage RoundTripInCSharpPage(this DemoSession @this)
    {
        var vbGenScript = File.ReadAllText(Path.Combine(@this.OtherCodePath, "VbScript.cs.txt"));
        var vbGenDoc = CSharpCodeFunctions.Colorize(vbGenScript);
        var vbGenControl = new CodeControl();
        var vbGenViewModel = new CodeControlViewModel((RichTextBox)vbGenControl.FindName("_RichTextBox"), vbGenDoc);

        return new DemoPageControl("https://github.com/slowsigma/Nysa/blob/master/Nysa.CodeAnalysis.VbScript/Rescript/VbScript.cs", vbGenControl.Bound(vbGenViewModel), null, null);
    }

    public static (DemoPage Page, Listener<String> Listener) ChartBuildingPage(this DemoSession @this)
    {
        var chartControl = new ChartControl();
        var chartControlVM = new ChartControlViewModel();

        return (new DemoPageControl("Chart Building", chartControl.Bound(chartControlVM), chartControlVM.OnEnter, chartControlVM.OnLeave), chartControlVM.Listener);
    }

    public static (DemoPage Page, Listener<String> Listener) SemanticTreeDemoPage(this DemoSession @this)
    {
        var treeControl   = new SemanticTreeControl();
        var treeControlVM = new SemanticTreeControlViewModel((RichTextBox)treeControl.FindName("_RichTextBox"), @this.ColorKey);

        return new(new DemoPageControl("Semantic Tree", treeControl.Bound(treeControlVM), treeControlVM.OnEnter, null), treeControlVM.Listener);
    }

    public static DemoContent CreateContent(this DemoSession @this)
    {
        var demoPages = new List<DemoPage>();
        var images = new DemoImages(@this);


        // demoPages.Add(images.OregonTrailFull);
        // demoPages.Add(images.OregonTrailZoomOne);
        // demoPages.Add(images.OregonTrailZoomTwo);


        // tokens with matching colorized code
        var tokenTracking = @this.TokenSourceTrackingDemoPage();
        var parseTree = @this.ParseTreeDemoPage();
        var parseChart = @this.ChartBuildingPage();
        var semanticTree = @this.SemanticTreeDemoPage();
        var rescriptTree = @this.RescriptDemoPage();


        var sampleCode = File.ReadAllText(Path.Combine(@this.VbScriptsPath, "Sample.vbs"))
                             .ToObservable(tokenTracking.Listener,
                                           parseTree.Listener,
                                           parseChart.Listener,
                                           semanticTree.Listener,
                                           rescriptTree.Listener);

        // click bait title
        demoPages.Add(new DemoPageControl(null, (new MistakeControl()).ContextSetControl(new MistakeControlViewModel()), null, null));

        demoPages.Add(images.SlideTitle);
        demoPages.Add(images.SlideCodeToData);
        demoPages.Add(images.SlideCompilers);
        demoPages.Add(images.SlideCompilerTerms);

        demoPages.Add(new DemoPageText("Example VB Script", sampleCode.Value.ToSourceCodeTextBox()));

        demoPages.Add(images.SlideSyntax);

        demoPages.Add(@this.RawVbScriptSyntaxPage());
        demoPages.Add(@this.VbScriptSynaxDemoPage());

        demoPages.Add(@this.GrammarInCSharpPage());

        demoPages.Add(images.SlideTokenStream);

        demoPages.Add(@this.LexingDemoPage(sampleCode));

        demoPages.Add(tokenTracking.Page);

        demoPages.Add(images.SlideParseTree);

        demoPages.Add(parseChart.Page);

        demoPages.Add(images.SlideParseTree);

        demoPages.Add(parseTree.Page);

        demoPages.Add(images.SlideSemanticTree);

        demoPages.Add(semanticTree.Page);

        demoPages.Add(images.SlideRoundTrip);

        demoPages.Add(@this.RoundTripInCSharpPage());

        demoPages.Add(images.SlideRoundTripCompare);

        demoPages.Add(rescriptTree.Page);

        demoPages.Add(images.SlideCodeSymbols);

        demoPages.Add(images.SlideExternalSymbols);

        demoPages.Add(images.SlideJavaScript);

        demoPages.Add(images.SlideTrivia);

        demoPages.Add(images.SlideTypeData);

        demoPages.Add(new DemoPageControl("Results", new ResultsControl(), null, null));

        return new DemoContent(demoPages, sampleCode);
    }

}