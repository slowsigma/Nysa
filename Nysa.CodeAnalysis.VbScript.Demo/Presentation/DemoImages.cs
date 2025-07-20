using System;
using System.IO;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class DemoImages
{
    public DemoPageImage Repos { get; init; }
    public DemoPageImage RescriptExample { get; init; }
    public DemoPageImage SlideTitle { get; init; }
    public DemoPageImage SlideCodeToData { get; init; }
    public DemoPageImage SlideCompilers { get; init; }
    public DemoPageImage SlideCompilerTerms { get; init; }
    public DemoPageImage SlideSourceCode { get; init; }
    public DemoPageImage SlideSyntax { get; init; }
    public DemoPageImage SlideTokenStream { get; init; }
    public DemoPageImage SlideParseTree { get; init; }
    public DemoPageImage SlideSemanticTree { get; init; }
    public DemoPageImage SlideRoundTrip { get; init; }
    public DemoPageImage SlideRoundTripCompare { get; init; }
    public DemoPageImage SlideCodeSymbols { get; init; }
    public DemoPageImage SlideExternalSymbols { get; init; }
    public DemoPageImage SlideJavaScript { get; init; }
    public DemoPageImage SlideTrivia { get; init; }
    public DemoPageImage SlideTypeData { get; init; }

    public DemoImages(DemoSession session)
    {
        this.Repos = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "Repos.png"));
        this.RescriptExample = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "Rescript_Example.png"));
        this.SlideTitle = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_01.png"));
        this.SlideCodeToData = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_02.png"));
        this.SlideCompilers = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_03.png"));
        this.SlideCompilerTerms = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_04.png"));
        this.SlideSourceCode = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_05.png"));
        this.SlideSyntax = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_06.png"));
        this.SlideTokenStream = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_07.png"));
        this.SlideParseTree = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_08.png"));
        this.SlideSemanticTree = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_09.png"));
        this.SlideRoundTrip = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_10.png"));
        this.SlideRoundTripCompare = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_11.png"));
        this.SlideCodeSymbols = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_12.png"));
        this.SlideExternalSymbols = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_13.png"));
        this.SlideJavaScript = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_14.png"));
        this.SlideTrivia = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_15.png"));
        this.SlideTypeData = new DemoPageImage(null, Path.Combine(session.BackgroundsPath, "VbsToJs_16.png"));
    }

}

