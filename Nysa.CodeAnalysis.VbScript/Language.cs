using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text;
using Nysa.Text.Lexing;
using Nysa.Text.Parsing;

namespace Nysa.CodeAnalysis.VbScript;

public static partial class Language
{
    public static readonly String END_OF_INPUT = "{EOI}";

    public static readonly Grammar Grammar;
    public static readonly SeekRule Seek;

    static Language()
    {
        var builder = new GrammarBuilder("<Program>");

        builder.Rule("<Program>").Is("<NLOpt>", "<GlobalStmtList>", END_OF_INPUT);

        builder.Rule("<InlineNL>").Is(":", "<InlineNL>")
                                  .Or(":");

        builder.Rule("<NL>", NodePolicy.Remove)
               .Is("{new-line}", "<NL>")
               .Or("{new-line}")
               .Or("<InlineNL>", "<NL>")
               .Or("<InlineNL>");

        builder.Rule("<NLOpt>", NodePolicy.Remove)
               .Is("<NL>")
               .OrOptional();

        builder.Rule("<GlobalStmtList>", NodePolicy.Collapse)
               .Is("<GlobalStmt>", "<GlobalStmtList>")
               .OrOptional();

        builder.Rule("<GlobalStmt>", NodePolicy.Collapse)
               .Is("<OptionExplicit>")
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

        builder.Rule("<BlockConstDecl>").Is("Const", "<ConstList>", "<NL>");            // modification to original BNF

        builder.Rule("<SubDecl>").Is("<MethodAccessOpt>", "Sub", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Sub", "<NL>")
                                 .Or("<MethodAccessOpt>", "Sub", "<ExtendedID>", "<MethodArgList>", "<InlineStmt>", "End", "Sub", "<NL>");

        builder.Rule("<FunctionDecl>").Is("<MethodAccessOpt>", "Function", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Function", "<NL>")
                                      .Or("<MethodAccessOpt>", "Function", "<ExtendedID>", "<MethodArgList>", "<InlineStmt>", "End", "Function", "<NL>");

        builder.Rule("<BlockStmt>", NodePolicy.Collapse)
               .Is("<VarDecl>")
               .Or("<RedimStmt>")
               .Or("<IfStmt>")
               .Or("<WithStmt>")
               .Or("<SelectStmt>")
               .Or("<LoopStmt>")
               .Or("<ForStmt>")
               .Or("<BlockConstDecl>")                          // modification to original BNF
               .Or("<InlineStmt>", "<NL>");

        builder.Rule("<ExtendedID>").Is("<SafeKeywordID>")
                                    .Or("{ID}");

        builder.Rule("<MemberDeclList>", NodePolicy.Collapse)
               .Is("<MemberDecl>", "<MemberDeclList>")
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
                                   .Or("<ExtendedID>", ",", "<ConstList>")          // modification to original BNF
                                   .Or("<ExtendedID>");                             // modification to original BNF

        builder.Rule("<MethodAccessOpt>").Is("Public", "Default")
                                         .Or("<AccessModifierOpt>");

        builder.Rule("<MethodArgList>").Is("(", "<ArgList>", ")")
                                       .Or("(", ")")
                                       .OrOptional();

        builder.Rule("<MethodStmtList>", NodePolicy.Collapse)
               .Is("<MethodStmt>", "<MethodStmtList>")
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

        builder.Rule("<MemberDecl>", NodePolicy.Collapse)
               .Is("<FieldDecl>")
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

        builder.Rule("<ArgList>", NodePolicy.Collapse)
               .Is("<Arg>", ",", "<ArgList>")
               .Or("<Arg>");

        // original BNF had this:
        //builder.Rule("<MethodStmt>").Is("<ConstDecl>")
        //                            .Or("<BlockStmt>");
        // we modified to this:
        builder.Rule("<MethodStmt>", NodePolicy.Collapse)
               .Is("<BlockStmt>");

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

        builder.Rule("<BlockStmtList>", NodePolicy.Collapse)
               .Is("<BlockStmt>", "<BlockStmtList>")
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

        builder.Rule("<Arg>", NodePolicy.Collapse)
               .Is("<ArgModifierOpt>", "<ExtendedID>", "(", ")")
               .Or("<ArgModifierOpt>", "<ExtendedID>");

        builder.Rule("<NewObjectExpr>").Is("New", "<LeftExpr>");

        builder.Rule("<LeftExpr>").Is("<QualifiedID>", "<IndexOrParamsList>", ".", "<LeftExprTail>")
                                  .Or("<QualifiedID>", "<IndexOrParamsListDot>", "<LeftExprTail>")
                                  .Or("<QualifiedID>", "<IndexOrParamsList>")
                                  .Or("<QualifiedID>")
                                  .Or("<NewObjectExpr>")
                                  .Or("<SafeKeywordID>");

        builder.Rule("<Expr>").Is("<ImpExpr>");

        builder.Rule("<ImpExpr>", NodePolicy.CollapseSingle)
               .Is("<ImpExpr>", "Imp", "<EqvExpr>")
               .Or("<EqvExpr>");

        builder.Rule("<EqvExpr>", NodePolicy.CollapseSingle)
               .Is("<EqvExpr>", "Eqv", "<XorExpr>")
               .Or("<XorExpr>");

        builder.Rule("<XorExpr>", NodePolicy.CollapseSingle)
               .Is("<XorExpr>", "Xor", "<OrExpr>")
               .Or("<OrExpr>");

        builder.Rule("<OrExpr>", NodePolicy.CollapseSingle)
               .Is("<OrExpr>", "Or", "<AndExpr>")
               .Or("<AndExpr>");

        builder.Rule("<AndExpr>", NodePolicy.CollapseSingle)
               .Is("<AndExpr>", "And", "<NotExpr>")
               .Or("<NotExpr>");

        builder.Rule("<NotExpr>", NodePolicy.CollapseSingle)
               .Is("Not", "<NotExpr>")
               .Or("<CompareExpr>");

        builder.Rule("<CompareExpr>", NodePolicy.CollapseSingle)
               .Is("<CompareExpr>", "Is", "<ConcatExpr>")
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

        builder.Rule("<ConcatExpr>", NodePolicy.CollapseSingle)
               .Is("<ConcatExpr>", "&", "<AddExpr>")
               .Or("<AddExpr>");

        builder.Rule("<AddExpr>", NodePolicy.CollapseSingle)
               .Is("<AddExpr>", "+", "<ModExpr>")
               .Or("<AddExpr>", "-", "<ModExpr>")
               .Or("<ModExpr>");

        builder.Rule("<ModExpr>", NodePolicy.CollapseSingle)
               .Is("<ModExpr>", "Mod", "<IntDivExpr>")
               .Or("<IntDivExpr>");

        builder.Rule("<IntDivExpr>", NodePolicy.CollapseSingle)
               .Is("<IntDivExpr>", @"\", "<MultExpr>")
               .Or("<MultExpr>");

        builder.Rule("<MultExpr>", NodePolicy.CollapseSingle)
               .Is("<MultExpr>", "*", "<UnaryExpr>")
               .Or("<MultExpr>", "/", "<UnaryExpr>")
               .Or("<UnaryExpr>");

        builder.Rule("<UnaryExpr>", NodePolicy.CollapseSingle)
               .Is("-", "<UnaryExpr>")
               .Or("+", "<UnaryExpr>")
               .Or("<ExpExpr>");

        builder.Rule("<ExpExpr>", NodePolicy.CollapseSingle)
               .Is("<Value>", "^", "<ExpExpr>")
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

        builder.Rule("<CommaExprList>", NodePolicy.Collapse)
               .Is(",", "<Expr>", "<CommaExprList>")
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

        builder.Rule("<ArgModifierOpt>", NodePolicy.RemoveEmpty)
               .Is("ByVal")
               .Or("ByRef")
               .OrOptional();

        builder.Rule("<Value>").Is("<ConstExpr>")
                               .Or("<LeftExpr>")
                               .Or("(", "<Expr>", ")")
                               .Or("(", "<Expr>", ").", "<LeftExprTail>");

        builder.Rule("<SubSafeExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeImpExpr>");

        builder.Rule("<SubSafeImpExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeImpExpr>", "Imp", "<EqvExpr>")
               .Or("<SubSafeEqvExpr>");

        builder.Rule("<SubSafeEqvExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeEqvExpr>", "Eqv", "<XorExpr>")
               .Or("<SubSafeXorExpr>");

        builder.Rule("<SubSafeXorExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeXorExpr>", "Xor", "<OrExpr>")
               .Or("<SubSafeOrExpr>");

        builder.Rule("<SubSafeOrExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeOrExpr>", "Or", "<AndExpr>")
               .Or("<SubSafeAndExpr>");

        builder.Rule("<SubSafeAndExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeAndExpr>", "And", "<NotExpr>")
               .Or("<SubSafeNotExpr>");

        builder.Rule("<SubSafeNotExpr>", NodePolicy.CollapseSingle)
               .Is("Not", "<NotExpr>")
               .Or("<SubSafeCompareExpr>");

        builder.Rule("<SubSafeCompareExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeCompareExpr>", "Is", "<ConcatExpr>")
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

        builder.Rule("<SubSafeConcatExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeConcatExpr>", "&", "<AddExpr>")
               .Or("<SubSafeAddExpr>");

        builder.Rule("<SubSafeAddExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeAddExpr>", "+", "<ModExpr>")
               .Or("<SubSafeAddExpr>", "-", "<ModExpr>")
               .Or("<SubSafeModExpr>");

        builder.Rule("<SubSafeModExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeModExpr>", "Mod", "<IntDivExpr>")
               .Or("<SubSafeIntDivExpr>");

        builder.Rule("<SubSafeIntDivExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeIntDivExpr>", @"\", "<MultExpr>")
               .Or("<SubSafeMultExpr>");

        builder.Rule("<SubSafeMultExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeMultExpr>", "*", "<UnaryExpr>")
               .Or("<SubSafeMultExpr>", "/", "<UnaryExpr>")
               .Or("<SubSafeUnaryExpr>");

        builder.Rule("<SubSafeUnaryExpr>", NodePolicy.CollapseSingle)
               .Is("-", "<UnaryExpr>")
               .Or("+", "<UnaryExpr>")
               .Or("<SubSafeExpExpr>");

        builder.Rule("<SubSafeExpExpr>", NodePolicy.CollapseSingle)
               .Is("<SubSafeValue>", "^", "<ExpExpr>")
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

        Language.Grammar    = builder.ToGrammar();

        Take.IgnoreCase     = true;

        var reserved        = Take.Longest("and".Sequence(),
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

        var realNewLine     = Take.Longest('\r'.One(), '\n'.One(), Take.Sequence("\r\n"));
        var printable       = String.Concat(@"!", "\"", @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`{|}~");
        var space           = String.Concat("\t", " ");
        //var spacePlus       = space.Set().Value.Then('_'.One()).Then(Find.While(space.Set().Value)).Then(Find.Maybe(realNewLine));
        var spacePlus       = '_'.One().Then(Take.While(space.Set())).Then(realNewLine);

        var stringChar      = Take.Set(printable.Replace("\"", String.Empty));
        var dateChar        = Take.Set(printable.Replace("#", String.Empty));
        var idNameChar      = Take.Set(printable.Replace("[", String.Empty).Replace("]", String.Empty));
        var idTail          = Take.Set("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_");

        var letter          = Take.Set("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var digit           = Take.Set("0123456789");
        var octDigit        = Take.Set("01234567");
        var hexDigit        = Take.Set("0123456789ABCDEF");
        //var newLine         = Find.Longest(realNewLine, Find.One(':')).Value.Then(VBScriptX.Grammar.Id("{new-line}"));
        var newLine         = realNewLine.Then(Language.Grammar.Id("{new-line}"));
        var intLiteral      = digit.Then(Take.While(digit)).Then(Language.Grammar.Id("{intliteral}"));
        var dateLiteral     = '#'.One().Then(dateChar.Then(Take.While(dateChar))).Then('#'.One()).Then(Language.Grammar.Id("{dateliteral}"));
        var hexLiteral      = "&H".Sequence().Then(hexDigit.Then(Take.While(hexDigit))).Then(Take.Maybe('&'.One())).Then(Language.Grammar.Id("{hexliteral}"));
        var octLiteral      = '&'.One().Then(octDigit.Then(Take.While(octDigit))).Then(Take.Maybe('&'.One())).Then(Language.Grammar.Id("{octliteral}"));
        //var stringLiteral   = '"'.One().Then(Find.Until("\"\"".Sequence().Maybe().Then('"'.One()))).Then("\"\"\"".Sequence().Or('"'.One())).Then(Language.Grammar.Id("{stringliteral}"));
        var stringLiteral   = '"'.One().Then(Take.While("\"\"".Sequence().Or(Take.Not('"'.One())))).Then('"'.One()).Then(Language.Grammar.Id("{stringliteral}"));

        var digitOneToN     = digit.Then(Take.While(digit));
        var exponent        = 'E'.One().Then(Take.Maybe("+-".Set())).Then(digitOneToN);
        var floatPeriod     = '.'.One().Then(digitOneToN).Then(Take.Maybe(exponent));
        var floatFull       = digitOneToN.Then(floatPeriod);
        var floatExp        = digitOneToN.Then(exponent);

        var floatLiteral    = Take.Longest(floatPeriod, floatFull, floatExp).Then(Language.Grammar.Id("{floatliteral}"));

        var idBaseOne       = letter.Then(Take.While(idTail));
        var idBaseTwo       = '['.One().Then(Take.While(idNameChar)).Then(']'.One());

        var id              = Take.Longest(idBaseOne, idBaseTwo).NotEqual(reserved).Then(Language.Grammar.Id("{id}"));
        var idDot           = Take.Longest(idBaseOne, idBaseTwo).Then('.'.One()).Then(Language.Grammar.Id("{iddot}"));
        var dotId           = '.'.One().Then(Take.Longest(idBaseOne, idBaseTwo)).Then(Language.Grammar.Id("{dotid}"));
        var dotIdDot        = '.'.One().Then(Take.Longest(idBaseOne, idBaseTwo)).Then('.'.One()).Then(Language.Grammar.Id("{dotiddot}"));

        var commentOne      = '\''.One().Then(Take.Until(realNewLine)).Then(Identifier.Trivia);
        var commentTwo      = "REM".Sequence().Then(Take.Longest(realNewLine, space.Set().Then(Take.While(String.Concat(space, printable).Set())))).Then(Identifier.Trivia);

        var literals        = Take.Literals(Language.Grammar.LiteralSymbols().Select(s => (s, Language.Grammar.Id(s))));

        var all = Take.Longest(literals,
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

        Language.Seek = Take.Seek(all);
    }

    public static Suspect<Node> Parse(String source)
    {
        var lastChar = source[source.Length - 1];

        if (!(lastChar == '\r' || (lastChar == '\n' || lastChar == ':')))
            source = String.Concat(source, "\r\n");

        var hits     = Language.Seek.Repeat(source).Where(h => !h.Id.IsEqual(Identifier.Trivia)).ToArray();
        var tokens   = hits.Select(h => new Token(h.Span, h.Id)).Concat(new Token[] { new Token(End.Span(source), Language.Grammar.Id(Language.END_OF_INPUT).ToTokenIdentifier()) }).ToArray();
        var chart    =  Language.Grammar.CreateChart(tokens);

        if (!chart.IsIncomplete())
        {
            var inverse = chart.InverseChart();

            if (!inverse.IsIncomplete())
            {
                return inverse.ToSyntaxTree(tokens).Match(n => n.Confirmed(),
                                                          () => chart.CreateError(source, tokens).Failed<Node>());

            }
        }

        return chart.CreateError(source, tokens).Failed<Node>();
    }

}
