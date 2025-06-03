using System;

using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class ChartControlListener : Listener<String>
{
    public Int32        Version { get; private set; }
    public Token[]      Tokens  { get; private set; }
    public String       Code    { get; private set; }

    public ChartControlListener()
        : base()
    {
        this.Version    = -1;
        this.Tokens     = new Token[] { };
        this.Code       = String.Empty;
    }

    protected override void OnValueChanged(string value, int version)
    {
        this.Version = version;
        this.Tokens = Nysa.CodeAnalysis.VbScript.Language.Lex(value);
        this.Code = value;
    }
}