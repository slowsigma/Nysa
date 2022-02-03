using System;
using System.Collections.Generic;
using System.Text;

using Dorata.Text.Parsing;
using ParseId = Dorata.Text.Identifier;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Id
    {
        public static class Category
        {

            public static readonly ParseId EOI = VBScript.Grammar.Id("{EOI}");
            public static readonly ParseId ID = VBScript.Grammar.Id("{ID}");
            public static readonly ParseId IntLiteral = VBScript.Grammar.Id("{IntLiteral}");
            public static readonly ParseId FloatLiteral = VBScript.Grammar.Id("{FloatLiteral}");
            public static readonly ParseId StringLiteral = VBScript.Grammar.Id("{StringLiteral}");
            public static readonly ParseId DateLiteral = VBScript.Grammar.Id("{DateLiteral}");
            public static readonly ParseId IDDot = VBScript.Grammar.Id("{IDDot}");
            public static readonly ParseId DotIDDot = VBScript.Grammar.Id("{DotIDDot}");
            public static readonly ParseId DotID = VBScript.Grammar.Id("{DotID}");
            public static readonly ParseId HexLiteral = VBScript.Grammar.Id("{HexLiteral}");
            public static readonly ParseId OctLiteral = VBScript.Grammar.Id("{OctLiteral}");

        }

        public static class Symbol
        {

            public static readonly ParseId Colon = VBScript.Grammar.Id(":");
            public static readonly ParseId Option = VBScript.Grammar.Id("Option");
            public static readonly ParseId Explicit = VBScript.Grammar.Id("Explicit");
            public static readonly ParseId Class = VBScript.Grammar.Id("Class");
            public static readonly ParseId End = VBScript.Grammar.Id("End");
            public static readonly ParseId Private = VBScript.Grammar.Id("Private");
            public static readonly ParseId Public = VBScript.Grammar.Id("Public");
            public static readonly ParseId Const = VBScript.Grammar.Id("Const");
            public static readonly ParseId Sub = VBScript.Grammar.Id("Sub");
            public static readonly ParseId Function = VBScript.Grammar.Id("Function");
            public static readonly ParseId OpenParen = VBScript.Grammar.Id("(");
            public static readonly ParseId CloseParen = VBScript.Grammar.Id(")");
            public static readonly ParseId Comma = VBScript.Grammar.Id(",");
            public static readonly new ParseId Equals = VBScript.Grammar.Id("=");
            public static readonly ParseId Default = VBScript.Grammar.Id("Default");
            public static readonly ParseId Erase = VBScript.Grammar.Id("Erase");
            public static readonly ParseId Error = VBScript.Grammar.Id("Error");
            public static readonly ParseId Property = VBScript.Grammar.Id("Property");
            public static readonly ParseId Step = VBScript.Grammar.Id("Step");
            public static readonly ParseId Minus = VBScript.Grammar.Id("-");
            public static readonly ParseId Plus = VBScript.Grammar.Id("+");
            public static readonly ParseId Set = VBScript.Grammar.Id("Set");
            public static readonly ParseId Call = VBScript.Grammar.Id("Call");
            public static readonly ParseId Dot = VBScript.Grammar.Id(".");
            public static readonly ParseId On = VBScript.Grammar.Id("On");
            public static readonly ParseId Resume = VBScript.Grammar.Id("Resume");
            public static readonly ParseId Next = VBScript.Grammar.Id("Next");
            public static readonly ParseId GoTo = VBScript.Grammar.Id("GoTo");
            public static readonly ParseId Exit = VBScript.Grammar.Id("Exit");
            public static readonly ParseId Do = VBScript.Grammar.Id("Do");
            public static readonly ParseId For = VBScript.Grammar.Id("For");
            public static readonly ParseId Dim = VBScript.Grammar.Id("Dim");
            public static readonly ParseId Redim = VBScript.Grammar.Id("Redim");
            public static readonly ParseId Preserve = VBScript.Grammar.Id("Preserve");
            public static readonly ParseId If = VBScript.Grammar.Id("If");
            public static readonly ParseId Then = VBScript.Grammar.Id("Then");
            public static readonly ParseId ElseIf = VBScript.Grammar.Id("ElseIf");
            public static readonly ParseId Else = VBScript.Grammar.Id("Else");
            public static readonly ParseId With = VBScript.Grammar.Id("With");
            public static readonly ParseId Select = VBScript.Grammar.Id("Select");
            public static readonly ParseId Case = VBScript.Grammar.Id("Case");
            public static readonly ParseId Loop = VBScript.Grammar.Id("Loop");
            public static readonly ParseId While = VBScript.Grammar.Id("While");
            public static readonly ParseId WEnd = VBScript.Grammar.Id("WEnd");
            public static readonly ParseId Until = VBScript.Grammar.Id("Until");
            public static readonly ParseId To = VBScript.Grammar.Id("To");
            public static readonly ParseId Each = VBScript.Grammar.Id("Each");
            public static readonly ParseId In = VBScript.Grammar.Id("In");
            public static readonly ParseId New = VBScript.Grammar.Id("New");
            public static readonly ParseId Imp = VBScript.Grammar.Id("Imp");
            public static readonly ParseId Eqv = VBScript.Grammar.Id("Eqv");
            public static readonly ParseId Xor = VBScript.Grammar.Id("Xor");
            public static readonly ParseId Or = VBScript.Grammar.Id("Or");
            public static readonly ParseId And = VBScript.Grammar.Id("And");
            public static readonly ParseId Not = VBScript.Grammar.Id("Not");
            public static readonly ParseId Is = VBScript.Grammar.Id("Is");
            public static readonly ParseId GTE = VBScript.Grammar.Id(">=");
            public static readonly ParseId EGT = VBScript.Grammar.Id("=>");
            public static readonly ParseId LTE = VBScript.Grammar.Id("<=");
            public static readonly ParseId ELT = VBScript.Grammar.Id("=<");
            public static readonly ParseId GT = VBScript.Grammar.Id(">");
            public static readonly ParseId LT = VBScript.Grammar.Id("<");
            public static readonly ParseId NEQ = VBScript.Grammar.Id("<>");
            public static readonly ParseId Ampersand = VBScript.Grammar.Id("&");
            public static readonly ParseId Mod = VBScript.Grammar.Id("Mod");
            public static readonly ParseId IntDiv = VBScript.Grammar.Id(@"\");
            public static readonly ParseId Mult = VBScript.Grammar.Id("*");
            public static readonly ParseId Div = VBScript.Grammar.Id("/");
            public static readonly ParseId Caret = VBScript.Grammar.Id("^");
            public static readonly ParseId CloseParenDot = VBScript.Grammar.Id(").");
            public static readonly ParseId Get = VBScript.Grammar.Id("Get");
            public static readonly ParseId Let = VBScript.Grammar.Id("Let");
            public static readonly ParseId True = VBScript.Grammar.Id("True");
            public static readonly ParseId False = VBScript.Grammar.Id("False");
            public static readonly ParseId Nothing = VBScript.Grammar.Id("Nothing");
            public static readonly ParseId Null = VBScript.Grammar.Id("Null");
            public static readonly ParseId Empty = VBScript.Grammar.Id("Empty");
            public static readonly ParseId ByVal = VBScript.Grammar.Id("ByVal");
            public static readonly ParseId ByRef = VBScript.Grammar.Id("ByRef");

        }

        public static class Rule
        {

            public static readonly ParseId Program = VBScript.Grammar.Id("<Program>");
            public static readonly ParseId InlineNL = VBScript.Grammar.Id("<InlineNL>");
            public static readonly ParseId NL = VBScript.Grammar.Id("<NL>");
            public static readonly ParseId NLOpt = VBScript.Grammar.Id("<NLOpt>");
            public static readonly ParseId GlobalStmtList = VBScript.Grammar.Id("<GlobalStmtList>");
            public static readonly ParseId GlobalStmt = VBScript.Grammar.Id("<GlobalStmt>");
            public static readonly ParseId OptionExplicit = VBScript.Grammar.Id("<OptionExplicit>");
            public static readonly ParseId ClassDecl = VBScript.Grammar.Id("<ClassDecl>");
            public static readonly ParseId FieldDecl = VBScript.Grammar.Id("<FieldDecl>");
            public static readonly ParseId ConstDecl = VBScript.Grammar.Id("<ConstDecl>");
            public static readonly ParseId BlockConstDecl = VBScript.Grammar.Id("<BlockConstDecl>");
            public static readonly ParseId SubDecl = VBScript.Grammar.Id("<SubDecl>");
            public static readonly ParseId FunctionDecl = VBScript.Grammar.Id("<FunctionDecl>");
            public static readonly ParseId BlockStmt = VBScript.Grammar.Id("<BlockStmt>");
            public static readonly ParseId ExtendedID = VBScript.Grammar.Id("<ExtendedID>");
            public static readonly ParseId MemberDeclList = VBScript.Grammar.Id("<MemberDeclList>");
            public static readonly ParseId FieldName = VBScript.Grammar.Id("<FieldName>");
            public static readonly ParseId OtherVarsOpt = VBScript.Grammar.Id("<OtherVarsOpt>");
            public static readonly ParseId AccessModifierOpt = VBScript.Grammar.Id("<AccessModifierOpt>");
            public static readonly ParseId ConstList = VBScript.Grammar.Id("<ConstList>");
            public static readonly ParseId MethodAccessOpt = VBScript.Grammar.Id("<MethodAccessOpt>");
            public static readonly ParseId MethodArgList = VBScript.Grammar.Id("<MethodArgList>");
            public static readonly ParseId MethodStmtList = VBScript.Grammar.Id("<MethodStmtList>");
            public static readonly ParseId InlineStmtList = VBScript.Grammar.Id("<InlineStmtList>");
            public static readonly ParseId InlineStmt = VBScript.Grammar.Id("<InlineStmt>");
            public static readonly ParseId SafeKeywordID = VBScript.Grammar.Id("<SafeKeywordID>");
            public static readonly ParseId MemberDecl = VBScript.Grammar.Id("<MemberDecl>");
            public static readonly ParseId FieldID = VBScript.Grammar.Id("<FieldID>");
            public static readonly ParseId VarName = VBScript.Grammar.Id("<VarName>");
            public static readonly ParseId ConstExprDef = VBScript.Grammar.Id("<ConstExprDef>");
            public static readonly ParseId ArgList = VBScript.Grammar.Id("<ArgList>");
            public static readonly ParseId MethodStmt = VBScript.Grammar.Id("<MethodStmt>");
            public static readonly ParseId AssignStmt = VBScript.Grammar.Id("<AssignStmt>");
            public static readonly ParseId CallStmt = VBScript.Grammar.Id("<CallStmt>");
            public static readonly ParseId SubCallStmt = VBScript.Grammar.Id("<SubCallStmt>");
            public static readonly ParseId ErrorStmt = VBScript.Grammar.Id("<ErrorStmt>");
            public static readonly ParseId ExitStmt = VBScript.Grammar.Id("<ExitStmt>");
            public static readonly ParseId VarDecl = VBScript.Grammar.Id("<VarDecl>");
            public static readonly ParseId RedimStmt = VBScript.Grammar.Id("<RedimStmt>");
            public static readonly ParseId RedimDeclList = VBScript.Grammar.Id("<RedimDeclList>");
            public static readonly ParseId RedimDecl = VBScript.Grammar.Id("<RedimDecl>");
            public static readonly ParseId IfStmt = VBScript.Grammar.Id("<IfStmt>");
            public static readonly ParseId InlineIfStmt = VBScript.Grammar.Id("<InlineIfStmt>");
            public static readonly ParseId ElseStmtList = VBScript.Grammar.Id("<ElseStmtList>");
            public static readonly ParseId ElseOpt = VBScript.Grammar.Id("<ElseOpt>");
            public static readonly ParseId EndIfOpt = VBScript.Grammar.Id("<EndIfOpt>");
            public static readonly ParseId WithStmt = VBScript.Grammar.Id("<WithStmt>");
            public static readonly ParseId SelectStmt = VBScript.Grammar.Id("<SelectStmt>");
            public static readonly ParseId CaseStmtList = VBScript.Grammar.Id("<CaseStmtList>");
            public static readonly ParseId ExprList = VBScript.Grammar.Id("<ExprList>");
            public static readonly ParseId LoopStmt = VBScript.Grammar.Id("<LoopStmt>");
            public static readonly ParseId LoopType = VBScript.Grammar.Id("<LoopType>");
            public static readonly ParseId ForStmt = VBScript.Grammar.Id("<ForStmt>");
            public static readonly ParseId BlockStmtList = VBScript.Grammar.Id("<BlockStmtList>");
            public static readonly ParseId StepOpt = VBScript.Grammar.Id("<StepOpt>");
            public static readonly ParseId PropertyDecl = VBScript.Grammar.Id("<PropertyDecl>");
            public static readonly ParseId ArrayRankList = VBScript.Grammar.Id("<ArrayRankList>");
            public static readonly ParseId ConstExpr = VBScript.Grammar.Id("<ConstExpr>");
            public static readonly ParseId Arg = VBScript.Grammar.Id("<Arg>");
            public static readonly ParseId NewObjectExpr = VBScript.Grammar.Id("<NewObjectExpr>");
            public static readonly ParseId LeftExpr = VBScript.Grammar.Id("<LeftExpr>");
            public static readonly ParseId Expr = VBScript.Grammar.Id("<Expr>");
            public static readonly ParseId ImpExpr = VBScript.Grammar.Id("<ImpExpr>");
            public static readonly ParseId EqvExpr = VBScript.Grammar.Id("<EqvExpr>");
            public static readonly ParseId XorExpr = VBScript.Grammar.Id("<XorExpr>");
            public static readonly ParseId OrExpr = VBScript.Grammar.Id("<OrExpr>");
            public static readonly ParseId AndExpr = VBScript.Grammar.Id("<AndExpr>");
            public static readonly ParseId NotExpr = VBScript.Grammar.Id("<NotExpr>");
            public static readonly ParseId CompareExpr = VBScript.Grammar.Id("<CompareExpr>");
            public static readonly ParseId ConcatExpr = VBScript.Grammar.Id("<ConcatExpr>");
            public static readonly ParseId AddExpr = VBScript.Grammar.Id("<AddExpr>");
            public static readonly ParseId ModExpr = VBScript.Grammar.Id("<ModExpr>");
            public static readonly ParseId IntDivExpr = VBScript.Grammar.Id("<IntDivExpr>");
            public static readonly ParseId MultExpr = VBScript.Grammar.Id("<MultExpr>");
            public static readonly ParseId UnaryExpr = VBScript.Grammar.Id("<UnaryExpr>");
            public static readonly ParseId ExpExpr = VBScript.Grammar.Id("<ExpExpr>");
            public static readonly ParseId QualifiedID = VBScript.Grammar.Id("<QualifiedID>");
            public static readonly ParseId SubSafeExprOpt = VBScript.Grammar.Id("<SubSafeExprOpt>");
            public static readonly ParseId QualifiedIDTail = VBScript.Grammar.Id("<QualifiedIDTail>");
            public static readonly ParseId CommaExprList = VBScript.Grammar.Id("<CommaExprList>");
            public static readonly ParseId IndexOrParamsList = VBScript.Grammar.Id("<IndexOrParamsList>");
            public static readonly ParseId LeftExprTail = VBScript.Grammar.Id("<LeftExprTail>");
            public static readonly ParseId IndexOrParamsListDot = VBScript.Grammar.Id("<IndexOrParamsListDot>");
            public static readonly ParseId IndexOrParams = VBScript.Grammar.Id("<IndexOrParams>");
            public static readonly ParseId IndexOrParamsDot = VBScript.Grammar.Id("<IndexOrParamsDot>");
            public static readonly ParseId PropertyAccessType = VBScript.Grammar.Id("<PropertyAccessType>");
            public static readonly ParseId IntLiteral = VBScript.Grammar.Id("<IntLiteral>");
            public static readonly ParseId BoolLiteral = VBScript.Grammar.Id("<BoolLiteral>");
            public static readonly ParseId Nothing = VBScript.Grammar.Id("<Nothing>");
            public static readonly ParseId ArgModifierOpt = VBScript.Grammar.Id("<ArgModifierOpt>");
            public static readonly ParseId Value = VBScript.Grammar.Id("<Value>");
            public static readonly ParseId SubSafeExpr = VBScript.Grammar.Id("<SubSafeExpr>");
            public static readonly ParseId SubSafeImpExpr = VBScript.Grammar.Id("<SubSafeImpExpr>");
            public static readonly ParseId SubSafeEqvExpr = VBScript.Grammar.Id("<SubSafeEqvExpr>");
            public static readonly ParseId SubSafeXorExpr = VBScript.Grammar.Id("<SubSafeXorExpr>");
            public static readonly ParseId SubSafeOrExpr = VBScript.Grammar.Id("<SubSafeOrExpr>");
            public static readonly ParseId SubSafeAndExpr = VBScript.Grammar.Id("<SubSafeAndExpr>");
            public static readonly ParseId SubSafeNotExpr = VBScript.Grammar.Id("<SubSafeNotExpr>");
            public static readonly ParseId SubSafeCompareExpr = VBScript.Grammar.Id("<SubSafeCompareExpr>");
            public static readonly ParseId SubSafeConcatExpr = VBScript.Grammar.Id("<SubSafeConcatExpr>");
            public static readonly ParseId SubSafeAddExpr = VBScript.Grammar.Id("<SubSafeAddExpr>");
            public static readonly ParseId SubSafeModExpr = VBScript.Grammar.Id("<SubSafeModExpr>");
            public static readonly ParseId SubSafeIntDivExpr = VBScript.Grammar.Id("<SubSafeIntDivExpr>");
            public static readonly ParseId SubSafeMultExpr = VBScript.Grammar.Id("<SubSafeMultExpr>");
            public static readonly ParseId SubSafeUnaryExpr = VBScript.Grammar.Id("<SubSafeUnaryExpr>");
            public static readonly ParseId SubSafeExpExpr = VBScript.Grammar.Id("<SubSafeExpExpr>");
            public static readonly ParseId SubSafeValue = VBScript.Grammar.Id("<SubSafeValue>");
            public static readonly ParseId KeywordID = VBScript.Grammar.Id("<KeywordID>");

        }

    }

}
