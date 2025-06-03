using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Nysa.ComponentModel;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class SyntaxControlViewModel : ModelObject
{
    private Dictionary<String, List<String>> _SyntaxRules;

    public ObservableCollection<TextBlock> Rules { get; private set; }

    private void Rules_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.ResetRules();
    }

    private void Run_MouseUp(object sender, MouseButtonEventArgs e)
    {
        var key = (sender as Run).Text;

        if (this._SyntaxRules.ContainsKey(key))
        {
            var text = ToTextBlock(key, this._SyntaxRules[key]);

            this.Rules.Add(text);
        }
    }

    private TextBlock ToTextBlock(String ruleKey, List<String> ruleParts)
    {
        var font = new FontFamily("Courier New");
        var deco = new TextDecorationCollection();
        var text = new TextBlock();

        deco.Add(System.Windows.TextDecorations.Underline);

        text.TextWrapping = TextWrapping.Wrap;
        text.Inlines.Add(new Run() { Text = ruleKey, FontFamily = font, FontSize = 18.0 });
        text.Inlines.Add(new Run() { Text = " ::=", FontFamily = font, FontSize = 18.0 });

        foreach (var part in ruleParts)
        {
            // just a spacer
            text.Inlines.Add(new Run() { Text = " ", FontFamily = font, FontSize = 16.0 });

            var run = new Run();

            run.Text = part;
            run.FontFamily = font;
            run.FontSize = 18.0;

            if (part.StartsWith("<") && part.EndsWith(">"))
            {
                run.TextDecorations = deco;
                run.Foreground = Brushes.Blue;
                run.MouseUp += this.Run_MouseUp;
            }
                
            text.Inlines.Add(run);
        }

        text.Margin = new Thickness(0.0, 0.0, 0.0, 8.0);

        return text;
    }

    private void ResetRules()
    {
        this.Rules.Clear();

        var program = ToTextBlock("<Program>", _SyntaxRules["<Program>"]);

        this.Rules.Add(program);
    }

    public SyntaxControlViewModel(String exePath, TextBlock resetBlock)
    {
        this._SyntaxRules = new Dictionary<String, List<String>>();
        this.Rules = new ObservableCollection<TextBlock>();

        var syntaxPath = System.IO.Path.Combine(exePath, "Grammar", "VBScript Grammar.txt");
        var syntaxText = File.ReadAllLines(syntaxPath);

        foreach (var line in syntaxText)
        {
            var rule = line.Split("::=");
            var key = rule[0].Trim();
            var def = rule[1].Trim();
            var parts = new List<String>();

            foreach (var part in def.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                parts.Add(part.Trim());

            this._SyntaxRules.Add(key, parts);
        }

        this.ResetRules();

        resetBlock.MouseUp += this.Rules_MouseUp;
    }
    
}