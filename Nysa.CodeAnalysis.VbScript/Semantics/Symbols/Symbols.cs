using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class Symbols
    {
        /// <summary>
        /// Creates the standard set of host (global) symbols provided by the VbScript runtime engine.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<Symbol> VbScriptHostSymbols()
        {
            var vbsErrClass = new ClassSymbol("_VbScriptEngineErrClass", Symbols.Members("Description".ToReadOnlyPropertySymbol(),
                                                                                         "HelpContext".ToReadOnlyPropertySymbol(),
                                                                                         "HelpFile".ToReadOnlyPropertySymbol(),
                                                                                         "Number".ToReadOnlyPropertySymbol(),
                                                                                         "Source".ToReadOnlyPropertySymbol(),
                                                                                         "Clear".ToFunctionSymbol(),
                                                                                         "Raise".ToFunctionSymbol()));
            
            var hostMembers = new List<Symbol>();

            hostMembers.Add(vbsErrClass);
            hostMembers.Add("Err".ToVariableSymbol(Option.None, vbsErrClass, "Err object has no equivalent translation."));
            hostMembers.Add("me".ToVariableSymbol("this".Some()));

            hostMembers.Add(new ClassSymbol("_anytype", Return.Enumerable("runtimestyle".ToPropertySymbol("style".Some()))));

            hostMembers.Add("Abs".ToFunctionSymbol("Global.Abs".Some()));
            hostMembers.Add("Array".ToFunctionSymbol());
            hostMembers.Add("Asc".ToFunctionSymbol("Global.Asc".Some()));
            hostMembers.Add("Atn".ToFunctionSymbol());
            hostMembers.Add("CBool".ToFunctionSymbol("Global.CBool".Some()));
            hostMembers.Add("CByte".ToFunctionSymbol());
            hostMembers.Add("CCur".ToFunctionSymbol());
            hostMembers.Add("CDate".ToFunctionSymbol());
            hostMembers.Add("CDbl".ToFunctionSymbol("Global.CDbl".Some()));
            hostMembers.Add("CInt".ToFunctionSymbol("Global.CInt".Some()));
            hostMembers.Add("CLng".ToFunctionSymbol("Global.CLng".Some()));
            hostMembers.Add("Cos".ToFunctionSymbol());
            hostMembers.Add("CSng".ToFunctionSymbol("Global.CSng".Some()));
            hostMembers.Add("CStr".ToFunctionSymbol("Global.CStr".Some()));
            hostMembers.Add("Date".ToFunctionSymbol("Global.Date".Some()));
            hostMembers.Add("DateAdd".ToFunctionSymbol());
            hostMembers.Add("DateDiff".ToFunctionSymbol());
            hostMembers.Add("DatePart".ToFunctionSymbol());
            hostMembers.Add("DateSerial".ToFunctionSymbol());
            hostMembers.Add("DateValue".ToFunctionSymbol());
            hostMembers.Add("Day".ToFunctionSymbol("Global.Day".Some()));
            hostMembers.Add("Eval".ToErrFunctionSymbol("Eval has no equivalent translation."));
            hostMembers.Add("Execute".ToErrFunctionSymbol("Execute has no equivalent translation."));
            hostMembers.Add("Exp".ToFunctionSymbol());
            hostMembers.Add("Filter".ToFunctionSymbol());
            hostMembers.Add("Fix".ToFunctionSymbol("Global.Fix".Some()));
            hostMembers.Add("FormatCurrency".ToFunctionSymbol());
            hostMembers.Add("FormatDateTime".ToFunctionSymbol());
            hostMembers.Add("FormatNumber".ToFunctionSymbol("Global.FormatNumber".Some()));
            hostMembers.Add("FormatPercent".ToFunctionSymbol());
            hostMembers.Add("GetLocale".ToFunctionSymbol());
            hostMembers.Add("GetObject".ToFunctionSymbol());
            hostMembers.Add("GetRef".ToFunctionSymbol("Global.GetRef".Some()));
            hostMembers.Add("Hex".ToFunctionSymbol("Global.Hex".Some()));
            hostMembers.Add("Hour".ToFunctionSymbol("Global.Hour".Some()));
            hostMembers.Add("Chr".ToFunctionSymbol("Global.Chr".Some()));
            hostMembers.Add("InputBox".ToFunctionSymbol());
            hostMembers.Add("InStr".ToFunctionSymbol("Global.InStr".Some()));
            hostMembers.Add("InStrRev".ToFunctionSymbol("Global.InStrRev".Some()));
            hostMembers.Add("Int".ToFunctionSymbol("Global.Int".Some()));
            hostMembers.Add("IsArray".ToFunctionSymbol("Global.IsArray".Some()));
            hostMembers.Add("IsDate".ToFunctionSymbol("Global.IsDate".Some()));
            hostMembers.Add("IsEmpty".ToFunctionSymbol("Global.IsEmpty".Some()));
            hostMembers.Add("IsNull".ToFunctionSymbol("Global.IsNullOrUndefined".Some()));
            hostMembers.Add("IsNumeric".ToFunctionSymbol("Global.IsNumeric".Some()));
            hostMembers.Add("IsObject".ToFunctionSymbol("Global.IsObject".Some()));
            hostMembers.Add("Join".ToFunctionSymbol("Global.Join".Some()));
            hostMembers.Add("LBound".ToFunctionSymbol("Global.LBound".Some()));
            hostMembers.Add("LCase".ToFunctionSymbol("Global.LCase".Some()));
            hostMembers.Add("Left".ToFunctionSymbol("Global.Left".Some()));
            hostMembers.Add("Len".ToFunctionSymbol("Global.Len".Some()));
            hostMembers.Add("LoadPicture".ToFunctionSymbol());
            hostMembers.Add("Log".ToFunctionSymbol());
            hostMembers.Add("LTrim".ToFunctionSymbol("Global.LTrim".Some()));
            hostMembers.Add("Mid".ToFunctionSymbol("Global.Mid".Some()));
            hostMembers.Add("Minute".ToFunctionSymbol("Global.Minute".Some()));
            hostMembers.Add("Month".ToFunctionSymbol("Global.Month".Some()));
            hostMembers.Add("MonthName".ToFunctionSymbol());
            //hostMembers.Add("MsgBox".ToFunctionSymbol("Global.MsgBox".Some()));   // going to map this to an IUnityDocument
            hostMembers.Add("Now".ToFunctionSymbol("Global.Now".Some()));
            hostMembers.Add("Oct".ToFunctionSymbol());
            hostMembers.Add("Replace".ToFunctionSymbol("Global.Replace".Some()));
            hostMembers.Add("RGB".ToFunctionSymbol("Global.RGB".Some()));
            hostMembers.Add("Right".ToFunctionSymbol("Global.Right".Some()));
            hostMembers.Add("Rnd".ToFunctionSymbol());
            hostMembers.Add("Round".ToFunctionSymbol("Global.Round".Some()));
            hostMembers.Add("RTrim".ToFunctionSymbol("Global.RTrim".Some()));
            hostMembers.Add("ScriptEngine".ToFunctionSymbol());
            hostMembers.Add("ScriptEngineBuildVersion".ToFunctionSymbol());
            hostMembers.Add("ScriptEngineMajorVersion".ToFunctionSymbol());
            hostMembers.Add("ScriptEngineMinorVersion".ToFunctionSymbol());
            hostMembers.Add("Second".ToFunctionSymbol("Global.Second".Some()));
            hostMembers.Add("SetLocale".ToFunctionSymbol());
            hostMembers.Add("Sgn".ToFunctionSymbol());
            hostMembers.Add("Sin".ToFunctionSymbol());
            hostMembers.Add("Space".ToFunctionSymbol("Global.Space".Some()));
            hostMembers.Add("Split".ToFunctionSymbol("Global.Split".Some()));
            hostMembers.Add("Sqr".ToFunctionSymbol());
            hostMembers.Add("StrComp".ToFunctionSymbol("Global.StrComp".Some()));
            hostMembers.Add("String".ToFunctionSymbol("Global.String".Some()));
            hostMembers.Add("StrReverse".ToFunctionSymbol("Global.StrReverse".Some()));
            hostMembers.Add("Tan".ToFunctionSymbol());
            hostMembers.Add("Time".ToFunctionSymbol());
            hostMembers.Add("Timer".ToFunctionSymbol());
            hostMembers.Add("TimeSerial".ToFunctionSymbol());
            hostMembers.Add("TimeValue".ToFunctionSymbol());
            hostMembers.Add("Trim".ToFunctionSymbol("Global.Trim".Some()));
            hostMembers.Add("TypeName".ToErrFunctionSymbol("TypeName has no equivalent translation."));
            hostMembers.Add("UBound".ToFunctionSymbol("Global.UBound".Some()));
            hostMembers.Add("UCase".ToFunctionSymbol("Global.UCase".Some()));
            hostMembers.Add("VarType".ToErrFunctionSymbol("VarType has no equivalent translation."));
            hostMembers.Add("Weekday".ToFunctionSymbol());
            hostMembers.Add("WeekdayName".ToFunctionSymbol());
            hostMembers.Add("Year".ToFunctionSymbol("Global.Year".Some()));
            hostMembers.Add("Randomize".ToFunctionSymbol());
            hostMembers.Add("vbObjectError".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbObjectError"));
            hostMembers.Add("vbTrue".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbTrue"));
            hostMembers.Add("vbFalse".ToConstantSymbol(LiteralValueTypes.Integer, "Global.vbFalse"));

            hostMembers.Add("vbEmpty".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeEmpty"));
            hostMembers.Add("vbNull".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeNull"));
            hostMembers.Add("vbBoolean".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeBoolean"));
            hostMembers.Add("vbByte".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeByte"));
            hostMembers.Add("vbInteger".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeInteger"));
            hostMembers.Add("vbLong".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeLong"));
            hostMembers.Add("vbSingle".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeSingle"));
            hostMembers.Add("vbDouble".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeDouble"));
            hostMembers.Add("vbDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeDate"));
            hostMembers.Add("vbString".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeString"));
            hostMembers.Add("vbObject".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeObject"));
            hostMembers.Add("vbArray".ToConstantSymbol(LiteralValueTypes.Integer, "Global.typeArray"));

            hostMembers.Add("vbCr".ToConstantSymbol(LiteralValueTypes.String, "Global.Cr"));
            hostMembers.Add("vbCrLf".ToConstantSymbol(LiteralValueTypes.String, "Global.CrLf"));
            hostMembers.Add("vbFormFeed".ToConstantSymbol(LiteralValueTypes.String, "Global.FormFeed"));
            hostMembers.Add("vbLf".ToConstantSymbol(LiteralValueTypes.String, "Global.Lf"));
            hostMembers.Add("vbNewLine".ToConstantSymbol(LiteralValueTypes.String, "Global.NewLine"));
            hostMembers.Add("vbNullChar".ToConstantSymbol(LiteralValueTypes.String, "Global.NullChar"));
            hostMembers.Add("vbNullString".ToConstantSymbol(LiteralValueTypes.String, "Global.NullString"));
            hostMembers.Add("vbTab".ToConstantSymbol(LiteralValueTypes.String, "Global.Tab"));
            hostMembers.Add("vbVerticalTab".ToConstantSymbol(LiteralValueTypes.String, "Global.VerticalTab"));

            hostMembers.Add("vbSunday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Sunday"));
            hostMembers.Add("vbMonday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Monday"));
            hostMembers.Add("vbTuesday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Tuesday"));
            hostMembers.Add("vbWednesday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Wednesday"));
            hostMembers.Add("vbThursday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Thursday"));
            hostMembers.Add("vbFriday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Friday"));
            hostMembers.Add("vbSaturday".ToConstantSymbol(LiteralValueTypes.Integer, "Global.Saturday"));

            hostMembers.Add("vbUseSystemDayOfWeek".ToConstantSymbol(LiteralValueTypes.Integer, "Global.UseSystemDayOfWeek"));
            hostMembers.Add("vbLongDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtLongDate"));
            hostMembers.Add("vbShortDate".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtShortDate"));
            hostMembers.Add("vbLongTime".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtLongTime"));
            hostMembers.Add("vbShortTime".ToConstantSymbol(LiteralValueTypes.Integer, "Global.fmtShortTime"));
            hostMembers.Add("vbOKOnly".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOKOnly"));
            hostMembers.Add("vbOKCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOKCancel"));
            hostMembers.Add("vbAbortRetryIgnore".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgAbortRetryIgnore"));
            hostMembers.Add("vbYesNoCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYesNoCancel"));
            hostMembers.Add("vbYesNo".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYesNo"));
            hostMembers.Add("vbRetryCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgRetryCancel"));
            hostMembers.Add("vbCritical".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgCritical"));
            hostMembers.Add("vbQuestion".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgQuestion"));
            hostMembers.Add("vbExclamation".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgExclamation"));
            hostMembers.Add("vbInformation".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgInformation"));
            hostMembers.Add("vbDefaultButton1".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton1"));
            hostMembers.Add("vbDefaultButton2".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton2"));
            hostMembers.Add("vbDefaultButton3".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton3"));
            hostMembers.Add("vbDefaultButton4".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgDefaultButton4"));
            hostMembers.Add("vbApplicationModal".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgApplicationModal"));
            hostMembers.Add("vbSystemModal".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgSystemModal"));
            hostMembers.Add("vbOK".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgOK"));
            hostMembers.Add("vbCancel".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgCancel"));
            hostMembers.Add("vbAbort".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgAbort"));
            hostMembers.Add("vbRetry".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgRetry"));
            hostMembers.Add("vbIgnore".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgIgnore"));
            hostMembers.Add("vbYes".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgYes"));
            hostMembers.Add("vbNo".ToConstantSymbol(LiteralValueTypes.Integer, "Global.msgNo"));
            hostMembers.Add("vbBinaryCompare".ToConstantSymbol(LiteralValueTypes.Integer, "Global.BinaryCompare"));
            hostMembers.Add("vbTextCompare".ToConstantSymbol(LiteralValueTypes.Integer, "Global.TextCompare"));

            return hostMembers;
        }

        public static IReadOnlyList<Symbol> WebApiSymbols()
        {
            var hostMembers = new List<Symbol>();

            var domElemMembers = new List<Symbol>();

            domElemMembers.Add("accessKey".ToPropertySymbol());
            domElemMembers.Add("addEventListener".ToFunctionSymbol());
            domElemMembers.Add("appendChild".ToFunctionSymbol("page"));
            domElemMembers.Add("attributes".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("blur".ToFunctionSymbol());
            domElemMembers.Add("childElementCount".ToReadOnlyPropertySymbol());
            domElemMembers.Add("childNodes".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("children".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("classList".ToReadOnlyPropertySymbol());
            domElemMembers.Add("className".ToPropertySymbol());
            domElemMembers.Add("click".ToFunctionSymbol());
            domElemMembers.Add("clientHeight".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientLeft".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientTop".ToReadOnlyPropertySymbol());
            domElemMembers.Add("clientWidth".ToReadOnlyPropertySymbol());
            domElemMembers.Add("cloneNode".ToFunctionSymbol("page"));
            domElemMembers.Add("closest".ToFunctionSymbol("page"));
            domElemMembers.Add("compareDocumentPosition".ToFunctionSymbol());
            domElemMembers.Add("contains".ToFunctionSymbol());
            domElemMembers.Add("contentEditable".ToPropertySymbol());
            domElemMembers.Add("dir".ToPropertySymbol());
            domElemMembers.Add("exitFullscreen".ToFunctionSymbol());
            domElemMembers.Add("firstChild".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("firstElementChild".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("focus".ToFunctionSymbol());
            domElemMembers.Add("getAttribute".ToFunctionSymbol());
            domElemMembers.Add("getAttributeNode".ToFunctionSymbol("page"));
            domElemMembers.Add("getBoundingClientRect".ToFunctionSymbol("page"));
            domElemMembers.Add("getElementsByClassName".ToFunctionSymbol("page"));
            domElemMembers.Add("getElementsByTagName".ToFunctionSymbol("page"));
            domElemMembers.Add("hasAttribute".ToFunctionSymbol());
            domElemMembers.Add("hasAttributes".ToFunctionSymbol());
            domElemMembers.Add("hasChildNodes".ToFunctionSymbol());
            domElemMembers.Add("id".ToPropertySymbol());
            domElemMembers.Add("innerHTML".ToPropertySymbol());
            domElemMembers.Add("innerText".ToPropertySymbol());
            domElemMembers.Add("insertAdjacentElement".ToFunctionSymbol());
            domElemMembers.Add("insertAdjacentHTML".ToFunctionSymbol());
            domElemMembers.Add("insertAdjacentText".ToFunctionSymbol());
            domElemMembers.Add("insertBefore".ToFunctionSymbol("page"));
            domElemMembers.Add("isContentEditable".ToReadOnlyPropertySymbol());
            domElemMembers.Add("isDefaultNamespace".ToFunctionSymbol());
            domElemMembers.Add("isEqualNode".ToFunctionSymbol());
            domElemMembers.Add("isSameNode".ToFunctionSymbol());
            domElemMembers.Add("isSupported".ToFunctionSymbol());
            domElemMembers.Add("lang".ToPropertySymbol());
            domElemMembers.Add("lastChild".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("lastElementChild".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("matches".ToFunctionSymbol());
            domElemMembers.Add("namespaceURI".ToReadOnlyPropertySymbol());
            domElemMembers.Add("nextSibling".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("nextElementSibling".ToReadOnlyPropertySymbol("page"));
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
            domElemMembers.Add("ownerDocument".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("parentNode".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("parentElement".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("previousSibling".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("previousElementSibling".ToReadOnlyPropertySymbol("page"));
            domElemMembers.Add("querySelector".ToFunctionSymbol("page"));
            domElemMembers.Add("querySelectorAll".ToFunctionSymbol("page"));
            domElemMembers.Add("remove".ToFunctionSymbol());
            domElemMembers.Add("removeAttribute".ToFunctionSymbol());
            domElemMembers.Add("removeAttributeNode".ToFunctionSymbol("page"));
            domElemMembers.Add("removeChild".ToFunctionSymbol("page"));
            domElemMembers.Add("removeEventListener".ToFunctionSymbol());
            domElemMembers.Add("replaceChild".ToFunctionSymbol("page"));
            domElemMembers.Add("requestFullscreen".ToFunctionSymbol());
            domElemMembers.Add("scrollHeight".ToReadOnlyPropertySymbol());
            domElemMembers.Add("scrollIntoView".ToFunctionSymbol());
            domElemMembers.Add("scrollLeft".ToPropertySymbol());
            domElemMembers.Add("scrollTop".ToPropertySymbol());
            domElemMembers.Add("scrollWidth".ToReadOnlyPropertySymbol());
            domElemMembers.Add("setAttribute".ToFunctionSymbol());
            domElemMembers.Add("setAttributeNode".ToFunctionSymbol());
            domElemMembers.Add("style".ToPropertySymbol("page"));
            domElemMembers.Add("runtimestyle".ToPropertySymbol("style".Some(), "page"));
            domElemMembers.Add("tabIndex".ToPropertySymbol());
            domElemMembers.Add("tagName".ToReadOnlyPropertySymbol());
            domElemMembers.Add("textContent".ToPropertySymbol());
            domElemMembers.Add("title".ToPropertySymbol());
            domElemMembers.Add("toString".ToFunctionSymbol());

            hostMembers.Add(new ClassSymbol("_Element", domElemMembers));

            var namedNodeMapMembers = new List<Symbol>();

            namedNodeMapMembers.Add("length".ToReadOnlyPropertySymbol());
            namedNodeMapMembers.Add("getNamedItem".ToFunctionSymbol("page"));
            namedNodeMapMembers.Add("getNamedItemNS".ToFunctionSymbol());
            namedNodeMapMembers.Add("item".ToFunctionSymbol("page"));
            namedNodeMapMembers.Add("removeNamedItem".ToFunctionSymbol("page"));
            namedNodeMapMembers.Add("removeNamedItemNS".ToFunctionSymbol());
            namedNodeMapMembers.Add("setNamedItem".ToFunctionSymbol());
            namedNodeMapMembers.Add("setNamedItemNS".ToFunctionSymbol());

            hostMembers.Add(new ClassSymbol("_NamedNodeMap", namedNodeMapMembers));

            var collectionMembers = new List<Symbol>();

            collectionMembers.Add("length".ToReadOnlyPropertySymbol());
            collectionMembers.Add("item".ToFunctionSymbol("page"));
            collectionMembers.Add("namedItem".ToFunctionSymbol("page"));

            hostMembers.Add(new ClassSymbol("_Collection", collectionMembers));

            var attributeMembers = new List<Symbol>();

            attributeMembers.Add("name".ToReadOnlyPropertySymbol());
            attributeMembers.Add("value".ToPropertySymbol());
            attributeMembers.Add("specified".ToReadOnlyPropertySymbol());

            hostMembers.Add(new ClassSymbol("_Attr", attributeMembers));

            var documentMembers = new List<Symbol>();

            documentMembers.Add("activeElement".ToReadOnlyPropertySymbol("page"));  //Returns the currently focused element in the document
            documentMembers.Add("addEventListener".ToFunctionSymbol());  //Attaches an event handler to the document
            documentMembers.Add("adoptNode".ToFunctionSymbol());  //Adopts a node from another document
            documentMembers.Add("anchors".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("applets".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("baseURI".ToReadOnlyPropertySymbol());  //Returns the absolute base URI of a document
            documentMembers.Add("body".ToPropertySymbol("page"));  //Sets or returns the document's body (the <body> element)
            documentMembers.Add("charset".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("characterSet".ToReadOnlyPropertySymbol());  //Returns the character encoding for the document
            documentMembers.Add("close".ToFunctionSymbol());  //Closes the output stream previously opened with document.open()
            documentMembers.Add("cookie".ToReadOnlyPropertySymbol());  //Returns all name/value pairs of cookies in the document
            documentMembers.Add("createAttribute".ToFunctionSymbol("page"));  //Creates an attribute node
            documentMembers.Add("createComment".ToFunctionSymbol("page"));  //Creates a Comment node with the specified text
            documentMembers.Add("createDocumentFragment".ToFunctionSymbol("page"));  //Creates an empty DocumentFragment node
            documentMembers.Add("createElement".ToFunctionSymbol("page"));  //Creates an Element node
            documentMembers.Add("createEvent".ToFunctionSymbol("page"));  //Creates a new event
            documentMembers.Add("createTextNode".ToFunctionSymbol("page"));  //Creates a Text node
            documentMembers.Add("defaultView".ToReadOnlyPropertySymbol("page"));  //Returns the window object associated with a document, or null if none is available.
            documentMembers.Add("designMode".ToPropertySymbol());  //Controls whether the entire document should be editable or not.
            documentMembers.Add("doctype".ToReadOnlyPropertySymbol());  //Returns the Document Type Declaration associated with the document
            documentMembers.Add("documentElement".ToReadOnlyPropertySymbol("page"));  //Returns the Document Element of the document (the <html> element)
            documentMembers.Add("documentMode".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("documentURI".ToPropertySymbol());  //Sets or returns the location of the document
            documentMembers.Add("domain".ToReadOnlyPropertySymbol());  //Returns the domain name of the server that loaded the document
            documentMembers.Add("domConfig".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("embeds".ToReadOnlyPropertySymbol("page"));  //Returns a collection of all <embed> elements the document
            documentMembers.Add("execCommand".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("forms".ToReadOnlyPropertySymbol("page"));  //Returns a collection of all <form> elements in the document
            documentMembers.Add("getElementById".ToFunctionSymbol("page"));  //Returns the element that has the ID attribute with the specified value
            documentMembers.Add("getElementsByClassName".ToFunctionSymbol("page"));  //Returns a HTMLCollection containing all elements with the specified class name
            documentMembers.Add("getElementsByName".ToFunctionSymbol("page"));  //Deprecated
            documentMembers.Add("getElementsByTagName".ToFunctionSymbol("page"));  //Returns a HTMLCollection containing all elements with the specified tag name
            documentMembers.Add("hasFocus".ToFunctionSymbol());  //Returns a Boolean value indicating whether the document has focus
            documentMembers.Add("head".ToReadOnlyPropertySymbol("page"));  //Returns the <head> element of the document
            documentMembers.Add("images".ToReadOnlyPropertySymbol("page"));  //Returns a collection of all <img> elements in the document
            documentMembers.Add("implementation".ToReadOnlyPropertySymbol("page"));  //Returns the DOMImplementation object that handles this document
            documentMembers.Add("importNode".ToFunctionSymbol());  //Imports a node from another document
            documentMembers.Add("inputEncoding".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("lastModified".ToReadOnlyPropertySymbol());  //Returns the date and time the document was last modified
            documentMembers.Add("links".ToReadOnlyPropertySymbol("page"));  //Returns a collection of all <a> and <area> elements in the document that have a href attribute
            documentMembers.Add("normalize".ToFunctionSymbol());  //Removes empty Text nodes, and joins adjacent nodes
            documentMembers.Add("normalizeDocument".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("open".ToFunctionSymbol());  //Opens an HTML output stream to collect output from document.write()
            documentMembers.Add("querySelector".ToFunctionSymbol("page"));  //Returns the first element that matches a specified CSS selector(s) in the document
            documentMembers.Add("querySelectorAll".ToFunctionSymbol("page"));  //Returns a static NodeList containing all elements that matches a specified CSS selector(s) in the document
            documentMembers.Add("readyState".ToReadOnlyPropertySymbol());  //Returns the (loading) status of the document
            documentMembers.Add("referrer".ToReadOnlyPropertySymbol());  //Returns the URL of the document that loaded the current document
            documentMembers.Add("removeEventListener".ToFunctionSymbol());  //Removes an event handler from the document (that has been attached with the addEventListener() method)
            documentMembers.Add("renameNode".ToFunctionSymbol());  //Deprecated
            documentMembers.Add("scripts".ToReadOnlyPropertySymbol("page"));  //Returns a collection of <script> elements in the document
            documentMembers.Add("strictErrorChecking".ToReadOnlyPropertySymbol());  //Deprecated
            documentMembers.Add("title".ToPropertySymbol());  //Sets or returns the title of the document
            documentMembers.Add("URL".ToReadOnlyPropertySymbol());  //Returns the full URL of the HTML document
            documentMembers.Add("write".ToFunctionSymbol());  //Writes HTML expressions or JavaScript code to a document
            documentMembers.Add("writeln".ToFunctionSymbol());  //Same as write(), but adds a newline character after each statement

            var docClass = new ClassSymbol("_Document", documentMembers);

            hostMembers.Add(docClass);

            hostMembers.Add(new VariableSymbol("document", Option.None, Option.None, true, docClass.Name.Some()));

            var tableMembers = new List<Symbol>();

            tableMembers.Add("rows".ToReadOnlyPropertySymbol("page"));
            tableMembers.Add("tBodies".ToReadOnlyPropertySymbol("page"));
            tableMembers.Add("caption".ToReadOnlyPropertySymbol());
            tableMembers.Add("tFoot".ToReadOnlyPropertySymbol("page"));
            tableMembers.Add("tHead".ToReadOnlyPropertySymbol("page"));
            tableMembers.Add("createCaption".ToFunctionSymbol());
            tableMembers.Add("createTFoot".ToFunctionSymbol("page"));
            tableMembers.Add("createTHead".ToFunctionSymbol("page"));
            tableMembers.Add("deleteCaption".ToFunctionSymbol());
            tableMembers.Add("deleteRow".ToFunctionSymbol());
            tableMembers.Add("deleteTFoot".ToFunctionSymbol());
            tableMembers.Add("deleteTHead".ToFunctionSymbol());
            tableMembers.Add("insertRow".ToFunctionSymbol("page"));

            hostMembers.Add(new ClassSymbol("_HTMLTableElement", tableMembers));

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

            hostMembers.Add(new ClassSymbol("_HTMLStyleElement", Option.None, Option.None, styleMembers, Option.None));


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

            hostMembers.Add(new ClassSymbol("_HTMLImageElement", Option.None, Option.None, imageMembers, Option.None));

            return hostMembers;
        }

        private static Symbol[] _NoMembers = new Symbol[] { };

        public static ConstantSymbol ToConstantSymbol(this String @this, LiteralValueTypes type, String? newName = null)
            => new ConstantSymbol(@this, newName.AsOption(), Option.None, true, type);

        public static FunctionSymbol ToFunctionSymbol(this String @this, params String[] tags)
            => new FunctionSymbol(@this, Option.None, Option.None, true, Option.None, _NoMembers, tags);

        public static FunctionSymbol ToFunctionSymbol(this String @this, Option<String> newName, params String[] tags)
            => new FunctionSymbol(@this, newName, Option.None, true, Option.None, _NoMembers, tags);

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

        public static PropertySymbol ToReadOnlyPropertySymbol(this String @this, params String[] tags)
            => new PropertySymbol(@this, (new PropertyGetSymbol(@this, true, Option.None, _NoMembers, tags)).Some<FunctionSymbol>(), Option.None);
        public static PropertySymbol ToWriteOnlyPropertySymbol(this String @this, params String[] tags)
            => new PropertySymbol(@this, Option.None, (new PropertySetSymbol(@this, true, Option.None, false, _NoMembers, tags)).Some<FunctionSymbol>());

        public static VariableSymbol ToVariableSymbol(this String @this, Option<String> newName, ClassSymbol? @class = null, String? errMessage = null)
            => new VariableSymbol(@this, newName, errMessage.AsOption(), true, @class == null ? Option.None : @class.Name.Some());

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

