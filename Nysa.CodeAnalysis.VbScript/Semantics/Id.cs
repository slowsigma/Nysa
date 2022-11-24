using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Text.Parsing;
using ParseId = Nysa.Text.Identifier;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Id
    {
        public static class Category
        {

            public static readonly ParseId EOI = VbScript.Language.Grammar.Id("{EOI}");
            public static readonly ParseId ID = VbScript.Language.Grammar.Id("{ID}");
            public static readonly ParseId IntLiteral = VbScript.Language.Grammar.Id("{IntLiteral}");
            public static readonly ParseId FloatLiteral = VbScript.Language.Grammar.Id("{FloatLiteral}");
            public static readonly ParseId StringLiteral = VbScript.Language.Grammar.Id("{StringLiteral}");
            public static readonly ParseId DateLiteral = VbScript.Language.Grammar.Id("{DateLiteral}");
            public static readonly ParseId IDDot = VbScript.Language.Grammar.Id("{IDDot}");
            public static readonly ParseId DotIDDot = VbScript.Language.Grammar.Id("{DotIDDot}");
            public static readonly ParseId DotID = VbScript.Language.Grammar.Id("{DotID}");
            public static readonly ParseId HexLiteral = VbScript.Language.Grammar.Id("{HexLiteral}");
            public static readonly ParseId OctLiteral = VbScript.Language.Grammar.Id("{OctLiteral}");

        }

        public static class Symbol
        {

            public static readonly ParseId Colon = VbScript.Language.Grammar.Id(":");
            public static readonly ParseId Option = VbScript.Language.Grammar.Id("Option");
            public static readonly ParseId Explicit = VbScript.Language.Grammar.Id("Explicit");
            public static readonly ParseId Class = VbScript.Language.Grammar.Id("Class");
            public static readonly ParseId End = VbScript.Language.Grammar.Id("End");
            public static readonly ParseId Private = VbScript.Language.Grammar.Id("Private");
            public static readonly ParseId Public = VbScript.Language.Grammar.Id("Public");
            public static readonly ParseId Const = VbScript.Language.Grammar.Id("Const");
            public static readonly ParseId Sub = VbScript.Language.Grammar.Id("Sub");
            public static readonly ParseId Function = VbScript.Language.Grammar.Id("Function");
            public static readonly ParseId OpenParen = VbScript.Language.Grammar.Id("(");
            public static readonly ParseId CloseParen = VbScript.Language.Grammar.Id(")");
            public static readonly ParseId Comma = VbScript.Language.Grammar.Id(",");
            public static readonly new ParseId Equals = VbScript.Language.Grammar.Id("=");
            public static readonly ParseId Default = VbScript.Language.Grammar.Id("Default");
            public static readonly ParseId Erase = VbScript.Language.Grammar.Id("Erase");
            public static readonly ParseId Error = VbScript.Language.Grammar.Id("Error");
            public static readonly ParseId Property = VbScript.Language.Grammar.Id("Property");
            public static readonly ParseId Step = VbScript.Language.Grammar.Id("Step");
            public static readonly ParseId Minus = VbScript.Language.Grammar.Id("-");
            public static readonly ParseId Plus = VbScript.Language.Grammar.Id("+");
            public static readonly ParseId Set = VbScript.Language.Grammar.Id("Set");
            public static readonly ParseId Call = VbScript.Language.Grammar.Id("Call");
            public static readonly ParseId Dot = VbScript.Language.Grammar.Id(".");
            public static readonly ParseId On = VbScript.Language.Grammar.Id("On");
            public static readonly ParseId Resume = VbScript.Language.Grammar.Id("Resume");
            public static readonly ParseId Next = VbScript.Language.Grammar.Id("Next");
            public static readonly ParseId GoTo = VbScript.Language.Grammar.Id("GoTo");
            public static readonly ParseId Exit = VbScript.Language.Grammar.Id("Exit");
            public static readonly ParseId Do = VbScript.Language.Grammar.Id("Do");
            public static readonly ParseId For = VbScript.Language.Grammar.Id("For");
            public static readonly ParseId Dim = VbScript.Language.Grammar.Id("Dim");
            public static readonly ParseId Redim = VbScript.Language.Grammar.Id("Redim");
            public static readonly ParseId Preserve = VbScript.Language.Grammar.Id("Preserve");
            public static readonly ParseId If = VbScript.Language.Grammar.Id("If");
            public static readonly ParseId Then = VbScript.Language.Grammar.Id("Then");
            public static readonly ParseId ElseIf = VbScript.Language.Grammar.Id("ElseIf");
            public static readonly ParseId Else = VbScript.Language.Grammar.Id("Else");
            public static readonly ParseId With = VbScript.Language.Grammar.Id("With");
            public static readonly ParseId Select = VbScript.Language.Grammar.Id("Select");
            public static readonly ParseId Case = VbScript.Language.Grammar.Id("Case");
            public static readonly ParseId Loop = VbScript.Language.Grammar.Id("Loop");
            public static readonly ParseId While = VbScript.Language.Grammar.Id("While");
            public static readonly ParseId WEnd = VbScript.Language.Grammar.Id("WEnd");
            public static readonly ParseId Until = VbScript.Language.Grammar.Id("Until");
            public static readonly ParseId To = VbScript.Language.Grammar.Id("To");
            public static readonly ParseId Each = VbScript.Language.Grammar.Id("Each");
            public static readonly ParseId In = VbScript.Language.Grammar.Id("In");
            public static readonly ParseId New = VbScript.Language.Grammar.Id("New");
            public static readonly ParseId Imp = VbScript.Language.Grammar.Id("Imp");
            public static readonly ParseId Eqv = VbScript.Language.Grammar.Id("Eqv");
            public static readonly ParseId Xor = VbScript.Language.Grammar.Id("Xor");
            public static readonly ParseId Or = VbScript.Language.Grammar.Id("Or");
            public static readonly ParseId And = VbScript.Language.Grammar.Id("And");
            public static readonly ParseId Not = VbScript.Language.Grammar.Id("Not");
            public static readonly ParseId Is = VbScript.Language.Grammar.Id("Is");
            public static readonly ParseId GTE = VbScript.Language.Grammar.Id(">=");
            public static readonly ParseId EGT = VbScript.Language.Grammar.Id("=>");
            public static readonly ParseId LTE = VbScript.Language.Grammar.Id("<=");
            public static readonly ParseId ELT = VbScript.Language.Grammar.Id("=<");
            public static readonly ParseId GT = VbScript.Language.Grammar.Id(">");
            public static readonly ParseId LT = VbScript.Language.Grammar.Id("<");
            public static readonly ParseId NEQ = VbScript.Language.Grammar.Id("<>");
            public static readonly ParseId Ampersand = VbScript.Language.Grammar.Id("&");
            public static readonly ParseId Mod = VbScript.Language.Grammar.Id("Mod");
            public static readonly ParseId IntDiv = VbScript.Language.Grammar.Id(@"\");
            public static readonly ParseId Mult = VbScript.Language.Grammar.Id("*");
            public static readonly ParseId Div = VbScript.Language.Grammar.Id("/");
            public static readonly ParseId Caret = VbScript.Language.Grammar.Id("^");
            public static readonly ParseId CloseParenDot = VbScript.Language.Grammar.Id(").");
            public static readonly ParseId Get = VbScript.Language.Grammar.Id("Get");
            public static readonly ParseId Let = VbScript.Language.Grammar.Id("Let");
            public static readonly ParseId True = VbScript.Language.Grammar.Id("True");
            public static readonly ParseId False = VbScript.Language.Grammar.Id("False");
            public static readonly ParseId Nothing = VbScript.Language.Grammar.Id("Nothing");
            public static readonly ParseId Null = VbScript.Language.Grammar.Id("Null");
            public static readonly ParseId Empty = VbScript.Language.Grammar.Id("Empty");
            public static readonly ParseId ByVal = VbScript.Language.Grammar.Id("ByVal");
            public static readonly ParseId ByRef = VbScript.Language.Grammar.Id("ByRef");

        }

        public static class Rule
        {

            public static readonly ParseId Program = VbScript.Language.Grammar.Id("<Program>");
            public static readonly ParseId InlineNL = VbScript.Language.Grammar.Id("<InlineNL>");
            public static readonly ParseId NL = VbScript.Language.Grammar.Id("<NL>");
            public static readonly ParseId NLOpt = VbScript.Language.Grammar.Id("<NLOpt>");
            public static readonly ParseId GlobalStmtList = VbScript.Language.Grammar.Id("<GlobalStmtList>");
            public static readonly ParseId GlobalStmt = VbScript.Language.Grammar.Id("<GlobalStmt>");
            public static readonly ParseId OptionExplicit = VbScript.Language.Grammar.Id("<OptionExplicit>");
            public static readonly ParseId ClassDecl = VbScript.Language.Grammar.Id("<ClassDecl>");
            public static readonly ParseId FieldDecl = VbScript.Language.Grammar.Id("<FieldDecl>");
            public static readonly ParseId ConstDecl = VbScript.Language.Grammar.Id("<ConstDecl>");
            public static readonly ParseId BlockConstDecl = VbScript.Language.Grammar.Id("<BlockConstDecl>");
            public static readonly ParseId SubDecl = VbScript.Language.Grammar.Id("<SubDecl>");
            public static readonly ParseId FunctionDecl = VbScript.Language.Grammar.Id("<FunctionDecl>");
            public static readonly ParseId BlockStmt = VbScript.Language.Grammar.Id("<BlockStmt>");
            public static readonly ParseId ExtendedID = VbScript.Language.Grammar.Id("<ExtendedID>");
            public static readonly ParseId MemberDeclList = VbScript.Language.Grammar.Id("<MemberDeclList>");
            public static readonly ParseId FieldName = VbScript.Language.Grammar.Id("<FieldName>");
            public static readonly ParseId OtherVarsOpt = VbScript.Language.Grammar.Id("<OtherVarsOpt>");
            public static readonly ParseId AccessModifierOpt = VbScript.Language.Grammar.Id("<AccessModifierOpt>");
            public static readonly ParseId ConstList = VbScript.Language.Grammar.Id("<ConstList>");
            public static readonly ParseId MethodAccessOpt = VbScript.Language.Grammar.Id("<MethodAccessOpt>");
            public static readonly ParseId MethodArgList = VbScript.Language.Grammar.Id("<MethodArgList>");
            public static readonly ParseId MethodStmtList = VbScript.Language.Grammar.Id("<MethodStmtList>");
            public static readonly ParseId InlineStmtList = VbScript.Language.Grammar.Id("<InlineStmtList>");
            public static readonly ParseId InlineStmt = VbScript.Language.Grammar.Id("<InlineStmt>");
            public static readonly ParseId SafeKeywordID = VbScript.Language.Grammar.Id("<SafeKeywordID>");
            public static readonly ParseId MemberDecl = VbScript.Language.Grammar.Id("<MemberDecl>");
            public static readonly ParseId FieldID = VbScript.Language.Grammar.Id("<FieldID>");
            public static readonly ParseId VarName = VbScript.Language.Grammar.Id("<VarName>");
            public static readonly ParseId ConstExprDef = VbScript.Language.Grammar.Id("<ConstExprDef>");
            public static readonly ParseId ArgList = VbScript.Language.Grammar.Id("<ArgList>");
            public static readonly ParseId MethodStmt = VbScript.Language.Grammar.Id("<MethodStmt>");
            public static readonly ParseId AssignStmt = VbScript.Language.Grammar.Id("<AssignStmt>");
            public static readonly ParseId CallStmt = VbScript.Language.Grammar.Id("<CallStmt>");
            public static readonly ParseId SubCallStmt = VbScript.Language.Grammar.Id("<SubCallStmt>");
            public static readonly ParseId ErrorStmt = VbScript.Language.Grammar.Id("<ErrorStmt>");
            public static readonly ParseId ExitStmt = VbScript.Language.Grammar.Id("<ExitStmt>");
            public static readonly ParseId VarDecl = VbScript.Language.Grammar.Id("<VarDecl>");
            public static readonly ParseId RedimStmt = VbScript.Language.Grammar.Id("<RedimStmt>");
            public static readonly ParseId RedimDeclList = VbScript.Language.Grammar.Id("<RedimDeclList>");
            public static readonly ParseId RedimDecl = VbScript.Language.Grammar.Id("<RedimDecl>");
            public static readonly ParseId IfStmt = VbScript.Language.Grammar.Id("<IfStmt>");
            public static readonly ParseId InlineIfStmt = VbScript.Language.Grammar.Id("<InlineIfStmt>");
            public static readonly ParseId ElseStmtList = VbScript.Language.Grammar.Id("<ElseStmtList>");
            public static readonly ParseId ElseOpt = VbScript.Language.Grammar.Id("<ElseOpt>");
            public static readonly ParseId EndIfOpt = VbScript.Language.Grammar.Id("<EndIfOpt>");
            public static readonly ParseId WithStmt = VbScript.Language.Grammar.Id("<WithStmt>");
            public static readonly ParseId SelectStmt = VbScript.Language.Grammar.Id("<SelectStmt>");
            public static readonly ParseId CaseStmtList = VbScript.Language.Grammar.Id("<CaseStmtList>");
            public static readonly ParseId ExprList = VbScript.Language.Grammar.Id("<ExprList>");
            public static readonly ParseId LoopStmt = VbScript.Language.Grammar.Id("<LoopStmt>");
            public static readonly ParseId LoopType = VbScript.Language.Grammar.Id("<LoopType>");
            public static readonly ParseId ForStmt = VbScript.Language.Grammar.Id("<ForStmt>");
            public static readonly ParseId BlockStmtList = VbScript.Language.Grammar.Id("<BlockStmtList>");
            public static readonly ParseId StepOpt = VbScript.Language.Grammar.Id("<StepOpt>");
            public static readonly ParseId PropertyDecl = VbScript.Language.Grammar.Id("<PropertyDecl>");
            public static readonly ParseId ArrayRankList = VbScript.Language.Grammar.Id("<ArrayRankList>");
            public static readonly ParseId ConstExpr = VbScript.Language.Grammar.Id("<ConstExpr>");
            public static readonly ParseId Arg = VbScript.Language.Grammar.Id("<Arg>");
            public static readonly ParseId NewObjectExpr = VbScript.Language.Grammar.Id("<NewObjectExpr>");
            public static readonly ParseId LeftExpr = VbScript.Language.Grammar.Id("<LeftExpr>");
            public static readonly ParseId Expr = VbScript.Language.Grammar.Id("<Expr>");
            public static readonly ParseId ImpExpr = VbScript.Language.Grammar.Id("<ImpExpr>");
            public static readonly ParseId EqvExpr = VbScript.Language.Grammar.Id("<EqvExpr>");
            public static readonly ParseId XorExpr = VbScript.Language.Grammar.Id("<XorExpr>");
            public static readonly ParseId OrExpr = VbScript.Language.Grammar.Id("<OrExpr>");
            public static readonly ParseId AndExpr = VbScript.Language.Grammar.Id("<AndExpr>");
            public static readonly ParseId NotExpr = VbScript.Language.Grammar.Id("<NotExpr>");
            public static readonly ParseId CompareExpr = VbScript.Language.Grammar.Id("<CompareExpr>");
            public static readonly ParseId ConcatExpr = VbScript.Language.Grammar.Id("<ConcatExpr>");
            public static readonly ParseId AddExpr = VbScript.Language.Grammar.Id("<AddExpr>");
            public static readonly ParseId ModExpr = VbScript.Language.Grammar.Id("<ModExpr>");
            public static readonly ParseId IntDivExpr = VbScript.Language.Grammar.Id("<IntDivExpr>");
            public static readonly ParseId MultExpr = VbScript.Language.Grammar.Id("<MultExpr>");
            public static readonly ParseId UnaryExpr = VbScript.Language.Grammar.Id("<UnaryExpr>");
            public static readonly ParseId ExpExpr = VbScript.Language.Grammar.Id("<ExpExpr>");
            public static readonly ParseId QualifiedID = VbScript.Language.Grammar.Id("<QualifiedID>");
            public static readonly ParseId SubSafeExprOpt = VbScript.Language.Grammar.Id("<SubSafeExprOpt>");
            public static readonly ParseId QualifiedIDTail = VbScript.Language.Grammar.Id("<QualifiedIDTail>");
            public static readonly ParseId CommaExprList = VbScript.Language.Grammar.Id("<CommaExprList>");
            public static readonly ParseId IndexOrParamsList = VbScript.Language.Grammar.Id("<IndexOrParamsList>");
            public static readonly ParseId LeftExprTail = VbScript.Language.Grammar.Id("<LeftExprTail>");
            public static readonly ParseId IndexOrParamsListDot = VbScript.Language.Grammar.Id("<IndexOrParamsListDot>");
            public static readonly ParseId IndexOrParams = VbScript.Language.Grammar.Id("<IndexOrParams>");
            public static readonly ParseId IndexOrParamsDot = VbScript.Language.Grammar.Id("<IndexOrParamsDot>");
            public static readonly ParseId PropertyAccessType = VbScript.Language.Grammar.Id("<PropertyAccessType>");
            public static readonly ParseId IntLiteral = VbScript.Language.Grammar.Id("<IntLiteral>");
            public static readonly ParseId BoolLiteral = VbScript.Language.Grammar.Id("<BoolLiteral>");
            public static readonly ParseId Nothing = VbScript.Language.Grammar.Id("<Nothing>");
            public static readonly ParseId ArgModifierOpt = VbScript.Language.Grammar.Id("<ArgModifierOpt>");
            public static readonly ParseId Value = VbScript.Language.Grammar.Id("<Value>");
            public static readonly ParseId SubSafeExpr = VbScript.Language.Grammar.Id("<SubSafeExpr>");
            public static readonly ParseId SubSafeImpExpr = VbScript.Language.Grammar.Id("<SubSafeImpExpr>");
            public static readonly ParseId SubSafeEqvExpr = VbScript.Language.Grammar.Id("<SubSafeEqvExpr>");
            public static readonly ParseId SubSafeXorExpr = VbScript.Language.Grammar.Id("<SubSafeXorExpr>");
            public static readonly ParseId SubSafeOrExpr = VbScript.Language.Grammar.Id("<SubSafeOrExpr>");
            public static readonly ParseId SubSafeAndExpr = VbScript.Language.Grammar.Id("<SubSafeAndExpr>");
            public static readonly ParseId SubSafeNotExpr = VbScript.Language.Grammar.Id("<SubSafeNotExpr>");
            public static readonly ParseId SubSafeCompareExpr = VbScript.Language.Grammar.Id("<SubSafeCompareExpr>");
            public static readonly ParseId SubSafeConcatExpr = VbScript.Language.Grammar.Id("<SubSafeConcatExpr>");
            public static readonly ParseId SubSafeAddExpr = VbScript.Language.Grammar.Id("<SubSafeAddExpr>");
            public static readonly ParseId SubSafeModExpr = VbScript.Language.Grammar.Id("<SubSafeModExpr>");
            public static readonly ParseId SubSafeIntDivExpr = VbScript.Language.Grammar.Id("<SubSafeIntDivExpr>");
            public static readonly ParseId SubSafeMultExpr = VbScript.Language.Grammar.Id("<SubSafeMultExpr>");
            public static readonly ParseId SubSafeUnaryExpr = VbScript.Language.Grammar.Id("<SubSafeUnaryExpr>");
            public static readonly ParseId SubSafeExpExpr = VbScript.Language.Grammar.Id("<SubSafeExpExpr>");
            public static readonly ParseId SubSafeValue = VbScript.Language.Grammar.Id("<SubSafeValue>");
            public static readonly ParseId KeywordID = VbScript.Language.Grammar.Id("<KeywordID>");

        }

    }

}
