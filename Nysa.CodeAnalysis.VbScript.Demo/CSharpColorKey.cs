using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Microsoft.CodeAnalysis.CSharp;

namespace Nysa.CodeAnalysis.VbScript.Demo;

public class CSharpColorKey
{
    private HashSet<SyntaxKind> _CommentKinds;
    private Dictionary<SyntaxKind, SolidColorBrush> _Key;
    private SolidColorBrush _DefaultColor;

    public Boolean IsComment(SyntaxKind kind) => this._CommentKinds.Contains(kind);

    public Boolean Contains(SyntaxKind kind) => this._Key.ContainsKey(kind);

    public SolidColorBrush GetColor(SyntaxKind kind) => this._Key.ContainsKey(kind) ? this._Key[kind] : this._DefaultColor;

    public CSharpColorKey(SolidColorBrush? defaultColor)
    {
        this._CommentKinds = new HashSet<SyntaxKind>();
        this._Key = new Dictionary<SyntaxKind, SolidColorBrush>();

        this._DefaultColor = defaultColor ?? Brushes.Black;

        this._CommentKinds.Add(SyntaxKind.MultiLineCommentTrivia);
        this._CommentKinds.Add(SyntaxKind.SingleLineCommentTrivia);
        this._CommentKinds.Add(SyntaxKind.MultiLineDocumentationCommentTrivia);
        this._CommentKinds.Add(SyntaxKind.SingleLineDocumentationCommentTrivia);

        this._Key.Add(SyntaxKind.BoolKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ByteKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.SByteKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ShortKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UShortKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.IntKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UIntKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.LongKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ULongKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DoubleKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FloatKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DecimalKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.StringKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.CharKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.VoidKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ObjectKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.TypeOfKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.SizeOfKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NullKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.TrueKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FalseKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.LockKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.PublicKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.PrivateKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.InternalKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ProtectedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.StaticKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ReadOnlyKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.SealedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ConstKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FixedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.StackAllocKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.VolatileKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NewKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OverrideKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AbstractKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.VirtualKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EventKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ExternKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RefKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OutKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.IsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ParamsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ArgListKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.MakeRefKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RefTypeKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RefValueKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ThisKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.BaseKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NamespaceKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UsingKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ClassKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.StructKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.InterfaceKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EnumKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DelegateKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.CheckedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UncheckedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UnsafeKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OperatorKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ExplicitKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ImplicitKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.PartialKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AliasKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.GlobalKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AssemblyKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ModuleKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.TypeKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FieldKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.MethodKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ParamKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.PropertyKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.TypeVarKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.GetKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.SetKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AddKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RemoveKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.WhereKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FromKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.GroupKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.JoinKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.IntoKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.LetKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ByKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.SelectKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OrderByKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OnKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EqualsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AscendingKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DescendingKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NameOfKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AsyncKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.OrKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AndKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NotKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.WithKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.InitKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RecordKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ManagedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UnmanagedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RequiredKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ScopedKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.FileKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AllowsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.VarKeyword, Brushes.Blue);

        // preprocessor directives
        this._Key.Add(SyntaxKind.ElifKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EndIfKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RegionKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EndRegionKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DefineKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.UndefKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.WarningKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ErrorKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.LineKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.PragmaKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.HiddenKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ChecksumKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.DisableKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.RestoreKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.ReferenceKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.LoadKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.NullableKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.EnableKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.WarningsKeyword, Brushes.Blue);
        this._Key.Add(SyntaxKind.AnnotationsKeyword, Brushes.Blue);



        this._Key.Add(SyntaxKind.IfKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ElseKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.WhileKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ForKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ForEachKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.DoKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.SwitchKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.CaseKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.DefaultKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.TryKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.CatchKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.FinallyKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.GotoKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.BreakKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ContinueKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ReturnKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.ThrowKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.InKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.YieldKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.AwaitKeyword, Brushes.Purple);
        this._Key.Add(SyntaxKind.WhenKeyword, Brushes.Purple);


        this._Key.Add(SyntaxKind.InterpolatedStringStartToken, Brushes.Red);
        this._Key.Add(SyntaxKind.InterpolatedStringEndToken, Brushes.Red);
        this._Key.Add(SyntaxKind.InterpolatedVerbatimStringStartToken, Brushes.Red);

        this._Key.Add(SyntaxKind.IdentifierName, Brushes.DarkBlue);

        this._Key.Add(SyntaxKind.NumericLiteralToken, Brushes.Green);

        this._Key.Add(SyntaxKind.CharacterLiteralToken, Brushes.Red);
        this._Key.Add(SyntaxKind.StringLiteralToken, Brushes.Red);


        this._Key.Add(SyntaxKind.InterpolatedStringToken, Brushes.Red);
        this._Key.Add(SyntaxKind.InterpolatedStringTextToken, Brushes.Red);

        this._Key.Add(SyntaxKind.SingleLineRawStringLiteralToken, Brushes.Red);
        this._Key.Add(SyntaxKind.MultiLineRawStringLiteralToken, Brushes.Red);

        this._Key.Add(SyntaxKind.Utf8StringLiteralToken, Brushes.Red);
        this._Key.Add(SyntaxKind.Utf8SingleLineRawStringLiteralToken, Brushes.Red);
        this._Key.Add(SyntaxKind.Utf8MultiLineRawStringLiteralToken, Brushes.Red);


        /* trivia

                EndOfLineTrivia = 8539,
                WhitespaceTrivia = 8540,
                SingleLineCommentTrivia = 8541,
                MultiLineCommentTrivia = 8542,
                DocumentationCommentExteriorTrivia = 8543,
                SingleLineDocumentationCommentTrivia = 8544,
                MultiLineDocumentationCommentTrivia = 8545,
                DisabledTextTrivia = 8546,
                PreprocessingMessageTrivia = 8547,
                IfDirectiveTrivia = 8548,
                ElifDirectiveTrivia = 8549,
                ElseDirectiveTrivia = 8550,
                EndIfDirectiveTrivia = 8551,
                RegionDirectiveTrivia = 8552,
                EndRegionDirectiveTrivia = 8553,
                DefineDirectiveTrivia = 8554,
                UndefDirectiveTrivia = 8555,
                ErrorDirectiveTrivia = 8556,
                WarningDirectiveTrivia = 8557,
                LineDirectiveTrivia = 8558,
                PragmaWarningDirectiveTrivia = 8559,
                PragmaChecksumDirectiveTrivia = 8560,
                ReferenceDirectiveTrivia = 8561,
                BadDirectiveTrivia = 8562,
                SkippedTokensTrivia = 8563,
                ConflictMarkerTrivia = 8564,

        */

    }
    
}