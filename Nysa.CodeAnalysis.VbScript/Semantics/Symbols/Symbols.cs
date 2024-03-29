using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Symbols
    {
        public static readonly String VbScriptEngineErrClass = "_VbScriptEngineErrClass";
        public static readonly String DocumentClass = "_Document";

        public static IReadOnlyList<Symbol> VbBaseSymbols()
        {
            var symbols = new List<Symbol>();

            symbols.Add(new VariableSymbol("Err", Option.None, "Err object has no equivalent translation.".Some(), true, VbScriptEngineErrClass.Some(), SymbolCategories.vb));

            symbols.Add("me".ToVariableSymbol("this".Some()));

            symbols.Add("Abs".ToFunctionSymbol("Global.Abs".Some(), 1, 0));
            symbols.Add("Array".ToFunctionSymbol("Global.Array".Some()));
            symbols.Add("Asc".ToFunctionSymbol("Global.Asc".Some(), 1, 0));
            symbols.Add("Atn".ToFunctionSymbol("Global.Atn".Some(), 1, 0));
            symbols.Add("CBool".ToFunctionSymbol("Global.CBool".Some(), 1, 0));
            symbols.Add("CByte".ToFunctionSymbol("Global.CByte".Some(), 1, 0));
            symbols.Add("CCur".ToFunctionSymbol("Global.CCur".Some(), 1, 0));
            symbols.Add("CDate".ToFunctionSymbol("Global.CDate".Some(), 1, 0));
            symbols.Add("CDbl".ToFunctionSymbol("Global.CDbl".Some(), 1, 0));
            symbols.Add("CInt".ToFunctionSymbol("Global.CInt".Some(), 1, 0));
            symbols.Add("CLng".ToFunctionSymbol("Global.CLng".Some(), 1, 0));
            symbols.Add("Cos".ToFunctionSymbol("Global.Cos".Some(), 1, 0));
            symbols.Add("CSng".ToFunctionSymbol("Global.CSng".Some(), 1, 0));
            symbols.Add("CStr".ToFunctionSymbol("Global.CStr".Some(), 1, 0));
            symbols.Add("Date".ToFunctionSymbol("Global.Date".Some()));
            symbols.Add("DateAdd".ToFunctionSymbol("Global.DateAdd".Some()));
            symbols.Add("DateDiff".ToFunctionSymbol("Global.DateDiff".Some()));
            symbols.Add("DatePart".ToFunctionSymbol("Global.DatePart".Some()));
            symbols.Add("DateSerial".ToFunctionSymbol("Global.DateSerial".Some()));
            symbols.Add("DateValue".ToFunctionSymbol("Global.DateValue".Some()));
            symbols.Add("Day".ToFunctionSymbol("Global.Day".Some(), 1, 0));
            symbols.Add("Eval".ToErrFunctionSymbol("Eval has no equivalent translation."));
            symbols.Add("Execute".ToErrFunctionSymbol("Execute has no equivalent translation."));
            symbols.Add("Exp".ToFunctionSymbol("Global.Exp".Some(), 1, 0));
            symbols.Add("Filter".ToFunctionSymbol("Global.Filter".Some()));
            symbols.Add("Fix".ToFunctionSymbol("Global.Fix".Some(), 1, 0));
            symbols.Add("FormatCurrency".ToFunctionSymbol("Global.FormatCurrency".Some()));
            symbols.Add("FormatDateTime".ToFunctionSymbol("Global.FormatDateTime".Some()));
            symbols.Add("FormatNumber".ToFunctionSymbol("Global.FormatNumber".Some(), 1, 4));
            symbols.Add("FormatPercent".ToFunctionSymbol("Global.FormatPercent".Some()));
            symbols.Add("GetLocale".ToFunctionSymbol("Global.GetLocale".Some()));
            symbols.Add("GetObject".ToFunctionSymbol("Global.GetObject".Some()));
            symbols.Add("GetRef".ToFunctionSymbol("Global.GetRef".Some(), 1, 0));
            symbols.Add("Hex".ToFunctionSymbol("Global.Hex".Some(), 1, 0));
            symbols.Add("Hour".ToFunctionSymbol("Global.Hour".Some()));
            symbols.Add("Chr".ToFunctionSymbol("Global.Chr".Some(), 1, 0));
            symbols.Add("InputBox".ToFunctionSymbol("Global.InputBox".Some()));
            symbols.Add("InStr".ToFunctionSymbol("Global.InStr".Some(), 2, 2));
            symbols.Add("InStrRev".ToFunctionSymbol("Global.InStrRev".Some(), 2, 2));
            symbols.Add("Int".ToFunctionSymbol("Global.Int".Some()));
            symbols.Add("IsArray".ToFunctionSymbol("Global.IsArray".Some()));
            symbols.Add("IsDate".ToFunctionSymbol("Global.IsDate".Some()));
            symbols.Add("IsEmpty".ToFunctionSymbol("Global.IsUndefinedOrNull".Some()));
            symbols.Add("IsNull".ToFunctionSymbol("Global.IsNullOrUndefined".Some()));
            symbols.Add("IsNumeric".ToFunctionSymbol("Global.IsNumeric".Some()));
            symbols.Add("IsObject".ToFunctionSymbol("Global.IsObject".Some()));
            symbols.Add("Join".ToFunctionSymbol("Global.Join".Some()));
            symbols.Add("LBound".ToFunctionSymbol("Global.LBound".Some(), 1, 1));
            symbols.Add("LCase".ToFunctionSymbol("Global.LCase".Some(), 1, 0));
            symbols.Add("Left".ToFunctionSymbol("Global.Left".Some(), 2, 0));
            symbols.Add("Len".ToFunctionSymbol("Global.Len".Some(), 1, 0));
            symbols.Add("LoadPicture".ToFunctionSymbol("Global.LoadPicture".Some()));
            symbols.Add("Log".ToFunctionSymbol("Global.Log".Some(), 1, 0));
            symbols.Add("LTrim".ToFunctionSymbol("Global.LTrim".Some(), 1, 0));
            symbols.Add("Mid".ToFunctionSymbol("Global.Mid".Some(), 2, 1));
            symbols.Add("Minute".ToFunctionSymbol("Global.Minute".Some(), 1, 0));
            symbols.Add("Month".ToFunctionSymbol("Global.Month".Some(), 1, 0));
            symbols.Add("MonthName".ToFunctionSymbol("Global.MonthName".Some()));
            symbols.Add("MsgBox".ToFunctionSymbol("Global.MsgBox".Some(), 1, 4));
            symbols.Add("CreateObject".ToFunctionSymbol("Global.CreateObject".Some(), SymbolCategories.com));
            symbols.Add("Now".ToFunctionSymbol("Global.Now".Some()));
            symbols.Add("Oct".ToFunctionSymbol("Global.Oct".Some(), 1, 0));
            symbols.Add("Replace".ToFunctionSymbol("Global.Replace".Some(), 3, 3));
            symbols.Add("RGB".ToFunctionSymbol("Global.RGB".Some(), 3, 0));
            symbols.Add("Right".ToFunctionSymbol("Global.Right".Some(), 2, 0));
            symbols.Add("Rnd".ToFunctionSymbol("Global.Rnd".Some(), 1, 0));
            symbols.Add("Round".ToFunctionSymbol("Global.Round".Some(), 1, 1));
            symbols.Add("RTrim".ToFunctionSymbol("Global.RTrim".Some(), 1, 0));
            symbols.Add("ScriptEngine".ToFunctionSymbol("Global.ScriptEngine".Some()));
            symbols.Add("ScriptEngineBuildVersion".ToFunctionSymbol("Global.ScriptEngineBuildVersion".Some()));
            symbols.Add("ScriptEngineMajorVersion".ToFunctionSymbol("Global.ScriptEngineMajorVersion".Some()));
            symbols.Add("ScriptEngineMinorVersion".ToFunctionSymbol("Global.ScriptEngineMinorVersion".Some()));
            symbols.Add("Second".ToFunctionSymbol("Global.Second".Some(), 1, 0));
            symbols.Add("SetLocale".ToFunctionSymbol("Global.SetLocale".Some(), 1, 0));
            symbols.Add("Sgn".ToFunctionSymbol("Global.Sgn".Some(), 1, 0));
            symbols.Add("Sin".ToFunctionSymbol("Global.Sin".Some(), 1, 0));
            symbols.Add("Space".ToFunctionSymbol("Global.Space".Some(), 1, 0));
            symbols.Add("Split".ToFunctionSymbol("Global.Split".Some(), 1, 3));
            symbols.Add("Sqr".ToFunctionSymbol("Global.Sqr".Some(), 1, 0));
            symbols.Add("StrComp".ToFunctionSymbol("Global.StrComp".Some(), 2, 1));
            symbols.Add("String".ToFunctionSymbol("Global.String".Some(), 2, 0));
            symbols.Add("StrReverse".ToFunctionSymbol("Global.StrReverse".Some(), 1, 0));
            symbols.Add("Tan".ToFunctionSymbol("Global.Tan".Some(), 1, 0));
            symbols.Add("Time".ToFunctionSymbol("Global.Time".Some()));
            symbols.Add("Timer".ToFunctionSymbol("Global.Timer".Some()));
            symbols.Add("TimeSerial".ToFunctionSymbol("Global.TimeSerial".Some()));
            symbols.Add("TimeValue".ToFunctionSymbol("Global.TimeValue".Some()));
            symbols.Add("Trim".ToFunctionSymbol("Global.Trim".Some(), 1, 0));
            symbols.Add("TypeName".ToErrFunctionSymbol("TypeName has no equivalent translation."));
            symbols.Add("UBound".ToFunctionSymbol("Global.UBound".Some(), 1, 1));
            symbols.Add("UCase".ToFunctionSymbol("Global.UCase".Some(), 1, 0));
            symbols.Add("VarType".ToErrFunctionSymbol("VarType has no equivalent translation."));
            symbols.Add("Weekday".ToFunctionSymbol("Global.Weekday".Some()));
            symbols.Add("WeekdayName".ToFunctionSymbol("Global.WeekdayName".Some()));
            symbols.Add("Year".ToFunctionSymbol("Global.Year".Some(), 1, 0));
            symbols.Add("Randomize".ToFunctionSymbol("Global.Randomize".Some()));
            symbols.Add("vbObjectError".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbObjectError"));
            symbols.Add("vbTrue".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbTrue"));
            symbols.Add("vbFalse".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbFalse"));

            symbols.Add("vbEmpty".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeEmpty"));
            symbols.Add("vbNull".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeNull"));
            symbols.Add("vbBoolean".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeBoolean"));
            symbols.Add("vbByte".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeByte"));
            symbols.Add("vbInteger".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeInteger"));
            symbols.Add("vbLong".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeLong"));
            symbols.Add("vbSingle".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeSingle"));
            symbols.Add("vbDouble".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeDouble"));
            symbols.Add("vbDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeDate"));
            symbols.Add("vbString".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeString"));
            symbols.Add("vbObject".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeObject"));
            symbols.Add("vbArray".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeArray"));

            symbols.Add("vbCr".ToConstantSymbol(LiteralValueTypes.String, "Global.Cr"));
            symbols.Add("vbCrLf".ToConstantSymbol(LiteralValueTypes.String, "Global.CrLf"));
            symbols.Add("vbFormFeed".ToConstantSymbol(LiteralValueTypes.String, "Global.FormFeed"));
            symbols.Add("vbLf".ToConstantSymbol(LiteralValueTypes.String, "Global.Lf"));
            symbols.Add("vbNewLine".ToConstantSymbol(LiteralValueTypes.String, "Global.NewLine"));
            symbols.Add("vbNullChar".ToConstantSymbol(LiteralValueTypes.String, "Global.NullChar"));
            symbols.Add("vbNullString".ToConstantSymbol(LiteralValueTypes.String, "Global.NullString"));
            symbols.Add("vbTab".ToConstantSymbol(LiteralValueTypes.String, "Global.Tab"));
            symbols.Add("vbVerticalTab".ToConstantSymbol(LiteralValueTypes.String, "Global.VerticalTab"));

            symbols.Add("vbSunday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Sunday"));
            symbols.Add("vbMonday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Monday"));
            symbols.Add("vbTuesday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Tuesday"));
            symbols.Add("vbWednesday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Wednesday"));
            symbols.Add("vbThursday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Thursday"));
            symbols.Add("vbFriday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Friday"));
            symbols.Add("vbSaturday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Saturday"));

            symbols.Add("vbUseSystemDayOfWeek".ToConstantSymbol(LiteralValueTypes.Integer, "Global.UseSystemDayOfWeek"));
            symbols.Add("vbLongDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtLongDate"));
            symbols.Add("vbShortDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtShortDate"));
            symbols.Add("vbLongTime".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtLongTime"));
            symbols.Add("vbShortTime".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtShortTime"));
            symbols.Add("vbOKOnly".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOKOnly"));
            symbols.Add("vbOKCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOKCancel"));
            symbols.Add("vbAbortRetryIgnore".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgAbortRetryIgnore"));
            symbols.Add("vbYesNoCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYesNoCancel"));
            symbols.Add("vbYesNo".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYesNo"));
            symbols.Add("vbRetryCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgRetryCancel"));
            symbols.Add("vbCritical".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgCritical"));
            symbols.Add("vbQuestion".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgQuestion"));
            symbols.Add("vbExclamation".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgExclamation"));
            symbols.Add("vbInformation".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgInformation"));
            symbols.Add("vbDefaultButton1".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton1"));
            symbols.Add("vbDefaultButton2".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton2"));
            symbols.Add("vbDefaultButton3".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton3"));
            symbols.Add("vbDefaultButton4".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton4"));
            symbols.Add("vbApplicationModal".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgApplicationModal"));
            symbols.Add("vbSystemModal".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgSystemModal"));
            symbols.Add("vbOK".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOK"));
            symbols.Add("vbCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgCancel"));
            symbols.Add("vbAbort".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgAbort"));
            symbols.Add("vbRetry".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgRetry"));
            symbols.Add("vbIgnore".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgIgnore"));
            symbols.Add("vbYes".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYes"));
            symbols.Add("vbNo".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgNo"));
            symbols.Add("vbBinaryCompare".ToConstantSymbol(LiteralValueTypes.Integer, "Global.BinaryCompare"));
            symbols.Add("vbTextCompare".ToConstantSymbol(LiteralValueTypes.Integer, "Global.TextCompare"));

            return symbols;
        }

        public static IReadOnlyList<Symbol> VbScriptTypes()
        {
            var symbols = new List<Symbol>();

            var vbsErrClass = new ClassSymbol("_VbScriptEngineErrClass",
                                              Symbols.Members("Description".ToReadOnlyPropertySymbol(),
                                                              "HelpContext".ToReadOnlyPropertySymbol(),
                                                              "HelpFile".ToReadOnlyPropertySymbol(),
                                                              "Number".ToReadOnlyPropertySymbol(),
                                                              "Source".ToReadOnlyPropertySymbol(),
                                                              "Clear".ToFunctionSymbol(),
                                                              "Raise".ToFunctionSymbol(1, 0)),
                                              SymbolCategories.vb);
            
            symbols.Add(vbsErrClass);

            return symbols;
        }

        public static IReadOnlyList<Symbol> PageBaseSymbols()
        {
            var symbols = new List<Symbol>();

            symbols.Add(new VariableSymbol("document", Option.None, Option.None, true, DocumentClass.Some(), SymbolCategories.page));

            return symbols;
        }

        public static IReadOnlyList<Symbol> PageTypeSymbols()
        {
            var symbols = new List<Symbol>();

            var domElemMembers = new List<Symbol>();

            domElemMembers.Add("accessKey".ToPropertySymbol());
            domElemMembers.Add("addEventListener".ToFunctionSymbol());
            domElemMembers.Add("appendChild".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("attributes".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("blur".ToFunctionSymbol());
            domElemMembers.Add("childElementCount".ToReadOnlyPropertySymbol());
            domElemMembers.Add("childNodes".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("children".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("classList".ToReadOnlyPropertySymbol());
            domElemMembers.Add("className".ToPropertySymbol());
            domElemMembers.Add("click".ToFunctionSymbol());
            domElemMembers.Add("clientHeight".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientLeft".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientTop".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientWidth".ToReadOnlyPropertySymbol());
            domElemMembers.Add("cloneNode".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("closest".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("compareDocumentPosition".ToFunctionSymbol());
            domElemMembers.Add("contains".ToFunctionSymbol());
            domElemMembers.Add("contentEditable".ToPropertySymbol());
            domElemMembers.Add("dir".ToPropertySymbol());
            domElemMembers.Add("exitFullscreen".ToFunctionSymbol());
            domElemMembers.Add("firstChild".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("firstElementChild".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("focus".ToFunctionSymbol());
            domElemMembers.Add("getAttribute".ToFunctionSymbol());
            domElemMembers.Add("getAttributeNode".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("getBoundingClientRect".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("getElementsByClassName".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("getElementsByTagName".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("hasAttribute".ToFunctionSymbol());
            domElemMembers.Add("hasAttributes".ToFunctionSymbol());
            domElemMembers.Add("hasChildNodes".ToFunctionSymbol());
            domElemMembers.Add("id".ToPropertySymbol());
            domElemMembers.Add("innerHTML".ToPropertySymbol());
            domElemMembers.Add("innerText".ToPropertySymbol());
            domElemMembers.Add("insertAdjacentElement".ToFunctionSymbol());
            domElemMembers.Add("insertAdjacentHTML".ToFunctionSymbol());
            domElemMembers.Add("insertAdjacentText".ToFunctionSymbol());
            domElemMembers.Add("insertBefore".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("isContentEditable".ToReadOnlyPropertySymbol());
            domElemMembers.Add("isDefaultNamespace".ToFunctionSymbol());
            domElemMembers.Add("isEqualNode".ToFunctionSymbol());
            domElemMembers.Add("isSameNode".ToFunctionSymbol());
            domElemMembers.Add("isSupported".ToFunctionSymbol());
            domElemMembers.Add("isTextEdit".ToReadOnlyPropertySymbol("The isTextEdit property is obsolete.".Some()));
            domElemMembers.Add("lang".ToPropertySymbol());
            domElemMembers.Add("lastChild".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("lastElementChild".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("matches".ToFunctionSymbol());
            domElemMembers.Add("namespaceURI".ToReadOnlyPropertySymbol());
            domElemMembers.Add("nextSibling".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("nextElementSibling".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("nodeName".ToReadOnlyPropertySymbol());
            domElemMembers.Add("nodeType".ToReadOnlyPropertySymbol());
            domElemMembers.Add("nodeValue".ToPropertySymbol());
            domElemMembers.Add("normalize".ToFunctionSymbol());
            domElemMembers.Add("offsetHeight".ToReadOnlyPropertySymbol());
            domElemMembers.Add("offsetWidth".ToReadOnlyPropertySymbol());
            domElemMembers.Add("offsetLeft".ToReadOnlyPropertySymbol());
            domElemMembers.Add("offsetParent".ToReadOnlyPropertySymbol());
            domElemMembers.Add("offsetTop".ToReadOnlyPropertySymbol());
            domElemMembers.Add("outerHTML".ToPropertySymbol());
            domElemMembers.Add("outerText".ToPropertySymbol());
            domElemMembers.Add("ownerDocument".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("parentNode".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("parentElement".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("previousSibling".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("previousElementSibling".ToReadOnlyPropertySymbol(SymbolCategories.page));
            domElemMembers.Add("querySelector".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("querySelectorAll".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("remove".ToFunctionSymbol());
            domElemMembers.Add("removeAttribute".ToFunctionSymbol());
            domElemMembers.Add("removeAttributeNode".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("removeChild".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("removeEventListener".ToFunctionSymbol());
            domElemMembers.Add("replaceChild".ToFunctionSymbol(SymbolCategories.page));
            domElemMembers.Add("requestFullscreen".ToFunctionSymbol());
            domElemMembers.Add("scrollHeight".ToReadOnlyPropertySymbol());
            domElemMembers.Add("scrollIntoView".ToFunctionSymbol());
            domElemMembers.Add("scrollLeft".ToPropertySymbol());
            domElemMembers.Add("scrollTop".ToPropertySymbol());
            domElemMembers.Add("scrollWidth".ToReadOnlyPropertySymbol());
            domElemMembers.Add("setAttribute".ToFunctionSymbol());
            domElemMembers.Add("setAttributeNode".ToFunctionSymbol());
            domElemMembers.Add("style".ToPropertySymbol(SymbolCategories.style));
            domElemMembers.Add("runtimestyle".ToPropertySymbol("style".Some(), SymbolCategories.style));
            domElemMembers.Add("tabIndex".ToPropertySymbol());
            domElemMembers.Add("tagName".ToReadOnlyPropertySymbol());
            domElemMembers.Add("textContent".ToPropertySymbol());
            domElemMembers.Add("title".ToPropertySymbol());
            domElemMembers.Add("toString".ToFunctionSymbol());

            symbols.Add(new ClassSymbol("_Element", domElemMembers, SymbolCategories.page));

            var namedNodeMapMembers = new List<Symbol>();

            namedNodeMapMembers.Add("length".ToReadOnlyPropertySymbol());
            namedNodeMapMembers.Add("getNamedItem".ToFunctionSymbol(SymbolCategories.page));
            namedNodeMapMembers.Add("getNamedItemNS".ToFunctionSymbol());
            namedNodeMapMembers.Add("item".ToFunctionSymbol(SymbolCategories.page));
            namedNodeMapMembers.Add("removeNamedItem".ToFunctionSymbol(SymbolCategories.page));
            namedNodeMapMembers.Add("removeNamedItemNS".ToFunctionSymbol());
            namedNodeMapMembers.Add("setNamedItem".ToFunctionSymbol());
            namedNodeMapMembers.Add("setNamedItemNS".ToFunctionSymbol());

            symbols.Add(new ClassSymbol("_NamedNodeMap", namedNodeMapMembers, SymbolCategories.page));

            var collectionMembers = new List<Symbol>();

            collectionMembers.Add("length".ToReadOnlyPropertySymbol());
            collectionMembers.Add("item".ToFunctionSymbol(SymbolCategories.page));
            collectionMembers.Add("namedItem".ToFunctionSymbol(SymbolCategories.page));

            symbols.Add(new ClassSymbol("_Collection", collectionMembers, SymbolCategories.page));

            var attributeMembers = new List<Symbol>();

            attributeMembers.Add("name".ToReadOnlyPropertySymbol());
            attributeMembers.Add("value".ToPropertySymbol());
            attributeMembers.Add("specified".ToReadOnlyPropertySymbol());

            symbols.Add(new ClassSymbol("_Attr", attributeMembers, SymbolCategories.page));

            var documentMembers = new List<Symbol>();

            documentMembers.Add("activeElement".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns the currently focused element in the document
            documentMembers.Add("addEventListener".ToFunctionSymbol());  //Attaches an event handler to the document
            documentMembers.Add("adoptNode".ToFunctionSymbol());  //Adopts a node from another document
            documentMembers.Add("anchors".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("applets".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("baseURI".ToReadOnlyPropertySymbol());  //Returns the absolute base URI of a document
            documentMembers.Add("body".ToPropertySymbol(SymbolCategories.page));  //Sets or returns the document's body (the <body> element)
            documentMembers.Add("charset".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("characterSet".ToReadOnlyPropertySymbol());  //Returns the character encoding for the document
            documentMembers.Add("close".ToFunctionSymbol());  //Closes the output stream previously opened with document.open()
            documentMembers.Add("cookie".ToReadOnlyPropertySymbol());  //Returns all name/value pairs of cookies in the document
            documentMembers.Add("createAttribute".ToFunctionSymbol(SymbolCategories.page));  //Creates an attribute node
            documentMembers.Add("createComment".ToFunctionSymbol(SymbolCategories.page));  //Creates a Comment node with the specified text
            documentMembers.Add("createDocumentFragment".ToFunctionSymbol(SymbolCategories.page));  //Creates an empty DocumentFragment node
            documentMembers.Add("createElement".ToFunctionSymbol(SymbolCategories.page));  //Creates an Element node
            documentMembers.Add("createEvent".ToFunctionSymbol(SymbolCategories.page));  //Creates a new event
            documentMembers.Add("createTextNode".ToFunctionSymbol(SymbolCategories.page));  //Creates a Text node
            documentMembers.Add("defaultView".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns the window object associated with a document, or null if none is available.
            documentMembers.Add("designMode".ToPropertySymbol());  //Controls whether the entire document should be editable or not.
            documentMembers.Add("doctype".ToReadOnlyPropertySymbol());  //Returns the Document Type Declaration associated with the document
            documentMembers.Add("documentElement".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns the Document Element of the document (the <html> element)
            documentMembers.Add("documentMode".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("documentURI".ToPropertySymbol());  //Sets or returns the location of the document
            documentMembers.Add("domain".ToReadOnlyPropertySymbol());  //Returns the domain name of the server that loaded the document
            documentMembers.Add("domConfig".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("embeds".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns a collection of all <embed> elements the document
            documentMembers.Add("execCommand".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("forms".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns a collection of all <form> elements in the document
            documentMembers.Add("getElementById".ToFunctionSymbol(SymbolCategories.page));  //Returns the element that has the ID attribute with the specified value
            documentMembers.Add("getElementsByClassName".ToFunctionSymbol(SymbolCategories.page));  //Returns a HTMLCollection containing all elements with the specified class name
            documentMembers.Add("getElementsByName".ToFunctionSymbol(SymbolCategories.page));  //Deprecated
            documentMembers.Add("getElementsByTagName".ToFunctionSymbol(SymbolCategories.page));  //Returns a HTMLCollection containing all elements with the specified tag name
            documentMembers.Add("hasFocus".ToFunctionSymbol());  //Returns a Boolean value indicating whether the document has focus
            documentMembers.Add("head".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns the <head> element of the document
            documentMembers.Add("images".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns a collection of all <img> elements in the document
            documentMembers.Add("implementation".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns the DOMImplementation object that handles this document
            documentMembers.Add("importNode".ToFunctionSymbol());  //Imports a node from another document
            documentMembers.Add("inputEncoding".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("lastModified".ToReadOnlyPropertySymbol());  //Returns the date and time the document was last modified
            documentMembers.Add("links".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns a collection of all <a> and <area> elements in the document that have a href attribute
            documentMembers.Add("normalize".ToFunctionSymbol());  //Removes empty Text nodes, and joins adjacent nodes
            documentMembers.Add("normalizeDocument".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("open".ToFunctionSymbol());  //Opens an HTML output stream to collect output from document.write()
            documentMembers.Add("querySelector".ToFunctionSymbol(SymbolCategories.page));  //Returns the first element that matches a specified CSS selector(s) in the document
            documentMembers.Add("querySelectorAll".ToFunctionSymbol(SymbolCategories.page));  //Returns a static NodeList containing all elements that matches a specified CSS selector(s) in the document
            documentMembers.Add("readyState".ToReadOnlyPropertySymbol());  //Returns the (loading) status of the document
            documentMembers.Add("referrer".ToReadOnlyPropertySymbol());  //Returns the URL of the document that loaded the current document
            documentMembers.Add("removeEventListener".ToFunctionSymbol());  //Removes an event handler from the document (that has been attached with the addEventListener() method)
            documentMembers.Add("renameNode".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("scripts".ToReadOnlyPropertySymbol(SymbolCategories.page));  //Returns a collection of <script> elements in the document
            documentMembers.Add("strictErrorChecking".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("title".ToPropertySymbol());  //Sets or returns the title of the document
            documentMembers.Add("URL".ToReadOnlyPropertySymbol());  //Returns the full URL of the HTML document
            documentMembers.Add("write".ToFunctionSymbol());  //Writes HTML expressions or JavaScript code to a document
            documentMembers.Add("writeln".ToFunctionSymbol());  //Same as write(), but adds a newline character after each statement

            var docClass = new ClassSymbol(DocumentClass, documentMembers, SymbolCategories.page);

            symbols.Add(docClass);

            var tableMembers = new List<Symbol>();

            tableMembers.Add("rows".ToReadOnlyPropertySymbol(SymbolCategories.page));
            tableMembers.Add("tBodies".ToReadOnlyPropertySymbol(SymbolCategories.page));
            tableMembers.Add("caption".ToReadOnlyPropertySymbol());
            tableMembers.Add("tFoot".ToReadOnlyPropertySymbol(SymbolCategories.page));
            tableMembers.Add("tHead".ToReadOnlyPropertySymbol(SymbolCategories.page));
            tableMembers.Add("createCaption".ToFunctionSymbol());
            tableMembers.Add("createTFoot".ToFunctionSymbol(SymbolCategories.page));
            tableMembers.Add("createTHead".ToFunctionSymbol(SymbolCategories.page));
            tableMembers.Add("deleteCaption".ToFunctionSymbol());
            tableMembers.Add("deleteRow".ToFunctionSymbol());
            tableMembers.Add("deleteTFoot".ToFunctionSymbol());
            tableMembers.Add("deleteTHead".ToFunctionSymbol());
            tableMembers.Add("insertRow".ToFunctionSymbol(SymbolCategories.page));

            symbols.Add(new ClassSymbol("_HTMLTableElement", tableMembers, SymbolCategories.page));

            var imageMembers = new List<Symbol>();

            imageMembers.Add("align".ToPropertySymbol());
            imageMembers.Add("alt".ToPropertySymbol());
            imageMembers.Add("border".ToPropertySymbol());

            imageMembers.Add("complete".ToFunctionSymbol());

            imageMembers.Add("crossOrigin".ToPropertySymbol());
            imageMembers.Add("height".ToPropertySymbol());
            imageMembers.Add("hspace".ToPropertySymbol());
            imageMembers.Add("isMap".ToPropertySymbol());
            imageMembers.Add("longDesc".ToPropertySymbol());
            imageMembers.Add("lowsrc".ToPropertySymbol());
            imageMembers.Add("name".ToPropertySymbol());
            imageMembers.Add("naturalHeight".ToReadOnlyPropertySymbol());
            imageMembers.Add("naturalWidth".ToReadOnlyPropertySymbol());
            imageMembers.Add("src".ToPropertySymbol());
            imageMembers.Add("useMap".ToPropertySymbol());
            imageMembers.Add("vspace".ToPropertySymbol());
            imageMembers.Add("width".ToPropertySymbol());

            symbols.Add(new ClassSymbol("_HTMLImageElement", Option.None, Option.None, imageMembers, Option.None, SymbolCategories.page));

            return symbols;
        }

        public static IReadOnlyList<Symbol> StyleTypeSymbols()
        {
            var symbols = new List<Symbol>();

            var styleMembers = new List<Symbol>();

            styleMembers.Add("alignContent".ToPropertySymbol());
            styleMembers.Add("alignItems".ToPropertySymbol());
            styleMembers.Add("alignSelf".ToPropertySymbol());
            styleMembers.Add("animation".ToPropertySymbol());
            styleMembers.Add("animationDelay".ToPropertySymbol());
            styleMembers.Add("animationDirection".ToPropertySymbol());
            styleMembers.Add("animationDuration".ToPropertySymbol());
            styleMembers.Add("animationFillMode".ToPropertySymbol());
            styleMembers.Add("animationIterationCount".ToPropertySymbol());
            styleMembers.Add("animationName".ToPropertySymbol());
            styleMembers.Add("animationTimingFunction".ToPropertySymbol());
            styleMembers.Add("animationPlayState".ToPropertySymbol());
            styleMembers.Add("background".ToPropertySymbol());
            styleMembers.Add("backgroundAttachment".ToPropertySymbol());
            styleMembers.Add("backgroundColor".ToPropertySymbol());
            styleMembers.Add("backgroundImage".ToPropertySymbol());
            styleMembers.Add("backgroundPosition".ToPropertySymbol());
            styleMembers.Add("backgroundRepeat".ToPropertySymbol());
            styleMembers.Add("backgroundClip".ToPropertySymbol());
            styleMembers.Add("backgroundOrigin".ToPropertySymbol());
            styleMembers.Add("backgroundSize".ToPropertySymbol());
            styleMembers.Add("backfaceVisibility".ToPropertySymbol());
            styleMembers.Add("border".ToPropertySymbol());
            styleMembers.Add("borderBottom".ToPropertySymbol());
            styleMembers.Add("borderBottomColor".ToPropertySymbol());
            styleMembers.Add("borderBottomLeftRadius".ToPropertySymbol());
            styleMembers.Add("borderBottomRightRadius".ToPropertySymbol());
            styleMembers.Add("borderBottomStyle".ToPropertySymbol());
            styleMembers.Add("borderBottomWidth".ToPropertySymbol());
            styleMembers.Add("borderCollapse".ToPropertySymbol());
            styleMembers.Add("borderColor".ToPropertySymbol());
            styleMembers.Add("borderImage".ToPropertySymbol());
            styleMembers.Add("borderImageOutset".ToPropertySymbol());
            styleMembers.Add("borderImageRepeat".ToPropertySymbol());
            styleMembers.Add("borderImageSlice".ToPropertySymbol());
            styleMembers.Add("borderImageSource".ToPropertySymbol());
            styleMembers.Add("borderImageWidth".ToPropertySymbol());
            styleMembers.Add("borderLeft".ToPropertySymbol());
            styleMembers.Add("borderLeftColor".ToPropertySymbol());
            styleMembers.Add("borderLeftStyle".ToPropertySymbol());
            styleMembers.Add("borderLeftWidth".ToPropertySymbol());
            styleMembers.Add("borderRadius".ToPropertySymbol());
            styleMembers.Add("borderRight".ToPropertySymbol());
            styleMembers.Add("borderRightColor".ToPropertySymbol());
            styleMembers.Add("borderRightStyle".ToPropertySymbol());
            styleMembers.Add("borderRightWidth".ToPropertySymbol());
            styleMembers.Add("borderSpacing".ToPropertySymbol());
            styleMembers.Add("borderStyle".ToPropertySymbol());
            styleMembers.Add("borderTop".ToPropertySymbol());
            styleMembers.Add("borderTopColor".ToPropertySymbol());
            styleMembers.Add("borderTopLeftRadius".ToPropertySymbol());
            styleMembers.Add("borderTopRightRadius".ToPropertySymbol());
            styleMembers.Add("borderTopStyle".ToPropertySymbol());
            styleMembers.Add("borderTopWidth".ToPropertySymbol());
            styleMembers.Add("borderWidth".ToPropertySymbol());
            styleMembers.Add("bottom".ToPropertySymbol());
            styleMembers.Add("boxDecorationBreak".ToPropertySymbol());
            styleMembers.Add("boxShadow".ToPropertySymbol());
            styleMembers.Add("boxSizing".ToPropertySymbol());
            styleMembers.Add("captionSide".ToPropertySymbol());
            styleMembers.Add("caretColor".ToPropertySymbol());
            styleMembers.Add("clear".ToPropertySymbol());
            styleMembers.Add("clip".ToPropertySymbol());
            styleMembers.Add("color".ToPropertySymbol());
            styleMembers.Add("columnCount".ToPropertySymbol());
            styleMembers.Add("columnFill".ToPropertySymbol());
            styleMembers.Add("columnGap".ToPropertySymbol());
            styleMembers.Add("columnRule".ToPropertySymbol());
            styleMembers.Add("columnRuleColor".ToPropertySymbol());
            styleMembers.Add("columnRuleStyle".ToPropertySymbol());
            styleMembers.Add("columnRuleWidth".ToPropertySymbol());
            styleMembers.Add("columns".ToPropertySymbol());
            styleMembers.Add("columnSpan".ToPropertySymbol());
            styleMembers.Add("columnWidth".ToPropertySymbol());
            styleMembers.Add("content".ToPropertySymbol());
            styleMembers.Add("counterIncrement".ToPropertySymbol());
            styleMembers.Add("counterReset".ToPropertySymbol());
            styleMembers.Add("cursor".ToPropertySymbol("pointer".Some()));
            styleMembers.Add("direction".ToPropertySymbol());
            styleMembers.Add("display".ToPropertySymbol());
            styleMembers.Add("emptyCells".ToPropertySymbol());
            styleMembers.Add("filter".ToPropertySymbol());
            styleMembers.Add("flex".ToPropertySymbol());
            styleMembers.Add("flexBasis".ToPropertySymbol());
            styleMembers.Add("flexDirection".ToPropertySymbol());
            styleMembers.Add("flexFlow".ToPropertySymbol());
            styleMembers.Add("flexGrow".ToPropertySymbol());
            styleMembers.Add("flexShrink".ToPropertySymbol());
            styleMembers.Add("flexWrap".ToPropertySymbol());
            styleMembers.Add("cssFloat".ToPropertySymbol());
            styleMembers.Add("font".ToPropertySymbol());
            styleMembers.Add("fontFamily".ToPropertySymbol());
            styleMembers.Add("fontSize".ToPropertySymbol());
            styleMembers.Add("fontStyle".ToPropertySymbol());
            styleMembers.Add("fontVariant".ToPropertySymbol());
            styleMembers.Add("fontWeight".ToPropertySymbol());
            styleMembers.Add("fontSizeAdjust".ToPropertySymbol());
            styleMembers.Add("fontStretch".ToPropertySymbol());
            styleMembers.Add("hangingPunctuation".ToPropertySymbol());
            styleMembers.Add("height".ToPropertySymbol());
            styleMembers.Add("hyphens".ToPropertySymbol());
            styleMembers.Add("icon".ToPropertySymbol());
            styleMembers.Add("imageOrientation".ToPropertySymbol());
            styleMembers.Add("isolation".ToPropertySymbol());
            styleMembers.Add("justifyContent".ToPropertySymbol());
            styleMembers.Add("left".ToPropertySymbol());
            styleMembers.Add("letterSpacing".ToPropertySymbol());
            styleMembers.Add("lineHeight".ToPropertySymbol());
            styleMembers.Add("listStyle".ToPropertySymbol());
            styleMembers.Add("listStyleImage".ToPropertySymbol());
            styleMembers.Add("listStylePosition".ToPropertySymbol());
            styleMembers.Add("listStyleType".ToPropertySymbol());
            styleMembers.Add("margin".ToPropertySymbol());
            styleMembers.Add("marginBottom".ToPropertySymbol());
            styleMembers.Add("marginLeft".ToPropertySymbol());
            styleMembers.Add("marginRight".ToPropertySymbol());
            styleMembers.Add("marginTop".ToPropertySymbol());
            styleMembers.Add("maxHeight".ToPropertySymbol());
            styleMembers.Add("maxWidth".ToPropertySymbol());
            styleMembers.Add("minHeight".ToPropertySymbol());
            styleMembers.Add("minWidth".ToPropertySymbol());
            styleMembers.Add("navDown".ToPropertySymbol());
            styleMembers.Add("navIndex".ToPropertySymbol());
            styleMembers.Add("navLeft".ToPropertySymbol());
            styleMembers.Add("navRight".ToPropertySymbol());
            styleMembers.Add("navUp".ToPropertySymbol());
            styleMembers.Add("objectFit".ToPropertySymbol());
            styleMembers.Add("objectPosition".ToPropertySymbol());
            styleMembers.Add("opacity".ToPropertySymbol());
            styleMembers.Add("order".ToPropertySymbol());
            styleMembers.Add("orphans".ToPropertySymbol());
            styleMembers.Add("outline".ToPropertySymbol());
            styleMembers.Add("outlineColor".ToPropertySymbol());
            styleMembers.Add("outlineOffset".ToPropertySymbol());
            styleMembers.Add("outlineStyle".ToPropertySymbol());
            styleMembers.Add("outlineWidth".ToPropertySymbol());
            styleMembers.Add("overflow".ToPropertySymbol());
            styleMembers.Add("overflowX".ToPropertySymbol());
            styleMembers.Add("overflowY".ToPropertySymbol());
            styleMembers.Add("padding".ToPropertySymbol());
            styleMembers.Add("paddingBottom".ToPropertySymbol());
            styleMembers.Add("paddingLeft".ToPropertySymbol());
            styleMembers.Add("paddingRight".ToPropertySymbol());
            styleMembers.Add("paddingTop".ToPropertySymbol());
            styleMembers.Add("pageBreakAfter".ToPropertySymbol());
            styleMembers.Add("pageBreakBefore".ToPropertySymbol());
            styleMembers.Add("pageBreakInside".ToPropertySymbol());
            styleMembers.Add("perspective".ToPropertySymbol());
            styleMembers.Add("perspectiveOrigin".ToPropertySymbol());
            styleMembers.Add("position".ToPropertySymbol());
            styleMembers.Add("quotes".ToPropertySymbol());
            styleMembers.Add("resize".ToPropertySymbol());
            styleMembers.Add("right".ToPropertySymbol());
            styleMembers.Add("scrollBehavior".ToPropertySymbol());
            styleMembers.Add("tableLayout".ToPropertySymbol());
            styleMembers.Add("tabSize".ToPropertySymbol());
            styleMembers.Add("textAlign".ToPropertySymbol());
            styleMembers.Add("textAlignLast".ToPropertySymbol());
            styleMembers.Add("textDecoration".ToPropertySymbol());
            styleMembers.Add("textDecorationColor".ToPropertySymbol());
            styleMembers.Add("textDecorationLine".ToPropertySymbol());
            styleMembers.Add("textDecorationStyle".ToPropertySymbol());
            styleMembers.Add("textIndent".ToPropertySymbol());
            styleMembers.Add("textJustify".ToPropertySymbol());
            styleMembers.Add("textOverflow".ToPropertySymbol());
            styleMembers.Add("textShadow".ToPropertySymbol());
            styleMembers.Add("textTransform".ToPropertySymbol());
            styleMembers.Add("top".ToPropertySymbol());
            styleMembers.Add("transform".ToPropertySymbol());
            styleMembers.Add("transformOrigin".ToPropertySymbol());
            styleMembers.Add("transformStyle".ToPropertySymbol());
            styleMembers.Add("transition".ToPropertySymbol());
            styleMembers.Add("transitionProperty".ToPropertySymbol());
            styleMembers.Add("transitionDuration".ToPropertySymbol());
            styleMembers.Add("transitionTimingFunction".ToPropertySymbol());
            styleMembers.Add("transitionDelay".ToPropertySymbol());
            styleMembers.Add("unicodeBidi".ToPropertySymbol());
            styleMembers.Add("userSelect".ToPropertySymbol());
            styleMembers.Add("verticalAlign".ToPropertySymbol());
            styleMembers.Add("visibility".ToPropertySymbol());
            styleMembers.Add("whiteSpace".ToPropertySymbol());
            styleMembers.Add("width".ToPropertySymbol());
            styleMembers.Add("wordBreak".ToPropertySymbol());
            styleMembers.Add("wordSpacing".ToPropertySymbol());
            styleMembers.Add("wordWrap".ToPropertySymbol());
            styleMembers.Add("widows".ToPropertySymbol());
            styleMembers.Add("zIndex".ToPropertySymbol());

            // obsolete members
            styleMembers.Add("pixelheight".ToPropertySymbol("height".Some()));
            styleMembers.Add("pixelwidth".ToPropertySymbol("width".Some()));

            symbols.Add(new ClassSymbol("_HTMLStyleElement", Option.None, Option.None, styleMembers, Option.None, SymbolCategories.style));

            return symbols;
        }

        private static Symbol[] _NoMembers = new Symbol[] { };

        public static ConstantSymbol ToConstantSymbol(this String @this, LiteralValueTypes type, String? newName = null)
            => new ConstantSymbol(@this, newName.AsOption(), Option.None, true, type);

        public static FunctionSymbol ToFunctionSymbol(this String @this, params String[] tags)
            => new FunctionSymbol(@this, Option.None, Option.None, true, Option.None, _NoMembers, tags);

        public static FunctionSymbol ToFunctionSymbol(this String @this, Option<String> newName, params String[] tags)
            => new FunctionSymbol(@this, newName, Option.None, true, Option.None, _NoMembers, tags);

        private static IEnumerable<ArgumentSymbol> CreateArgPlaceHolders(Int32 required, Int32 optional)
            => Enumerable.Range(0, (required + optional))
                         .Select(i => new ArgumentSymbol(String.Concat('_', (Char)('a' + i)), Option.None, Option.None, false, !(i < required)));

        public static FunctionSymbol ToFunctionSymbol(this String @this, Int32 requiredArgs, Int32 optionalArgs, params String[] tags)
            => new FunctionSymbol(@this, Option.None, Option.None, true, Option.None, CreateArgPlaceHolders(requiredArgs, optionalArgs), tags);

        public static FunctionSymbol ToFunctionSymbol(this String @this, Option<String> newName, Int32 requiredArgs, Int32 optionalArgs, params String[] tags)
            => new FunctionSymbol(@this, newName, Option.None, true, Option.None, CreateArgPlaceHolders(requiredArgs, optionalArgs), tags);


        public static FunctionSymbol ToErrFunctionSymbol(this String @this, String errorMessage)
            => new FunctionSymbol(@this, Option.None, errorMessage.Some(), true, Option.None, _NoMembers);

        public static PropertySymbol ToPropertySymbol(this String @this, params String[] tags)
            => new PropertySymbol(@this,
                                  (new PropertyGetSymbol(@this, true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(),
                                  (new PropertySetSymbol(@this, true, Option.None, false, _NoMembers, tags)).Some<FunctionSymbol>());

        public static PropertySymbol ToPropertySymbol(this String @this, Option<String> newName, params String[] tags)
            => new PropertySymbol(@this,
                                  (new PropertyGetSymbol(newName.Or(@this), true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(),
                                  (new PropertySetSymbol(newName.Or(@this), true, Option.None, false, _NoMembers, tags)).Some<FunctionSymbol>());
        public static PropertySymbol ToPropertySymbol(this String @this, Option<String> newName, Option<String> message, params String[] tags)
            => new PropertySymbol(@this,
                                  (new PropertyGetSymbol(newName.Or(@this), message, true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(),
                                  (new PropertySetSymbol(newName.Or(@this), message, true, Option.None, false, _NoMembers, tags)).Some<FunctionSymbol>());

        public static PropertySymbol ToReadOnlyPropertySymbol(this String @this, params String[] tags)
            => new PropertySymbol(@this, (new PropertyGetSymbol(@this, true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(), Option.None);
        public static PropertySymbol ToReadOnlyPropertySymbol(this String @this, Option<String> message, params String[] tags)
            => new PropertySymbol(@this, (new PropertyGetSymbol(@this, message, true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(), Option.None);
        public static PropertySymbol ToWriteOnlyPropertySymbol(this String @this, params String[] tags)
            => new PropertySymbol(@this, Option.None, (new PropertySetSymbol(@this, true, Option.None, false, _NoMembers, tags)).Some<FunctionSymbol>());

        public static VariableSymbol ToVariableSymbol(this String @this, Option<String> newName, ClassSymbol? @class = null, String? message = null)
            => new VariableSymbol(@this, newName, message.AsOption(), true, @class == null ? Option.None : @class.Name.Some());

        public static VariableSymbol ToVariableSymbol(this String @this, Option<String> newName, params String[] tags)
            => new VariableSymbol(@this, newName, Option.None, true, Option.None, tags);

        public static IEnumerable<Symbol> Members(params Symbol[] members)
            => members;

        internal static (List<Symbol> Members, Dictionary<String, Symbol> Index) Distinct(IEnumerable<Symbol> raw)
        {
            var symbols = new List<Symbol>();
            var unique  = new Dictionary<String, Symbol>(StringComparer.OrdinalIgnoreCase);

            foreach (var symbol in raw)
            {
                var name = symbol.Name;

                if (unique.ContainsKey(name))
                {
                    symbols.Remove(unique[name]);
                    unique.Remove(symbol.Name);
                }

                symbols.Add(symbol);
                unique.Add(name, symbol);
            }

            return (symbols, unique);
        }

        public static String BlockSymbolName(this IfStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this ElseBlock @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this SelectCase @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this ForEachStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this ForStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this DoLoopStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this DoLoopTestStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this DoTestLoopStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this WhileStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());
        public static String BlockSymbolName(this WithStatement @this)
            => String.Concat(@this.LeadToken().Span.Position.ToString());

        
    }

}

