using System;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.CodeAnalysis.VbScript.Semantics;
using Nysa.Logics;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed class SemanticTreeControlListener : Listener<String>
{
    public Int32            Version { get; private set; }
    public Token[]          Tokens  { get; private set; }
    public String           Code    { get; private set; }
    public Option<Program>  Root    { get; private set; }

    public SemanticTreeControlListener()
        : base()
    {
        this.Version    = -1;
        this.Tokens     = new Token[] { };
        this.Code       = String.Empty;
        this.Root       = Option.None;
    }

    protected override void OnValueChanged(string value, int version)
    {
        this.Version = version;
        this.Tokens = Language.Lex(value);
        this.Code = value;
        this.Root = (new VbScriptContent("Sample.vbs", String.Empty, this.Code))
                        .Parse()
                        .SemanticRoot
                        .Match(r => r.Some(), e => Option<Program>.None);
    }

}