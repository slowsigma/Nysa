using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.Text.Parsing;

public static partial class VBScriptX
{
    public static readonly String END_OF_INPUT = "{EOI}";

    public static readonly Grammar Grammar;
    public static readonly SeekRule Seek;

    private static readonly HashSet<Identifier> _AlwaysRemove;
    private static readonly HashSet<Identifier> _ListCollapse;
    private static readonly HashSet<Identifier> _SingleItemCollapse;

    private static readonly HashSet<Identifier> _EmptyNodeRemove;

    static VBScriptX()
    {
        var builder = new Grammar.Builder("<Program>");

        builder.Rule("<Program>").Is("<NLOpt>", "<GlobalStmtList>", END_OF_INPUT);

        builder.Rule("<InlineNL>").Is(":", "<InlineNL>")
                                  .Or(":");

        builder.Rule("<NL>").Is("{new-line}", "<NL>")
                            .Or("{new-line}")
                            .Or("<InlineNL>", "<NL>")
                            .Or("<InlineNL>");

        builder.Rule("<NLOpt>").Is("<NL>")
                               .OrOptional();

        builder.Rule("<GlobalStmtList>").Is("<GlobalStmt>", "<GlobalStmtList>")
                                        .OrOptional();

        builder.Rule("<GlobalStmt>").Is("<OptionExplicit>")
                                    .Or("<ClassDecl>")
                                    .Or("<FieldDecl>")
                                    .Or("<ConstDecl>")
                                    .Or("<SubDecl>")
                                    .Or("<FunctionDecl>")
                                    .Or("<BlockStmt>");

        builder.Rule("<OptionExplicit>").Is("Option", "Explicit", "<NL>");

        builder.Rule("<ClassDecl>").Is("Class", "<ExtendedID>", "<NL>", "<MemberDeclList>", "End", "Class", "<NL>");

        builder.Rule("<FieldDecl>").Is("Private", "<FieldName>", "<OtherVarsOpt>", "<NL>")
                                   .Or("Public", "<FieldName>", "<OtherVarsOpt>", "<NL>");

        builder.Rule("<ConstDecl>").Is("<AccessModifierOpt>", "Const", "<ConstList>", "<NL>");

        builder.Rule("<BlockConstDecl>").Is("Const", "<ConstList>", "<NL>");            // new

        builder.Rule("<SubDecl>").Is("<MethodAccessOpt>", "Sub", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Sub", "<NL>")
                                 .Or("<MethodAccessOpt>", "Sub", "<ExtendedID>", "<MethodArgList>", "<InlineStmt>", "End", "Sub", "<NL>");

        builder.Rule("<FunctionDecl>").Is("<MethodAccessOpt>", "Function", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Function", "<NL>")
                                      .Or("<MethodAccessOpt>", "Function", "<ExtendedID>", "<MethodArgList>", "<InlineStmt>", "End", "Function", "<NL>");

        builder.Rule("<BlockStmt>").Is("<VarDecl>")
                                   .Or("<RedimStmt>")
                                   .Or("<IfStmt>")
                                   .Or("<WithStmt>")
                                   .Or("<SelectStmt>")
                                   .Or("<LoopStmt>")
                                   .Or("<ForStmt>")
                                   .Or("<BlockConstDecl>")                          // new
                                   .Or("<InlineStmt>", "<NL>");

        builder.Rule("<ExtendedID>").Is("<SafeKeywordID>")
                                    .Or("{ID}");

        builder.Rule("<MemberDeclList>").Is("<MemberDecl>", "<MemberDeclList>")
                                        .OrOptional();

        builder.Rule("<FieldName>").Is("<FieldID>", "(", "<ArrayRankList>", ")")
                                   .Or("<FieldID>");

        builder.Rule("<OtherVarsOpt>").Is(",", "<VarName>", "<OtherVarsOpt>")
                                      .OrOptional();

        builder.Rule("<AccessModifierOpt>").Is("Public")
                                           .Or("Private")
                                           .OrOptional();

        builder.Rule("<ConstList>").Is("<ExtendedID>", "=", "<ConstExprDef>", ",", "<ConstList>")
                                   .Or("<ExtendedID>", "=", "<ConstExprDef>")
                                   .Or("<ExtendedID>", ",", "<ConstList>")          // new
                                   .Or("<ExtendedID>");                             // new

        builder.Rule("<MethodAccessOpt>").Is("Public", "Default")
                                         .Or("<AccessModifierOpt>");

        builder.Rule("<MethodArgList>").Is("(", "<ArgList>", ")")
                                       .Or("(", ")")
                                       .OrOptional();

        builder.Rule("<MethodStmtList>").Is("<MethodStmt>", "<MethodStmtList>")
                                        .OrOptional();

        builder.Rule("<InlineStmtList>").Is("<InlineStmt>", "<InlineNL>", "<InlineStmtList>")
                                       .Or("<InlineStmt>", "<InlineNL>")
                                       .Or("<InlineStmt>");

        builder.Rule("<InlineStmt>").Is("<AssignStmt>")
                                    .Or("<CallStmt>")
                                    .Or("<SubCallStmt>")
                                    .Or("<ErrorStmt>")
                                    .Or("<ExitStmt>")
                                    .Or("<InlineIfStmt>")
                                    .Or("Erase", "<ExtendedID>");

        builder.Rule("<SafeKeywordID>").Is("Default")
                                       .Or("Erase")
                                       .Or("Error")
                                       .Or("Explicit")
                                       .Or("Property")
                                       .Or("Step");

        builder.Rule("<MemberDecl>").Is("<FieldDecl>")
                                    .Or("<VarDecl>")
                                    .Or("<ConstDecl>")
                                    .Or("<SubDecl>")
                                    .Or("<FunctionDecl>")
                                    .Or("<PropertyDecl>");

        builder.Rule("<FieldID>").Is("{ID}")
                                 .Or("Default")
                                 .Or("Erase")
                                 .Or("Error")
                                 .Or("Explicit")
                                 .Or("Step");

        builder.Rule("<VarName>").Is("<ExtendedID>", "(", "<ArrayRankList>", ")")
                                 .Or("<ExtendedID>");

        builder.Rule("<ConstExprDef>").Is("(", "<ConstExprDef>", ")")
                                      .Or("-", "<ConstExprDef>")
                                      .Or("+", "<ConstExprDef>")
                                      .Or("<ConstExpr>");

        builder.Rule("<ArgList>").Is("<Arg>", ",", "<ArgList>")
                                 .Or("<Arg>");

        //builder.Rule("<MethodStmt>").Is("<ConstDecl>")
        //                            .Or("<BlockStmt>");
        builder.Rule("<MethodStmt>").Is("<BlockStmt>");                     // new

        builder.Rule("<AssignStmt>").Is("<LeftExpr>", "=", "<Expr>")
                                    .Or("Set", "<LeftExpr>", "=", "<Expr>");
        //.Or("Set", "<LeftExpr>", "=", "New", "<LeftExpr>");

        builder.Rule("<CallStmt>").Is("Call", "<LeftExpr>");

        builder.Rule("<SubCallStmt>").Is("<QualifiedID>", "<SubSafeExprOpt>", "<CommaExprList>")
                                     .Or("<QualifiedID>", "<SubSafeExprOpt>")
                                     .Or("<QualifiedID>", "(", "<Expr>", ")", "<CommaExprList>")
                                     .Or("<QualifiedID>", "(", "<Expr>", ")")
                                     .Or("<QualifiedID>", "(", ")")
                                     .Or("<QualifiedID>", "<IndexOrParamsList>", ".", "<LeftExprTail>", "<SubSafeExprOpt>", "<CommaExprList>")
                                     .Or("<QualifiedID>", "<IndexOrParamsListDot>", "<LeftExprTail>", "<SubSafeExprOpt>", "<CommaExprList>")
                                     .Or("<QualifiedID>", "<IndexOrParamsList>", ".", "<LeftExprTail>", "<SubSafeExprOpt>")
                                     .Or("<QualifiedID>", "<IndexOrParamsListDot>", "<LeftExprTail>", "<SubSafeExprOpt>");

        builder.Rule("<ErrorStmt>").Is("On", "Error", "Resume", "Next")
                                   .Or("On", "Error", "GoTo", "{IntLiteral}");

        builder.Rule("<ExitStmt>").Is("Exit", "Do")
                                  .Or("Exit", "For")
                                  .Or("Exit", "Function")
                                  .Or("Exit", "Property")
                                  .Or("Exit", "Sub");

        builder.Rule("<VarDecl>").Is("Dim", "<VarName>", "<OtherVarsOpt>", "<NL>");

        builder.Rule("<RedimStmt>").Is("Redim", "<RedimDeclList>", "<NL>")
                                   .Or("Redim", "Preserve", "<RedimDeclList>", "<NL>");

        builder.Rule("<RedimDeclList>").Is("<RedimDecl>", ",", "<RedimDeclList>")
                                       .Or("<RedimDecl>");

        builder.Rule("<RedimDecl>").Is("<ExtendedID>", "(", "<ExprList>", ")");

        builder.Rule("<IfStmt>").Is("If", "<Expr>", "Then", "<NL>", "<BlockStmtList>", "<ElseStmtList>", "End", "If", "<NL>");
        //                        .Or("If", "<Expr>", "Then", "<InlineStmt>", "<ElseOpt>", "<EndIfOpt>", "<NL>");

        // new
        builder.Rule("<InlineIfStmt>").Is("If", "<Expr>", "Then", "<InlineStmtList>", "<ElseOpt>", "<EndIfOpt>");

        builder.Rule("<ElseStmtList>").Is("ElseIf", "<Expr>", "Then", "<NL>", "<BlockStmtList>", "<ElseStmtList>")
                                      .Or("ElseIf", "<Expr>", "Then", "<InlineStmt>", "<NL>", "<ElseStmtList>")
                                      .Or("Else", "<InlineStmt>", "<NL>")
                                      .Or("Else", "<NL>", "<BlockStmtList>")
                                      .OrOptional();

        builder.Rule("<ElseOpt>").Is("Else", "<InlineStmtList>")
                                 .OrOptional();

        builder.Rule("<EndIfOpt>").Is("End", "If")
                                  .OrOptional();

        builder.Rule("<WithStmt>").Is("With", "<Expr>", "<NL>", "<BlockStmtList>", "End", "With", "<NL>");

        builder.Rule("<SelectStmt>").Is("Select", "Case", "<Expr>", "<NL>", "<CaseStmtList>", "End", "Select", "<NL>");

        builder.Rule("<CaseStmtList>").Is("Case", "<ExprList>", "<NLOpt>", "<BlockStmtList>", "<CaseStmtList>")
                                      .Or("Case", "Else", "<NLOpt>", "<BlockStmtList>")
                                      .OrOptional();

        builder.Rule("<ExprList>").Is("<Expr>", ",", "<ExprList>")
                                  .Or("<Expr>");

        builder.Rule("<LoopStmt>").Is("Do", "<LoopType>", "<Expr>", "<NL>", "<BlockStmtList>", "Loop", "<NL>")
                                  .Or("Do", "<NL>", "<BlockStmtList>", "Loop", "<LoopType>", "<Expr>", "<NL>")
                                  .Or("Do", "<NL>", "<BlockStmtList>", "Loop", "<NL>")
                                  .Or("While", "<Expr>", "<NL>", "<BlockStmtList>", "WEnd", "<NL>");

        builder.Rule("<LoopType>").Is("While")
                                  .Or("Until");

        builder.Rule("<ForStmt>").Is("For", "<ExtendedID>", "=", "<Expr>", "To", "<Expr>", "<StepOpt>", "<NL>", "<BlockStmtList>", "Next", "<NL>")
                                 .Or("For", "Each", "<ExtendedID>", "In", "<Expr>", "<NL>", "<BlockStmtList>", "Next", "<NL>");

        builder.Rule("<BlockStmtList>").Is("<BlockStmt>", "<BlockStmtList>")
                                       .OrOptional();

        builder.Rule("<StepOpt>").Is("Step", "<Expr>")
                                 .OrOptional();

        builder.Rule("<PropertyDecl>").Is("<MethodAccessOpt>", "Property", "<PropertyAccessType>", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Property", "<NL>");

        builder.Rule("<ArrayRankList>").Is("<IntLiteral>", ",", "<ArrayRankList>")
                                       .Or("<IntLiteral>")
                                       .OrOptional();

        builder.Rule("<ConstExpr>").Is("<BoolLiteral>")
                                   .Or("<IntLiteral>")
                                   .Or("{FloatLiteral}")
                                   .Or("{StringLiteral}")
                                   .Or("{DateLiteral}")
                                   .Or("<Nothing>");

        builder.Rule("<Arg>").Is("<ArgModifierOpt>", "<ExtendedID>", "(", ")")
                             .Or("<ArgModifierOpt>", "<ExtendedID>");

        builder.Rule("<NewObjectExpr>").Is("New", "<LeftExpr>");

        builder.Rule("<LeftExpr>").Is("<QualifiedID>", "<IndexOrParamsList>", ".", "<LeftExprTail>")
                                  .Or("<QualifiedID>", "<IndexOrParamsListDot>", "<LeftExprTail>")
                                  .Or("<QualifiedID>", "<IndexOrParamsList>")
                                  .Or("<QualifiedID>")
                                  .Or("<NewObjectExpr>")
                                  .Or("<SafeKeywordID>");

        builder.Rule("<Expr>").Is("<ImpExpr>");

        builder.Rule("<ImpExpr>").Is("<ImpExpr>", "Imp", "<EqvExpr>")
                                 .Or("<EqvExpr>");

        builder.Rule("<EqvExpr>").Is("<EqvExpr>", "Eqv", "<XorExpr>")
                                 .Or("<XorExpr>");

        builder.Rule("<XorExpr>").Is("<XorExpr>", "Xor", "<OrExpr>")
                                 .Or("<OrExpr>");

        builder.Rule("<OrExpr>").Is("<OrExpr>", "Or", "<AndExpr>")
                                .Or("<AndExpr>");

        builder.Rule("<AndExpr>").Is("<AndExpr>", "And", "<NotExpr>")
                                 .Or("<NotExpr>");

        builder.Rule("<NotExpr>").Is("Not", "<NotExpr>")
                                 .Or("<CompareExpr>");

        builder.Rule("<CompareExpr>").Is("<CompareExpr>", "Is", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "Is", "Not", "<ConcatExpr>")
                                     .Or("<CompareExpr>", ">=", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "=>", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "<=", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "=<", "<ConcatExpr>")
                                     .Or("<CompareExpr>", ">", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "<", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "<>", "<ConcatExpr>")
                                     .Or("<CompareExpr>", "=", "<ConcatExpr>")
                                     .Or("<ConcatExpr>");

        builder.Rule("<ConcatExpr>").Is("<ConcatExpr>", "&", "<AddExpr>")
                                    .Or("<AddExpr>");

        builder.Rule("<AddExpr>").Is("<AddExpr>", "+", "<ModExpr>")
                                 .Or("<AddExpr>", "-", "<ModExpr>")
                                 .Or("<ModExpr>");

        builder.Rule("<ModExpr>").Is("<ModExpr>", "Mod", "<IntDivExpr>")
                                 .Or("<IntDivExpr>");

        builder.Rule("<IntDivExpr>").Is("<IntDivExpr>", @"\", "<MultExpr>")
                                    .Or("<MultExpr>");

        builder.Rule("<MultExpr>").Is("<MultExpr>", "*", "<UnaryExpr>")
                                  .Or("<MultExpr>", "/", "<UnaryExpr>")
                                  .Or("<UnaryExpr>");

        builder.Rule("<UnaryExpr>").Is("-", "<UnaryExpr>")
                                   .Or("+", "<UnaryExpr>")
                                   .Or("<ExpExpr>");

        builder.Rule("<ExpExpr>").Is("<Value>", "^", "<ExpExpr>")
                                 .Or("<Value>");


        builder.Rule("<QualifiedID>").Is("{IDDot}", "<QualifiedIDTail>")
                                     .Or("{DotIDDot}", "<QualifiedIDTail>")
                                     .Or("{ID}")
                                     .Or("{DotID}");

        builder.Rule("<SubSafeExprOpt>").Is("<SubSafeExpr>")
                                        .OrOptional();

        builder.Rule("<QualifiedIDTail>").Is("{IDDot}", "<QualifiedIDTail>")
                                         .Or("{ID}")
                                         .Or("<KeywordID>");

        builder.Rule("<CommaExprList>").Is(",", "<Expr>", "<CommaExprList>")
                                       .Or(",", "<CommaExprList>")
                                       .Or(",", "<Expr>")
                                       .Or(",");

        builder.Rule("<IndexOrParamsList>").Is("<IndexOrParams>", "<IndexOrParamsList>")
                                           .Or("<IndexOrParams>");

        builder.Rule("<LeftExprTail>").Is("<QualifiedIDTail>", "<IndexOrParamsList>", ".", "<LeftExprTail>")
                                      .Or("<QualifiedIDTail>", "<IndexOrParamsListDot>", "<LeftExprTail>")
                                      .Or("<QualifiedIDTail>", "<IndexOrParamsList>")
                                      .Or("<QualifiedIDTail>");

        builder.Rule("<IndexOrParamsListDot>").Is("<IndexOrParams>", "<IndexOrParamsListDot>")
                                              .Or("<IndexOrParamsDot>");

        builder.Rule("<IndexOrParams>").Is("(", "<Expr>", "<CommaExprList>", ")")
                                       .Or("(", "<CommaExprList>", ")")
                                       .Or("(", "<Expr>", ")")
                                       .Or("(", ")");

        builder.Rule("<IndexOrParamsDot>").Is("(", "<Expr>", "<CommaExprList>", ").")
                                          .Or("(", "<CommaExprList>", ").")
                                          .Or("(", "<Expr>", ").")
                                          .Or("(", ").");

        builder.Rule("<PropertyAccessType>").Is("Get")
                                            .Or("Let")
                                            .Or("Set");

        builder.Rule("<IntLiteral>").Is("{IntLiteral}")
                                    .Or("{HexLiteral}")
                                    .Or("{OctLiteral}");

        builder.Rule("<BoolLiteral>").Is("True")
                                     .Or("False");

        builder.Rule("<Nothing>").Is("Nothing")
                                 .Or("Null")
                                 .Or("Empty");

        builder.Rule("<ArgModifierOpt>").Is("ByVal")
                                        .Or("ByRef")
                                        .OrOptional();

        builder.Rule("<Value>").Is("<ConstExpr>")
                               .Or("<LeftExpr>")
                               .Or("(", "<Expr>", ")")
                               .Or("(", "<Expr>", ").", "<LeftExprTail>");

        builder.Rule("<SubSafeExpr>").Is("<SubSafeImpExpr>");

        builder.Rule("<SubSafeImpExpr>").Is("<SubSafeImpExpr>", "Imp", "<EqvExpr>")
                                        .Or("<SubSafeEqvExpr>");

        builder.Rule("<SubSafeEqvExpr>").Is("<SubSafeEqvExpr>", "Eqv", "<XorExpr>")
                                        .Or("<SubSafeXorExpr>");

        builder.Rule("<SubSafeXorExpr>").Is("<SubSafeXorExpr>", "Xor", "<OrExpr>")
                                        .Or("<SubSafeOrExpr>");

        builder.Rule("<SubSafeOrExpr>").Is("<SubSafeOrExpr>", "Or", "<AndExpr>")
                                       .Or("<SubSafeAndExpr>");

        builder.Rule("<SubSafeAndExpr>").Is("<SubSafeAndExpr>", "And", "<NotExpr>")
                                        .Or("<SubSafeNotExpr>");

        builder.Rule("<SubSafeNotExpr>").Is("Not", "<NotExpr>")
                                        .Or("<SubSafeCompareExpr>");

        builder.Rule("<SubSafeCompareExpr>").Is("<SubSafeCompareExpr>", "Is", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "Is", "Not", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", ">=", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "=>", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "<=", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "=<", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", ">", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "<", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "<>", "<ConcatExpr>")
                                            .Or("<SubSafeCompareExpr>", "=", "<ConcatExpr>")
                                            .Or("<SubSafeConcatExpr>");

        builder.Rule("<SubSafeConcatExpr>").Is("<SubSafeConcatExpr>", "&", "<AddExpr>")
                                           .Or("<SubSafeAddExpr>");

        builder.Rule("<SubSafeAddExpr>").Is("<SubSafeAddExpr>", "+", "<ModExpr>")
                                        .Or("<SubSafeAddExpr>", "-", "<ModExpr>")
                                        .Or("<SubSafeModExpr>");

        builder.Rule("<SubSafeModExpr>").Is("<SubSafeModExpr>", "Mod", "<IntDivExpr>")
                                        .Or("<SubSafeIntDivExpr>");

        builder.Rule("<SubSafeIntDivExpr>").Is("<SubSafeIntDivExpr>", @"\", "<MultExpr>")
                                           .Or("<SubSafeMultExpr>");

        builder.Rule("<SubSafeMultExpr>").Is("<SubSafeMultExpr>", "*", "<UnaryExpr>")
                                         .Or("<SubSafeMultExpr>", "/", "<UnaryExpr>")
                                         .Or("<SubSafeUnaryExpr>");

        builder.Rule("<SubSafeUnaryExpr>").Is("-", "<UnaryExpr>")
                                          .Or("+", "<UnaryExpr>")
                                          .Or("<SubSafeExpExpr>");

        builder.Rule("<SubSafeExpExpr>").Is("<SubSafeValue>", "^", "<ExpExpr>")
                                        .Or("<SubSafeValue>");

        builder.Rule("<SubSafeValue>").Is("<ConstExpr>")
                                      .Or("<LeftExpr>");

        builder.Rule("<KeywordID>").Is("<SafeKeywordID>")
                                   .Or("And")
                                   .Or("ByRef")
                                   .Or("ByVal")
                                   .Or("Call")
                                   .Or("Case")
                                   .Or("Class")
                                   .Or("Const")
                                   .Or("Dim")
                                   .Or("Do")
                                   .Or("Each")
                                   .Or("Else")
                                   .Or("ElseIf")
                                   .Or("Empty")
                                   .Or("End")
                                   .Or("Eqv")
                                   .Or("Exit")
                                   .Or("False")
                                   .Or("For")
                                   .Or("Function")
                                   .Or("Get")
                                   .Or("GoTo")
                                   .Or("If")
                                   .Or("Imp")
                                   .Or("In")
                                   .Or("Is")
                                   .Or("Let")
                                   .Or("Loop")
                                   .Or("Mod")
                                   .Or("New")
                                   .Or("Next")
                                   .Or("Not")
                                   .Or("Nothing")
                                   .Or("Null")
                                   .Or("On")
                                   .Or("Option")
                                   .Or("Or")
                                   .Or("Preserve")
                                   .Or("Private")
                                   .Or("Public")
                                   .Or("Redim")
                                   .Or("Resume")
                                   .Or("Select")
                                   .Or("Set")
                                   .Or("Sub")
                                   .Or("Then")
                                   .Or("To")
                                   .Or("True")
                                   .Or("Until")
                                   .Or("WEnd")
                                   .Or("While")
                                   .Or("With")
                                   .Or("Xor");

        VBScriptX.Grammar    = builder.ToGrammar();

        Find.IgnoreCase     = true;

        var reserved        = Find.Longest("and".Sequence(),
                                           "byref".Sequence(),
                                           "byval".Sequence(),
                                           "call".Sequence(),
                                           "case".Sequence(),
                                           "class".Sequence(),
                                           "const".Sequence(),
                                           "dim".Sequence(),
                                           "do".Sequence(),
                                           "each".Sequence(),
                                           "else".Sequence(),
                                           "elseif".Sequence(),
                                           "empty".Sequence(),
                                           "end".Sequence(),
                                           "eqv".Sequence(),
                                           "exit".Sequence(),
                                           "false".Sequence(),
                                           "for".Sequence(),
                                           "function".Sequence(),
                                           "get".Sequence(),
                                           "goto".Sequence(),
                                           "if".Sequence(),
                                           "imp".Sequence(),
                                           "in".Sequence(),
                                           "is".Sequence(),
                                           "let".Sequence(),
                                           "loop".Sequence(),
                                           "mod".Sequence(),
                                           "new".Sequence(),
                                           "next".Sequence(),
                                           "not".Sequence(),
                                           "nothing".Sequence(),
                                           "null".Sequence(),
                                           "on".Sequence(),
                                           "option".Sequence(),
                                           "or".Sequence(),
                                           "preserve".Sequence(),
                                           "private".Sequence(),
                                           "public".Sequence(),
                                           "redim".Sequence(),
                                           "rem".Sequence(),
                                           "resume".Sequence(),
                                           "select".Sequence(),
                                           "set".Sequence(),
                                           "sub".Sequence(),
                                           "then".Sequence(),
                                           "to".Sequence(),
                                           "true".Sequence(),
                                           "until".Sequence(),
                                           "wend".Sequence(),
                                           "while".Sequence(),
                                           "with".Sequence(),
                                           "xor".Sequence());

        var realNewLine     = Find.Longest('\r'.One(), '\n'.One(), Find.Sequence("\r\n"));
        var printable       = String.Concat(@"!", "\"", @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`{|}~");
        var space           = String.Concat("\t", " ");
        //var spacePlus       = space.Set().Value.Then('_'.One()).Then(Find.While(space.Set().Value)).Then(Find.Maybe(realNewLine));
        var spacePlus       = '_'.One().Then(Find.While(space.Set())).Then(realNewLine);

        var stringChar      = Find.Set(printable.Replace("\"", String.Empty));
        var dateChar        = Find.Set(printable.Replace("#", String.Empty));
        var idNameChar      = Find.Set(printable.Replace("[", String.Empty).Replace("]", String.Empty));
        var idTail          = Find.Set("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_");

        var letter          = Find.Set("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var digit           = Find.Set("0123456789");
        var octDigit        = Find.Set("01234567");
        var hexDigit        = Find.Set("0123456789ABCDEF");
        //var newLine         = Find.Longest(realNewLine, Find.One(':')).Value.Then(VBScriptX.Grammar.Id("{new-line}"));
        var newLine         = realNewLine.Then(VBScriptX.Grammar.Id("{new-line}"));
        var intLiteral      = digit.Then(Find.While(digit)).Then(VBScriptX.Grammar.Id("{intliteral}"));
        var dateLiteral     = '#'.One().Then(dateChar.Then(Find.While(dateChar))).Then('#'.One()).Then(VBScriptX.Grammar.Id("{dateliteral}"));
        var hexLiteral      = "&H".Sequence().Then(hexDigit.Then(Find.While(hexDigit))).Then(Find.Maybe('&'.One())).Then(VBScriptX.Grammar.Id("{hexliteral}"));
        var octLiteral      = '&'.One().Then(octDigit.Then(Find.While(octDigit))).Then(Find.Maybe('&'.One())).Then(VBScriptX.Grammar.Id("{octliteral}"));
        //var stringLiteral   = '"'.One().Then(Find.Until("\"\"".Sequence().Maybe().Then('"'.One()))).Then("\"\"\"".Sequence().Or('"'.One())).Then(VBScriptX.Grammar.Id("{stringliteral}"));
        var stringLiteral   = '"'.One().Then(Find.While("\"\"".Sequence().Or(Find.Not('"'.One())))).Then('"'.One()).Then(VBScriptX.Grammar.Id("{stringliteral}"));

        var digitOneToN     = digit.Then(Find.While(digit));
        var exponent        = 'E'.One().Then(Find.Maybe("+-".Set())).Then(digitOneToN);
        var floatPeriod     = '.'.One().Then(digitOneToN).Then(Find.Maybe(exponent));
        var floatFull       = digitOneToN.Then(floatPeriod);
        var floatExp        = digitOneToN.Then(exponent);

        var floatLiteral    = Find.Longest(floatPeriod, floatFull, floatExp).Then(VBScriptX.Grammar.Id("{floatliteral}"));

        var idBaseOne       = letter.Then(Find.While(idTail));
        var idBaseTwo       = '['.One().Then(Find.While(idNameChar)).Then(']'.One());

        var id              = Find.Longest(idBaseOne, idBaseTwo).NotEqual(reserved).Then(VBScriptX.Grammar.Id("{id}"));
        var idDot           = Find.Longest(idBaseOne, idBaseTwo).Then('.'.One()).Then(VBScriptX.Grammar.Id("{iddot}"));
        var dotId           = '.'.One().Then(Find.Longest(idBaseOne, idBaseTwo)).Then(VBScriptX.Grammar.Id("{dotid}"));
        var dotIdDot        = '.'.One().Then(Find.Longest(idBaseOne, idBaseTwo)).Then('.'.One()).Then(VBScriptX.Grammar.Id("{dotiddot}"));

        var commentOne      = '\''.One().Then(Find.Until(realNewLine)).Then(Identifier.Trivia);
        var commentTwo      = "REM".Sequence().Then(Find.Longest(realNewLine, space.Set().Then(Find.While(String.Concat(space, printable).Set())))).Then(Identifier.Trivia);

        var literals        = Find.Literals(VBScriptX.Grammar.LiteralSymbols().Select(s => (s, VBScriptX.Grammar.Id(s))));

        var all = Find.Longest(literals,
                               commentOne,
                               commentTwo,
                               id,
                               idDot,
                               dotId,
                               dotIdDot,
                               floatLiteral,
                               stringLiteral,
                               octLiteral,
                               hexLiteral,
                               dateLiteral,
                               intLiteral,
                               newLine,
                               space.Set().Then(Identifier.Trivia),
                               spacePlus.Then(Identifier.Trivia));

        VBScriptX.Seek = Find.Seek(all);

        _AlwaysRemove = new HashSet<Identifier>();
        _AlwaysRemove.Add(VBScriptX.Grammar.Id("<NL>"));
        _AlwaysRemove.Add(VBScriptX.Grammar.Id("<NLOpt>"));
        _AlwaysRemove.Add(VBScriptX.Grammar.Id("{new-line}"));

        _ListCollapse = new HashSet<Identifier>();
        _ListCollapse.Add(VBScriptX.Grammar.Id("<GlobalStmtList>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<GlobalStmt>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<MemberDeclList>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<MemberDecl>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<MethodStmtList>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<MethodStmt>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<BlockStmt>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<BlockStmtList>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<Arg>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<ArgList>"));
        _ListCollapse.Add(VBScriptX.Grammar.Id("<CommaExprList>"));

        _SingleItemCollapse = new HashSet<Identifier>();
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<ImpExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<EqvExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<XorExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<OrExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<AndExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<NotExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<CompareExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<ConcatExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<AddExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<ModExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<IntDivExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<MultExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<UnaryExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<ExpExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeImpExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeEqvExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeXorExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeOrExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeAndExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeNotExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeCompareExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeConcatExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeAddExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeModExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeIntDivExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeMultExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeUnaryExpr>"));
        _SingleItemCollapse.Add(VBScriptX.Grammar.Id("<SubSafeExpExpr>"));

        _EmptyNodeRemove = new HashSet<Identifier>();
        _EmptyNodeRemove.Add(VBScriptX.Grammar.Id("<ArgModifierOpt>"));

    }

    private static IEnumerable<NodeOrToken> CollapsedMembers(this IEnumerable<NodeOrToken> members)
    {
        foreach (var member in members)
        {
            if (member.AsNode != null)
            {
                if (_AlwaysRemove.Contains(member.AsNode.Id))
                    continue;
                if (_ListCollapse.Contains(member.AsNode.Id))
                    foreach (var sub in member.AsNode.Members.CollapsedMembers())
                        yield return sub;
                else if (_SingleItemCollapse.Contains(member.AsNode.Id) && member.AsNode.Members.Count == 1)
                    foreach (var sub in member.AsNode.Members.CollapsedMembers())
                        yield return sub;
                else if (_EmptyNodeRemove.Contains(member.AsNode.Id) && member.AsNode.Members.Count == 0)
                    continue;
                else
                    yield return member.AsNode.Collapsed();
            }
            else
                yield return member;
        }
    }

    private static Node Collapsed(this Node node)
        => new Node(node.Id, node.Symbol, node.Members.CollapsedMembers());


    private static ParseException CreateError(Chart chart, String source, Token[] tokens)
    {
        var errPoint = chart[0];

        while (errPoint.Index < tokens.Length - 1 && chart[errPoint.Index + 1].Count > 0)
            errPoint = chart[errPoint.Index + 1];
        
        //var errPoint = chart.FirstOrDefault(p => p.GetEntries().Count() == 0 || p.Index == tokens.Length - 1);

        var lineStart = errPoint.Index;
        var lineStop  = errPoint.Index;

        var newLine = Grammar.Id("{new-line}");

        if (tokens[errPoint.Index].Id.IsEqual(newLine))
        {
            while (lineStart > 0 && !tokens[lineStart - 1].Id.IsEqual(newLine))
                lineStart--;
        }
        else
        {
            while (lineStart > 0 && !tokens[lineStart - 1].Id.IsEqual(newLine))
                lineStart--;

            while (lineStop < tokens.Length && !tokens[lineStop].Id.IsEqual(newLine))
                lineStop++;
        }

        var lineCount = (Int32)1;
        var current   = lineStart - 1;

        while (current > -1)
        {
            if (tokens[current].Id.IsEqual(newLine))
                lineCount++;

            current--;
        }

        var lineNumber   = lineCount;
        var columnNumber = (  tokens[errPoint.Index].Span.Position
                                - tokens[lineStart].Span.Position     );

        var positionStart = tokens[lineStart].Span.Position;
        var positionStop  = tokens[lineStop].Span.Position + tokens[lineStop].Span.Length;

        var errorLine = positionStart >= 0 && positionStop > positionStart
                            ? source.Substring(positionStart, (positionStop - positionStart))
                            : String.Empty;

        if (Grammar.IsValid(tokens[errPoint.Index].Id))
        {
            var rules = String.Join("\r\n", errPoint.Where(e => e.NextRuleId != Identifier.None && Grammar.IsTerminal(e.NextRuleId))
                                                    .Select(t => t.ToString()));

            return new ParseException("Unexpected symbol.", lineNumber, columnNumber, errorLine, rules);
        }
        else
        {
            return new ParseException("Invalid symbol.", lineNumber, columnNumber, errorLine, String.Empty);
        }
    }

    public static Suspect<Node> Parse(String source)
    {
        var lastChar = source[source.Length - 1];

        if (!(lastChar == '\r' || (lastChar == '\n' || lastChar == ':')))
            source = String.Concat(source, "\r\n");

        var hits     = VBScriptX.Seek.Repeat(source).Where(h => !h.Id.IsEqual(Identifier.Trivia)).ToArray();
        var tokens   = hits.Select(h => new Token(h.Span, h.Id)).Concat(new Token[] { new Token(End.Span(source), VBScriptX.Grammar.Id(VBScriptX.END_OF_INPUT).ToTokenIdentifier()) }).ToArray();
        var recChart = VBScriptX.Grammar.Chart(tokens);

        if (recChart[recChart.Length - 1].Any(entry => entry.Rule.Symbol == "<Program>" && entry.Number == 0))
        {
            var hitsChart = new FinalChart(recChart);

            if (hitsChart[0].Any(entry => entry.Rule.Symbol == "<Program>"))
            {
                var ast = Node.Create(hitsChart, tokens);

                if (ast is Some<Node> someAst)
                    return someAst.Value.Collapsed().Confirmed();
            }
        }

        return CreateError(recChart, source, tokens).Failed<Node>();
    }

}

