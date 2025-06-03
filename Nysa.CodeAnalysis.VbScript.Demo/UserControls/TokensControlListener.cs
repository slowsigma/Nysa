using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

using Nysa.CodeAnalysis.Documents;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public sealed class TokensControlListener : Listener<String>
{
    public Int32    Version { get; private set; }
    public Token[]  Tokens  { get; private set; }
    public String   Code    { get; private set; }

    public TokensControlListener()
        : base()
    {
        this.Version = -1;
        this.Tokens = new Token[] { };
        this.Code = String.Empty;
    }


    protected override void OnValueChanged(string value, Int32 version)
    {
        this.Version = version;
        this.Tokens = Nysa.CodeAnalysis.VbScript.Language.Lex(value);
        this.Code = value;
    }

}