using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
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

    public static FlowDocument BulletPointDocument(String title, IReadOnlyList<BulletPoint> points, Int32 pointCount)
    {
        var document = new FlowDocument();
        var paragraph = new Paragraph(new Run(title)) { FontSize = 20.0 };
        var pointList = new System.Windows.Documents.List();

        pointList.MarkerStyle = System.Windows.TextMarkerStyle.Disc;

        document.Blocks.Add(paragraph);

        for (var i = 0; i < pointCount; i++)
        {
            var bulletPara = new Paragraph(new Run(points[i].Text));
            bulletPara.FontSize = points[i].IsImportant ? 24.0 : 20.0;

            var bulletItem = new System.Windows.Documents.ListItem(bulletPara);
            bulletItem.Margin = new System.Windows.Thickness(0, 0, 0, 10);

            pointList.ListItems.Add(bulletItem);
        }

        document.Blocks.Add(pointList);

        return document;
    }

    public static DemoPage BulletPointPage(this DemoSession @this, FlowDocument document)
    {
        var bulletControl = new BulletPointControl();
        var bulletControlVM = new BulletPointControlViewModel((FlowDocumentScrollViewer)bulletControl.FindName("_ScrollViewer"), document);

        return new DemoPageControl(null, bulletControl.Bound(bulletControlVM), null, null);
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

        demoPages.Add(semanticTree.Page);

        demoPages.Add(images.SlideExternalSymbols);

        demoPages.Add(images.SlideJavaScript);

        demoPages.Add(images.SlideTrivia);

        demoPages.Add(images.SlideTypeData);

        // Callenges
        var challengePoints = new List<BulletPoint>
        {
            new BulletPoint("VB Script feeding into 'execute' and 'eval' not translated."),
            new BulletPoint("Correctly picking property/method member where classes define the same name."),
            new BulletPoint("Default properties (can appear as array access or global function)."),
            new BulletPoint("VB Script array access is identical to a method call."),
            new BulletPoint("Correctly fixing async call chains."),
            new BulletPoint("Subtle differences in null, empty, and undefined.")
        };

        var challengeZer = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 0));
        var challengeOne = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 1));
        var challengeTwo = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 2));
        var challengeThr = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 3));
        var challengeFou = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 4));
        var challengeFiv = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 5));
        var challengeSix = @this.BulletPointPage(BulletPointDocument("Translation challenges included...", challengePoints, 6));
        
        demoPages.Add(challengeZer);
        demoPages.Add(challengeOne);
        demoPages.Add(challengeTwo);
        demoPages.Add(challengeThr);
        demoPages.Add(challengeFou);
        demoPages.Add(challengeFiv);
        demoPages.Add(challengeSix);

        // Results
        var resultPoints = new List<BulletPoint>
        {
            new BulletPoint("Over 2.1 million lines of VB Script translated."),
            new BulletPoint("Included logic to fix non-standard HTML to get to HTML5."),
            new BulletPoint("An attempt was made to see if A.I. might help with translation challenges, but that did not bear fruit."),
            new BulletPoint("The remaining work was distilled to a list of known issues to fix in-house and with contractors."),
            new BulletPoint("Tech stack now more attractive to quality talent.", true),
            new BulletPoint("Enterprise Justice Suite catastrophe averted.", true)
        };

        var resultsBulletsZer = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 0));
        var resultsBulletsOne = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 1));
        var resultsBulletsTwo = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 2));
        var resultsBulletsThr = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 3));
        var resultsBulletsFou = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 4));
        var resultsBulletsFiv = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 5));
        var resultsBulletsSix = @this.BulletPointPage(BulletPointDocument("Results...", resultPoints, 6));

        demoPages.Add(resultsBulletsZer);
        demoPages.Add(resultsBulletsOne);
        demoPages.Add(resultsBulletsTwo);
        demoPages.Add(resultsBulletsThr);
        demoPages.Add(resultsBulletsFou);
        demoPages.Add(resultsBulletsFiv);
        demoPages.Add(resultsBulletsSix);

        // My Background
        var introPoints = new List<BulletPoint>
        {
            new BulletPoint("Youngest of five boys."),
            new BulletPoint("First grade remedial reading/spelling."),
            new BulletPoint("Below average high school GPA."),
            new BulletPoint("Bachelors degree in: nothing."),
            new BulletPoint("Initially failed USAF exam for programming aptitude.")
        };

        var introBulletsZer = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 0));
        var introBulletsOne = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 1));
        var introBulletsTwo = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 2));
        var introBulletsThr = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 3));
        var introBulletsFou = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 4));
        var introBulletsFiv = @this.BulletPointPage(BulletPointDocument("My Background...", introPoints, 5));

        demoPages.Add(introBulletsZer);
        demoPages.Add(introBulletsOne);
        demoPages.Add(introBulletsTwo);
        demoPages.Add(introBulletsThr);
        demoPages.Add(introBulletsFou);
        demoPages.Add(introBulletsFiv);

        // Questions
        var questionPoints = new List<BulletPoint>
        {
        };

        var questionBulletsZer = @this.BulletPointPage(BulletPointDocument("Questions...", questionPoints, 0));

        demoPages.Add(questionBulletsZer);
        
        // demoPages.Add(new DemoPageControl(null, new ConclusionsControl(), null, null));

        return new DemoContent(demoPages, sampleCode);
    }

}