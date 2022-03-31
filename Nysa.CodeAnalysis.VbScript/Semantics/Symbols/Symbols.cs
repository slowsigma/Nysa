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
            var vbsErrClass = new ClassSymbol("_VbScriptEngineErrClass", Symbols.Members("Description".EmptyProperty(true),
                                                                                         "HelpContext".EmptyProperty(true),
                                                                                         "HelpFile".EmptyProperty(true),
                                                                                         "Number".EmptyProperty(true, true),
                                                                                         "Source".EmptyProperty(true),
                                                                                         "Clear".EmptyFunction(),
                                                                                         "Raise".EmptyFunction()));
            
            var hostMembers = new List<Symbol>();

            hostMembers.Add(vbsErrClass);
            hostMembers.Add("Err".ToVariable(true, vbsErrClass, "Err object has no equivalent translation."));
            hostMembers.Add("me".ToVariable(true).Renamed("this"));

            hostMembers.Add("Abs".EmptyFunction().Renamed("Global.Abs"));
            hostMembers.Add("Array".EmptyFunction());
            hostMembers.Add("Asc".EmptyFunction().Renamed("Global.Asc"));
            hostMembers.Add("Atn".EmptyFunction());
            hostMembers.Add("CBool".EmptyFunction().Renamed("Global.CBool"));
            hostMembers.Add("CByte".EmptyFunction());
            hostMembers.Add("CCur".EmptyFunction());
            hostMembers.Add("CDate".EmptyFunction());
            hostMembers.Add("CDbl".EmptyFunction().Renamed("Global.CDbl"));
            hostMembers.Add("CInt".EmptyFunction().Renamed("Global.CInt"));
            hostMembers.Add("CLng".EmptyFunction().Renamed("Global.CLng"));
            hostMembers.Add("Cos".EmptyFunction());
            hostMembers.Add("CreateObject".EmptyFunction().Renamed("Global.external.CreateObject"));
            hostMembers.Add("CSng".EmptyFunction().Renamed("Global.CSng"));
            hostMembers.Add("CStr".EmptyFunction().Renamed("Global.CStr"));
            hostMembers.Add("Date".EmptyFunction().Renamed("Global.Date"));
            hostMembers.Add("DateAdd".EmptyFunction());
            hostMembers.Add("DateDiff".EmptyFunction());
            hostMembers.Add("DatePart".EmptyFunction());
            hostMembers.Add("DateSerial".EmptyFunction());
            hostMembers.Add("DateValue".EmptyFunction());
            hostMembers.Add("Day".EmptyFunction().Renamed("Global.Day"));
            hostMembers.Add("Eval".EmptyFunction("Eval has no equivalent translation."));
            hostMembers.Add("Execute".EmptyFunction("Execute has no equivalent translation."));
            hostMembers.Add("Exp".EmptyFunction());
            hostMembers.Add("Filter".EmptyFunction());
            hostMembers.Add("Fix".EmptyFunction().Renamed("Global.Fix"));
            hostMembers.Add("FormatCurrency".EmptyFunction());
            hostMembers.Add("FormatDateTime".EmptyFunction());
            hostMembers.Add("FormatNumber".EmptyFunction().Renamed("Global.FormatNumber"));
            hostMembers.Add("FormatPercent".EmptyFunction());
            hostMembers.Add("GetLocale".EmptyFunction());
            hostMembers.Add("GetObject".EmptyFunction());
            hostMembers.Add("GetRef".EmptyFunction().Renamed("Global.GetRef"));
            hostMembers.Add("Hex".EmptyFunction().Renamed("Global.Hex"));
            hostMembers.Add("Hour".EmptyFunction().Renamed("Global.Hour"));
            hostMembers.Add("Chr".EmptyFunction().Renamed("Global.Chr"));
            hostMembers.Add("InputBox".EmptyFunction());
            hostMembers.Add("InStr".EmptyFunction().Renamed("Global.InStr"));
            hostMembers.Add("InStrRev".EmptyFunction().Renamed("Global.InStrRev"));
            hostMembers.Add("Int".EmptyFunction().Renamed("Global.Int"));
            hostMembers.Add("IsArray".EmptyFunction().Renamed("Global.IsArray"));
            hostMembers.Add("IsDate".EmptyFunction().Renamed("Global.IsDate"));
            hostMembers.Add("IsEmpty".EmptyFunction().Renamed("Global.IsEmpty"));
            hostMembers.Add("IsNull".EmptyFunction().Renamed("Global.IsNullOrUndefined"));
            hostMembers.Add("IsNumeric".EmptyFunction().Renamed("Global.IsNumeric"));
            hostMembers.Add("IsObject".EmptyFunction().Renamed("Global.IsObject"));
            hostMembers.Add("Join".EmptyFunction().Renamed("Global.Join"));
            hostMembers.Add("LBound".EmptyFunction().Renamed("Global.LBound"));
            hostMembers.Add("LCase".EmptyFunction().Renamed("Global.LCase"));
            hostMembers.Add("Left".EmptyFunction().Renamed("Global.Left"));
            hostMembers.Add("Len".EmptyFunction().Renamed("Global.Len"));
            hostMembers.Add("LoadPicture".EmptyFunction());
            hostMembers.Add("Log".EmptyFunction());
            hostMembers.Add("LTrim".EmptyFunction().Renamed("Global.LTrim"));
            hostMembers.Add("Mid".EmptyFunction().Renamed("Global.Mid"));
            hostMembers.Add("Minute".EmptyFunction().Renamed("Global.Minute"));
            hostMembers.Add("Month".EmptyFunction().Renamed("Global.Month"));
            hostMembers.Add("MonthName".EmptyFunction());
            hostMembers.Add("MsgBox".EmptyFunction().Renamed("Global.external.MsgBox"));
            hostMembers.Add("Now".EmptyFunction().Renamed("Global.Now"));
            hostMembers.Add("Oct".EmptyFunction());
            hostMembers.Add("Replace".EmptyFunction().Renamed("Global.Replace"));
            hostMembers.Add("RGB".EmptyFunction().Renamed("Global.RGB"));
            hostMembers.Add("Right".EmptyFunction().Renamed("Global.Right"));
            hostMembers.Add("Rnd".EmptyFunction());
            hostMembers.Add("Round".EmptyFunction().Renamed("Global.Round"));
            hostMembers.Add("RTrim".EmptyFunction().Renamed("Global.RTrim"));
            hostMembers.Add("ScriptEngine".EmptyFunction());
            hostMembers.Add("ScriptEngineBuildVersion".EmptyFunction());
            hostMembers.Add("ScriptEngineMajorVersion".EmptyFunction());
            hostMembers.Add("ScriptEngineMinorVersion".EmptyFunction());
            hostMembers.Add("Second".EmptyFunction().Renamed("Global.Second"));
            hostMembers.Add("SetLocale".EmptyFunction());
            hostMembers.Add("Sgn".EmptyFunction());
            hostMembers.Add("Sin".EmptyFunction());
            hostMembers.Add("Space".EmptyFunction().Renamed("Global.Space"));
            hostMembers.Add("Split".EmptyFunction().Renamed("Global.Split"));
            hostMembers.Add("Sqr".EmptyFunction());
            hostMembers.Add("StrComp".EmptyFunction().Renamed("Global.StrComp"));
            hostMembers.Add("String".EmptyFunction().Renamed("Global.String"));
            hostMembers.Add("StrReverse".EmptyFunction().Renamed("Global.StrReverse"));
            hostMembers.Add("Tan".EmptyFunction());
            hostMembers.Add("Time".EmptyFunction());
            hostMembers.Add("Timer".EmptyFunction());
            hostMembers.Add("TimeSerial".EmptyFunction());
            hostMembers.Add("TimeValue".EmptyFunction());
            hostMembers.Add("Trim".EmptyFunction().Renamed("Global.Trim"));
            hostMembers.Add("TypeName".EmptyFunction("TypeName has no equivalent translation."));
            hostMembers.Add("UBound".EmptyFunction().Renamed("Global.UBound"));
            hostMembers.Add("UCase".EmptyFunction().Renamed("Global.UCase"));
            hostMembers.Add("VarType".EmptyFunction("VarType has no equivalent translation."));
            hostMembers.Add("Weekday".EmptyFunction());
            hostMembers.Add("WeekdayName".EmptyFunction());
            hostMembers.Add("Year".EmptyFunction().Renamed("Global.Year"));
            hostMembers.Add("Randomize".EmptyFunction());
            hostMembers.Add("external".ToVariable().Renamed("Global.external"));
            hostMembers.Add("vbObjectError".ToConstant().Renamed("Global.vbObjectError"));
            hostMembers.Add("vbTrue".ToConstant().Renamed("Global.vbTrue"));
            hostMembers.Add("vbFalse".ToConstant().Renamed("Global.vbFalse"));

            hostMembers.Add("vbEmpty".ToConstant().Renamed("Global.typeEmpty"));
            hostMembers.Add("vbNull".ToConstant().Renamed("Global.typeNull"));
            hostMembers.Add("vbBoolean".ToConstant().Renamed("Global.typeBoolean"));
            hostMembers.Add("vbByte".ToConstant().Renamed("Global.typeByte"));
            hostMembers.Add("vbInteger".ToConstant().Renamed("Global.typeInteger"));
            hostMembers.Add("vbLong".ToConstant().Renamed("Global.typeLong"));
            hostMembers.Add("vbSingle".ToConstant().Renamed("Global.typeSingle"));
            hostMembers.Add("vbDouble".ToConstant().Renamed("Global.typeDouble"));
            hostMembers.Add("vbDate".ToConstant().Renamed("Global.typeDate"));
            hostMembers.Add("vbString".ToConstant().Renamed("Global.typeString"));
            hostMembers.Add("vbObject".ToConstant().Renamed("Global.typeObject"));
            hostMembers.Add("vbArray".ToConstant().Renamed("Global.typeArray"));

            hostMembers.Add("vbCr".ToConstant().Renamed("Global.Cr"));
            hostMembers.Add("vbCrLf".ToConstant().Renamed("Global.CrLf"));
            hostMembers.Add("vbFormFeed".ToConstant().Renamed("Global.FormFeed"));
            hostMembers.Add("vbLf".ToConstant().Renamed("Global.Lf"));
            hostMembers.Add("vbNewLine".ToConstant().Renamed("Global.NewLine"));
            hostMembers.Add("vbNullChar".ToConstant().Renamed("Global.NullChar"));
            hostMembers.Add("vbNullString".ToConstant().Renamed("Global.NullString"));
            hostMembers.Add("vbTab".ToConstant().Renamed("Global.Tab"));
            hostMembers.Add("vbVerticalTab".ToConstant().Renamed("Global.VerticalTab"));

            hostMembers.Add("vbSunday".ToConstant().Renamed("Global.Sunday"));
            hostMembers.Add("vbMonday".ToConstant().Renamed("Global.Monday"));
            hostMembers.Add("vbTuesday".ToConstant().Renamed("Global.Tuesday"));
            hostMembers.Add("vbWednesday".ToConstant().Renamed("Global.Wednesday"));
            hostMembers.Add("vbThursday".ToConstant().Renamed("Global.Thursday"));
            hostMembers.Add("vbFriday".ToConstant().Renamed("Global.Friday"));
            hostMembers.Add("vbSaturday".ToConstant().Renamed("Global.Saturday"));

            hostMembers.Add("vbUseSystemDayOfWeek".ToConstant().Renamed("Global.UseSystemDayOfWeek"));
            hostMembers.Add("vbLongDate".ToConstant().Renamed("Global.fmtLongDate"));
            hostMembers.Add("vbShortDate".ToConstant().Renamed("Global.fmtShortDate"));
            hostMembers.Add("vbLongTime".ToConstant().Renamed("Global.fmtLongTime"));
            hostMembers.Add("vbShortTime".ToConstant().Renamed("Global.fmtShortTime"));
            hostMembers.Add("vbOKOnly".ToConstant().Renamed("Global.msgOKOnly"));
            hostMembers.Add("vbOKCancel".ToConstant().Renamed("Global.msgOKCancel"));
            hostMembers.Add("vbAbortRetryIgnore".ToConstant().Renamed("Global.msgAbortRetryIgnore"));
            hostMembers.Add("vbYesNoCancel".ToConstant().Renamed("Global.msgYesNoCancel"));
            hostMembers.Add("vbYesNo".ToConstant().Renamed("Global.msgYesNo"));
            hostMembers.Add("vbRetryCancel".ToConstant().Renamed("Global.msgRetryCancel"));
            hostMembers.Add("vbCritical".ToConstant().Renamed("Global.msgCritical"));
            hostMembers.Add("vbQuestion".ToConstant().Renamed("Global.msgQuestion"));
            hostMembers.Add("vbExclamation".ToConstant().Renamed("Global.msgExclamation"));
            hostMembers.Add("vbInformation".ToConstant().Renamed("Global.msgInformation"));
            hostMembers.Add("vbDefaultButton1".ToConstant().Renamed("Global.msgDefaultButton1"));
            hostMembers.Add("vbDefaultButton2".ToConstant().Renamed("Global.msgDefaultButton2"));
            hostMembers.Add("vbDefaultButton3".ToConstant().Renamed("Global.msgDefaultButton3"));
            hostMembers.Add("vbDefaultButton4".ToConstant().Renamed("Global.msgDefaultButton4"));
            hostMembers.Add("vbApplicationModal".ToConstant().Renamed("Global.msgApplicationModal"));
            hostMembers.Add("vbSystemModal".ToConstant().Renamed("Global.msgSystemModal"));
            hostMembers.Add("vbOK".ToConstant().Renamed("Global.msgOK"));
            hostMembers.Add("vbCancel".ToConstant().Renamed("Global.msgCancel"));
            hostMembers.Add("vbAbort".ToConstant().Renamed("Global.msgAbort"));
            hostMembers.Add("vbRetry".ToConstant().Renamed("Global.msgRetry"));
            hostMembers.Add("vbIgnore".ToConstant().Renamed("Global.msgIgnore"));
            hostMembers.Add("vbYes".ToConstant().Renamed("Global.msgYes"));
            hostMembers.Add("vbNo".ToConstant().Renamed("Global.msgNo"));
            hostMembers.Add("vbBinaryCompare".ToConstant().Renamed("Global.BinaryCompare"));
            hostMembers.Add("vbTextCompare".ToConstant().Renamed("Global.TextCompare"));

            return hostMembers;
        }

        public static IReadOnlyList<Symbol> WebApiSymbols()
        {
            var hostMembers = new List<Symbol>();

            var domElemMembers = new List<Symbol>();

            domElemMembers.Add("accessKey".EmptyProperty(false));
            domElemMembers.Add("addEventListener".EmptyFunction());
            domElemMembers.Add("appendChild".EmptyFunction());
            domElemMembers.Add("attributes".EmptyProperty(true));
            domElemMembers.Add("blur".EmptyFunction());
            domElemMembers.Add("childElementCount".EmptyProperty(true));
            domElemMembers.Add("childNodes".EmptyProperty(true));
            domElemMembers.Add("children".EmptyProperty(true));
            domElemMembers.Add("classList".EmptyProperty(true));
            domElemMembers.Add("className".EmptyProperty(false));
            domElemMembers.Add("click".EmptyFunction());
            domElemMembers.Add("clientHeight".EmptyProperty(true));
            domElemMembers.Add("clientLeft".EmptyProperty(true));
            domElemMembers.Add("clientTop".EmptyProperty(true));
            domElemMembers.Add("clientWidth".EmptyProperty(true));
            domElemMembers.Add("cloneNode".EmptyFunction());
            domElemMembers.Add("closest".EmptyFunction());
            domElemMembers.Add("compareDocumentPosition".EmptyFunction());
            domElemMembers.Add("contains".EmptyFunction());
            domElemMembers.Add("contentEditable".EmptyProperty(false));
            domElemMembers.Add("dir".EmptyProperty(false));
            domElemMembers.Add("exitFullscreen".EmptyFunction());
            domElemMembers.Add("firstChild".EmptyProperty(true));
            domElemMembers.Add("firstElementChild".EmptyProperty(true));
            domElemMembers.Add("focus".EmptyFunction());
            domElemMembers.Add("getAttribute".EmptyFunction());
            domElemMembers.Add("getAttributeNode".EmptyFunction());
            domElemMembers.Add("getBoundingClientRect".EmptyFunction());
            domElemMembers.Add("getElementsByClassName".EmptyFunction());
            domElemMembers.Add("getElementsByTagName".EmptyFunction());
            domElemMembers.Add("hasAttribute".EmptyFunction());
            domElemMembers.Add("hasAttributes".EmptyFunction());
            domElemMembers.Add("hasChildNodes".EmptyFunction());
            domElemMembers.Add("id".EmptyProperty(false));
            domElemMembers.Add("innerHTML".EmptyProperty(false));
            domElemMembers.Add("innerText".EmptyProperty(false));
            domElemMembers.Add("insertAdjacentElement".EmptyFunction());
            domElemMembers.Add("insertAdjacentHTML".EmptyFunction());
            domElemMembers.Add("insertAdjacentText".EmptyFunction());
            domElemMembers.Add("insertBefore".EmptyFunction());
            domElemMembers.Add("isContentEditable".EmptyProperty(true));
            domElemMembers.Add("isDefaultNamespace".EmptyFunction());
            domElemMembers.Add("isEqualNode".EmptyFunction());
            domElemMembers.Add("isSameNode".EmptyFunction());
            domElemMembers.Add("isSupported".EmptyFunction());
            domElemMembers.Add("lang".EmptyProperty(false));
            domElemMembers.Add("lastChild".EmptyProperty(true));
            domElemMembers.Add("lastElementChild".EmptyProperty(true));
            domElemMembers.Add("matches".EmptyFunction());
            domElemMembers.Add("namespaceURI".EmptyProperty(true));
            domElemMembers.Add("nextSibling".EmptyProperty(true));
            domElemMembers.Add("nextElementSibling".EmptyProperty(true));
            domElemMembers.Add("nodeName".EmptyProperty(true));
            domElemMembers.Add("nodeType".EmptyProperty(true));
            domElemMembers.Add("nodeValue".EmptyProperty(false));
            domElemMembers.Add("normalize".EmptyFunction());
            domElemMembers.Add("offsetHeight".EmptyProperty(true));
            domElemMembers.Add("offsetWidth".EmptyProperty(true));
            domElemMembers.Add("offsetLeft".EmptyProperty(true));
            domElemMembers.Add("offsetParent".EmptyProperty(true));
            domElemMembers.Add("offsetTop".EmptyProperty(true));
            domElemMembers.Add("outerHTML".EmptyProperty(false));
            domElemMembers.Add("outerText".EmptyProperty(false));
            domElemMembers.Add("ownerDocument".EmptyProperty(true));
            domElemMembers.Add("parentNode".EmptyProperty(true));
            domElemMembers.Add("parentElement".EmptyProperty(true));
            domElemMembers.Add("previousSibling".EmptyProperty(true));
            domElemMembers.Add("previousElementSibling".EmptyProperty(true));
            domElemMembers.Add("querySelector".EmptyFunction());
            domElemMembers.Add("querySelectorAll".EmptyFunction());
            domElemMembers.Add("remove".EmptyFunction());
            domElemMembers.Add("removeAttribute".EmptyFunction());
            domElemMembers.Add("removeAttributeNode".EmptyFunction());
            domElemMembers.Add("removeChild".EmptyFunction());
            domElemMembers.Add("removeEventListener".EmptyFunction());
            domElemMembers.Add("replaceChild".EmptyFunction());
            domElemMembers.Add("requestFullscreen".EmptyFunction());
            domElemMembers.Add("scrollHeight".EmptyProperty(true));
            domElemMembers.Add("scrollIntoView".EmptyFunction());
            domElemMembers.Add("scrollLeft".EmptyProperty(false));
            domElemMembers.Add("scrollTop".EmptyProperty(false));
            domElemMembers.Add("scrollWidth".EmptyProperty(true));
            domElemMembers.Add("setAttribute".EmptyFunction());
            domElemMembers.Add("setAttributeNode".EmptyFunction());
            domElemMembers.Add("style".EmptyProperty(false));
            domElemMembers.Add("runtimestyle".EmptyProperty(false, true, "style"));
            domElemMembers.Add("tabIndex".EmptyProperty(false));
            domElemMembers.Add("tagName".EmptyProperty(true));
            domElemMembers.Add("textContent".EmptyProperty(false));
            domElemMembers.Add("title".EmptyProperty(false));
            domElemMembers.Add("toString".EmptyFunction());

            hostMembers.Add(new ClassSymbol("_Element", domElemMembers));

            var namedNodeMapMembers = new List<Symbol>();

            namedNodeMapMembers.Add("length".EmptyProperty(true));
            namedNodeMapMembers.Add("getNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("getNamedItemNS".EmptyFunction());
            namedNodeMapMembers.Add("item".EmptyFunction());
            namedNodeMapMembers.Add("removeNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("removeNamedItemNS".EmptyFunction());
            namedNodeMapMembers.Add("setNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("setNamedItemNS".EmptyFunction());

            hostMembers.Add(new ClassSymbol("_NamedNodeMap", namedNodeMapMembers));

            var attributeMembers = new List<Symbol>();

            attributeMembers.Add("name".EmptyProperty(true));
            attributeMembers.Add("value".EmptyProperty(false));
            attributeMembers.Add("specified".EmptyProperty(true));

            hostMembers.Add(new ClassSymbol("_Attr", attributeMembers));

            var documentMembers = new List<Symbol>();

            documentMembers.Add("activeElement".EmptyProperty(true));  //Returns the currently focused element in the document
            documentMembers.Add("addEventListener".EmptyFunction());  //Attaches an event handler to the document
            documentMembers.Add("adoptNode".EmptyFunction());  //Adopts a node from another document
            documentMembers.Add("anchors".EmptyProperty(true));  //Deprecated
            documentMembers.Add("applets".EmptyProperty(true));  //Deprecated
            documentMembers.Add("baseURI".EmptyProperty(true));  //Returns the absolute base URI of a document
            documentMembers.Add("body".EmptyProperty(false));  //Sets or returns the document's body (the <body> element)
            documentMembers.Add("charset".EmptyProperty(true));  //Deprecated
            documentMembers.Add("characterSet".EmptyProperty(true));  //Returns the character encoding for the document
            documentMembers.Add("close".EmptyFunction());  //Closes the output stream previously opened with document.open()
            documentMembers.Add("cookie".EmptyProperty(true));  //Returns all name/value pairs of cookies in the document
            documentMembers.Add("createAttribute".EmptyFunction());  //Creates an attribute node
            documentMembers.Add("createComment".EmptyFunction());  //Creates a Comment node with the specified text
            documentMembers.Add("createDocumentFragment".EmptyFunction());  //Creates an empty DocumentFragment node
            documentMembers.Add("createElement".EmptyFunction());  //Creates an Element node
            documentMembers.Add("createEvent".EmptyFunction());  //Creates a new event
            documentMembers.Add("createTextNode".EmptyFunction());  //Creates a Text node
            documentMembers.Add("defaultView".EmptyProperty(true));  //Returns the window object associated with a document, or null if none is available.
            documentMembers.Add("designMode".EmptyProperty(false));  //Controls whether the entire document should be editable or not.
            documentMembers.Add("doctype".EmptyProperty(true));  //Returns the Document Type Declaration associated with the document
            documentMembers.Add("documentElement".EmptyProperty(true));  //Returns the Document Element of the document (the <html> element)
            documentMembers.Add("documentMode".EmptyProperty(true));  //Deprecated
            documentMembers.Add("documentURI".EmptyProperty(false));  //Sets or returns the location of the document
            documentMembers.Add("domain".EmptyProperty(true));  //Returns the domain name of the server that loaded the document
            documentMembers.Add("domConfig".EmptyProperty(true));  //Deprecated
            documentMembers.Add("embeds".EmptyProperty(true));  //Returns a collection of all <embed> elements the document
            documentMembers.Add("execCommand".EmptyFunction());  //Deprecated
            documentMembers.Add("forms".EmptyProperty(true));  //Returns a collection of all <form> elements in the document
            documentMembers.Add("getElementById".EmptyFunction());  //Returns the element that has the ID attribute with the specified value
            documentMembers.Add("getElementsByClassName".EmptyFunction());  //Returns a HTMLCollection containing all elements with the specified class name
            documentMembers.Add("getElementsByName".EmptyFunction());  //Deprecated
            documentMembers.Add("getElementsByTagName".EmptyFunction());  //Returns a HTMLCollection containing all elements with the specified tag name
            documentMembers.Add("hasFocus".EmptyFunction());  //Returns a Boolean value indicating whether the document has focus
            documentMembers.Add("head".EmptyProperty(true));  //Returns the <head> element of the document
            documentMembers.Add("images".EmptyProperty(true));  //Returns a collection of all <img> elements in the document
            documentMembers.Add("implementation".EmptyProperty(true));  //Returns the DOMImplementation object that handles this document
            documentMembers.Add("importNode".EmptyFunction());  //Imports a node from another document
            documentMembers.Add("inputEncoding".EmptyProperty(true));  //Deprecated
            documentMembers.Add("lastModified".EmptyProperty(true));  //Returns the date and time the document was last modified
            documentMembers.Add("links".EmptyProperty(true));  //Returns a collection of all <a> and <area> elements in the document that have a href attribute
            documentMembers.Add("normalize".EmptyFunction());  //Removes empty Text nodes, and joins adjacent nodes
            documentMembers.Add("normalizeDocument".EmptyFunction());  //Deprecated
            documentMembers.Add("open".EmptyFunction());  //Opens an HTML output stream to collect output from document.write()
            documentMembers.Add("querySelector".EmptyFunction());  //Returns the first element that matches a specified CSS selector(s) in the document
            documentMembers.Add("querySelectorAll".EmptyFunction());  //Returns a static NodeList containing all elements that matches a specified CSS selector(s) in the document
            documentMembers.Add("readyState".EmptyProperty(true));  //Returns the (loading) status of the document
            documentMembers.Add("referrer".EmptyProperty(true));  //Returns the URL of the document that loaded the current document
            documentMembers.Add("removeEventListener".EmptyFunction());  //Removes an event handler from the document (that has been attached with the addEventListener() method)
            documentMembers.Add("renameNode".EmptyFunction());  //Deprecated
            documentMembers.Add("scripts".EmptyProperty(true));  //Returns a collection of <script> elements in the document
            documentMembers.Add("strictErrorChecking".EmptyProperty(true));  //Deprecated
            documentMembers.Add("title".EmptyProperty(false));  //Sets or returns the title of the document
            documentMembers.Add("URL".EmptyProperty(true));  //Returns the full URL of the HTML document
            documentMembers.Add("write".EmptyFunction());  //Writes HTML expressions or JavaScript code to a document
            documentMembers.Add("writeln".EmptyFunction());  //Same as write(), but adds a newline character after each statement

            var docClass = new ClassSymbol("_Document", documentMembers);

            hostMembers.Add(docClass);

            hostMembers.Add(new VariableSymbol("document", Option.None, Option.None, true, docClass.Name.Some()));

            var tableMembers = new List<Symbol>();

            tableMembers.Add("rows".EmptyProperty(true, true));
            tableMembers.Add("tBodies".EmptyProperty(true, true));
            tableMembers.Add("caption".EmptyProperty(true, true));
            tableMembers.Add("tFoot".EmptyProperty(true, true));
            tableMembers.Add("tHead".EmptyProperty(true, true));
            tableMembers.Add("createCaption".EmptyFunction());
            tableMembers.Add("createTFoot".EmptyFunction());
            tableMembers.Add("createTHead".EmptyFunction());
            tableMembers.Add("deleteCaption".EmptyFunction());
            tableMembers.Add("deleteRow".EmptyFunction());
            tableMembers.Add("deleteTFoot".EmptyFunction());
            tableMembers.Add("deleteTHead".EmptyFunction());
            tableMembers.Add("insertRow".EmptyFunction());

            hostMembers.Add(new ClassSymbol("_HTMLTableElement", tableMembers));

            var styleMembers = new List<Symbol>();

            styleMembers.Add("alignContent".EmptyProperty(false));
            styleMembers.Add("alignItems".EmptyProperty(false));
            styleMembers.Add("alignSelf".EmptyProperty(false));
            styleMembers.Add("animation".EmptyProperty(false));
            styleMembers.Add("animationDelay".EmptyProperty(false));
            styleMembers.Add("animationDirection".EmptyProperty(false));
            styleMembers.Add("animationDuration".EmptyProperty(false));
            styleMembers.Add("animationFillMode".EmptyProperty(false));
            styleMembers.Add("animationIterationCount".EmptyProperty(false));
            styleMembers.Add("animationName".EmptyProperty(false));
            styleMembers.Add("animationTimingFunction".EmptyProperty(false));
            styleMembers.Add("animationPlayState".EmptyProperty(false));
            styleMembers.Add("background".EmptyProperty(false));
            styleMembers.Add("backgroundAttachment".EmptyProperty(false));
            styleMembers.Add("backgroundColor".EmptyProperty(false));
            styleMembers.Add("backgroundImage".EmptyProperty(false));
            styleMembers.Add("backgroundPosition".EmptyProperty(false));
            styleMembers.Add("backgroundRepeat".EmptyProperty(false));
            styleMembers.Add("backgroundClip".EmptyProperty(false));
            styleMembers.Add("backgroundOrigin".EmptyProperty(false));
            styleMembers.Add("backgroundSize".EmptyProperty(false));
            styleMembers.Add("backfaceVisibility".EmptyProperty(false));
            styleMembers.Add("border".EmptyProperty(false));
            styleMembers.Add("borderBottom".EmptyProperty(false));
            styleMembers.Add("borderBottomColor".EmptyProperty(false));
            styleMembers.Add("borderBottomLeftRadius".EmptyProperty(false));
            styleMembers.Add("borderBottomRightRadius".EmptyProperty(false));
            styleMembers.Add("borderBottomStyle".EmptyProperty(false));
            styleMembers.Add("borderBottomWidth".EmptyProperty(false));
            styleMembers.Add("borderCollapse".EmptyProperty(false));
            styleMembers.Add("borderColor".EmptyProperty(false));
            styleMembers.Add("borderImage".EmptyProperty(false));
            styleMembers.Add("borderImageOutset".EmptyProperty(false));
            styleMembers.Add("borderImageRepeat".EmptyProperty(false));
            styleMembers.Add("borderImageSlice".EmptyProperty(false));
            styleMembers.Add("borderImageSource".EmptyProperty(false));
            styleMembers.Add("borderImageWidth".EmptyProperty(false));
            styleMembers.Add("borderLeft".EmptyProperty(false));
            styleMembers.Add("borderLeftColor".EmptyProperty(false));
            styleMembers.Add("borderLeftStyle".EmptyProperty(false));
            styleMembers.Add("borderLeftWidth".EmptyProperty(false));
            styleMembers.Add("borderRadius".EmptyProperty(false));
            styleMembers.Add("borderRight".EmptyProperty(false));
            styleMembers.Add("borderRightColor".EmptyProperty(false));
            styleMembers.Add("borderRightStyle".EmptyProperty(false));
            styleMembers.Add("borderRightWidth".EmptyProperty(false));
            styleMembers.Add("borderSpacing".EmptyProperty(false));
            styleMembers.Add("borderStyle".EmptyProperty(false));
            styleMembers.Add("borderTop".EmptyProperty(false));
            styleMembers.Add("borderTopColor".EmptyProperty(false));
            styleMembers.Add("borderTopLeftRadius".EmptyProperty(false));
            styleMembers.Add("borderTopRightRadius".EmptyProperty(false));
            styleMembers.Add("borderTopStyle".EmptyProperty(false));
            styleMembers.Add("borderTopWidth".EmptyProperty(false));
            styleMembers.Add("borderWidth".EmptyProperty(false));
            styleMembers.Add("bottom".EmptyProperty(false));
            styleMembers.Add("boxDecorationBreak".EmptyProperty(false));
            styleMembers.Add("boxShadow".EmptyProperty(false));
            styleMembers.Add("boxSizing".EmptyProperty(false));
            styleMembers.Add("captionSide".EmptyProperty(false));
            styleMembers.Add("caretColor".EmptyProperty(false));
            styleMembers.Add("clear".EmptyProperty(false));
            styleMembers.Add("clip".EmptyProperty(false));
            styleMembers.Add("color".EmptyProperty(false));
            styleMembers.Add("columnCount".EmptyProperty(false));
            styleMembers.Add("columnFill".EmptyProperty(false));
            styleMembers.Add("columnGap".EmptyProperty(false));
            styleMembers.Add("columnRule".EmptyProperty(false));
            styleMembers.Add("columnRuleColor".EmptyProperty(false));
            styleMembers.Add("columnRuleStyle".EmptyProperty(false));
            styleMembers.Add("columnRuleWidth".EmptyProperty(false));
            styleMembers.Add("columns".EmptyProperty(false));
            styleMembers.Add("columnSpan".EmptyProperty(false));
            styleMembers.Add("columnWidth".EmptyProperty(false));
            styleMembers.Add("content".EmptyProperty(false));
            styleMembers.Add("counterIncrement".EmptyProperty(false));
            styleMembers.Add("counterReset".EmptyProperty(false));
            styleMembers.Add("cursor".EmptyProperty(false));
            styleMembers.Add("direction".EmptyProperty(false));
            styleMembers.Add("display".EmptyProperty(false));
            styleMembers.Add("emptyCells".EmptyProperty(false));
            styleMembers.Add("filter".EmptyProperty(false));
            styleMembers.Add("flex".EmptyProperty(false));
            styleMembers.Add("flexBasis".EmptyProperty(false));
            styleMembers.Add("flexDirection".EmptyProperty(false));
            styleMembers.Add("flexFlow".EmptyProperty(false));
            styleMembers.Add("flexGrow".EmptyProperty(false));
            styleMembers.Add("flexShrink".EmptyProperty(false));
            styleMembers.Add("flexWrap".EmptyProperty(false));
            styleMembers.Add("cssFloat".EmptyProperty(false));
            styleMembers.Add("font".EmptyProperty(false));
            styleMembers.Add("fontFamily".EmptyProperty(false));
            styleMembers.Add("fontSize".EmptyProperty(false));
            styleMembers.Add("fontStyle".EmptyProperty(false));
            styleMembers.Add("fontVariant".EmptyProperty(false));
            styleMembers.Add("fontWeight".EmptyProperty(false));
            styleMembers.Add("fontSizeAdjust".EmptyProperty(false));
            styleMembers.Add("fontStretch".EmptyProperty(false));
            styleMembers.Add("hangingPunctuation".EmptyProperty(false));
            styleMembers.Add("height".EmptyProperty(false));
            styleMembers.Add("hyphens".EmptyProperty(false));
            styleMembers.Add("icon".EmptyProperty(false));
            styleMembers.Add("imageOrientation".EmptyProperty(false));
            styleMembers.Add("isolation".EmptyProperty(false));
            styleMembers.Add("justifyContent".EmptyProperty(false));
            styleMembers.Add("left".EmptyProperty(false));
            styleMembers.Add("letterSpacing".EmptyProperty(false));
            styleMembers.Add("lineHeight".EmptyProperty(false));
            styleMembers.Add("listStyle".EmptyProperty(false));
            styleMembers.Add("listStyleImage".EmptyProperty(false));
            styleMembers.Add("listStylePosition".EmptyProperty(false));
            styleMembers.Add("listStyleType".EmptyProperty(false));
            styleMembers.Add("margin".EmptyProperty(false));
            styleMembers.Add("marginBottom".EmptyProperty(false));
            styleMembers.Add("marginLeft".EmptyProperty(false));
            styleMembers.Add("marginRight".EmptyProperty(false));
            styleMembers.Add("marginTop".EmptyProperty(false));
            styleMembers.Add("maxHeight".EmptyProperty(false));
            styleMembers.Add("maxWidth".EmptyProperty(false));
            styleMembers.Add("minHeight".EmptyProperty(false));
            styleMembers.Add("minWidth".EmptyProperty(false));
            styleMembers.Add("navDown".EmptyProperty(false));
            styleMembers.Add("navIndex".EmptyProperty(false));
            styleMembers.Add("navLeft".EmptyProperty(false));
            styleMembers.Add("navRight".EmptyProperty(false));
            styleMembers.Add("navUp".EmptyProperty(false));
            styleMembers.Add("objectFit".EmptyProperty(false));
            styleMembers.Add("objectPosition".EmptyProperty(false));
            styleMembers.Add("opacity".EmptyProperty(false));
            styleMembers.Add("order".EmptyProperty(false));
            styleMembers.Add("orphans".EmptyProperty(false));
            styleMembers.Add("outline".EmptyProperty(false));
            styleMembers.Add("outlineColor".EmptyProperty(false));
            styleMembers.Add("outlineOffset".EmptyProperty(false));
            styleMembers.Add("outlineStyle".EmptyProperty(false));
            styleMembers.Add("outlineWidth".EmptyProperty(false));
            styleMembers.Add("overflow".EmptyProperty(false));
            styleMembers.Add("overflowX".EmptyProperty(false));
            styleMembers.Add("overflowY".EmptyProperty(false));
            styleMembers.Add("padding".EmptyProperty(false));
            styleMembers.Add("paddingBottom".EmptyProperty(false));
            styleMembers.Add("paddingLeft".EmptyProperty(false));
            styleMembers.Add("paddingRight".EmptyProperty(false));
            styleMembers.Add("paddingTop".EmptyProperty(false));
            styleMembers.Add("pageBreakAfter".EmptyProperty(false));
            styleMembers.Add("pageBreakBefore".EmptyProperty(false));
            styleMembers.Add("pageBreakInside".EmptyProperty(false));
            styleMembers.Add("perspective".EmptyProperty(false));
            styleMembers.Add("perspectiveOrigin".EmptyProperty(false));
            styleMembers.Add("position".EmptyProperty(false));
            styleMembers.Add("quotes".EmptyProperty(false));
            styleMembers.Add("resize".EmptyProperty(false));
            styleMembers.Add("right".EmptyProperty(false));
            styleMembers.Add("scrollBehavior".EmptyProperty(false));
            styleMembers.Add("tableLayout".EmptyProperty(false));
            styleMembers.Add("tabSize".EmptyProperty(false));
            styleMembers.Add("textAlign".EmptyProperty(false));
            styleMembers.Add("textAlignLast".EmptyProperty(false));
            styleMembers.Add("textDecoration".EmptyProperty(false));
            styleMembers.Add("textDecorationColor".EmptyProperty(false));
            styleMembers.Add("textDecorationLine".EmptyProperty(false));
            styleMembers.Add("textDecorationStyle".EmptyProperty(false));
            styleMembers.Add("textIndent".EmptyProperty(false));
            styleMembers.Add("textJustify".EmptyProperty(false));
            styleMembers.Add("textOverflow".EmptyProperty(false));
            styleMembers.Add("textShadow".EmptyProperty(false));
            styleMembers.Add("textTransform".EmptyProperty(false));
            styleMembers.Add("top".EmptyProperty(false));
            styleMembers.Add("transform".EmptyProperty(false));
            styleMembers.Add("transformOrigin".EmptyProperty(false));
            styleMembers.Add("transformStyle".EmptyProperty(false));
            styleMembers.Add("transition".EmptyProperty(false));
            styleMembers.Add("transitionProperty".EmptyProperty(false));
            styleMembers.Add("transitionDuration".EmptyProperty(false));
            styleMembers.Add("transitionTimingFunction".EmptyProperty(false));
            styleMembers.Add("transitionDelay".EmptyProperty(false));
            styleMembers.Add("unicodeBidi".EmptyProperty(false));
            styleMembers.Add("userSelect".EmptyProperty(false));
            styleMembers.Add("verticalAlign".EmptyProperty(false));
            styleMembers.Add("visibility".EmptyProperty(false));
            styleMembers.Add("whiteSpace".EmptyProperty(false));
            styleMembers.Add("width".EmptyProperty(false));
            styleMembers.Add("wordBreak".EmptyProperty(false));
            styleMembers.Add("wordSpacing".EmptyProperty(false));
            styleMembers.Add("wordWrap".EmptyProperty(false));
            styleMembers.Add("widows".EmptyProperty(false));
            styleMembers.Add("zIndex".EmptyProperty(false));

            // obsolete members
            styleMembers.Add("pixelheight".EmptyProperty(false, true, "height"));
            styleMembers.Add("pixelwidth".EmptyProperty(false, true, "width"));

            hostMembers.Add(new ClassSymbol("_HTMLStyleElement", Option.None, Option.None, styleMembers, Option.None));


            var imageMembers = new List<Symbol>();

            imageMembers.Add("align".EmptyProperty(false));
            imageMembers.Add("alt".EmptyProperty(false));
            imageMembers.Add("border".EmptyProperty(false));

            imageMembers.Add("complete".EmptyFunction());

            imageMembers.Add("crossOrigin".EmptyProperty(false));
            imageMembers.Add("height".EmptyProperty(false));
            imageMembers.Add("hspace".EmptyProperty(false));
            imageMembers.Add("isMap".EmptyProperty(false));
            imageMembers.Add("longDesc".EmptyProperty(false));
            imageMembers.Add("lowsrc".EmptyProperty(false));
            imageMembers.Add("name".EmptyProperty(false));
            imageMembers.Add("naturalHeight".EmptyProperty(true));
            imageMembers.Add("naturalWidth".EmptyProperty(true));
            imageMembers.Add("src".EmptyProperty(false));
            imageMembers.Add("useMap".EmptyProperty(false));
            imageMembers.Add("vspace".EmptyProperty(false));
            imageMembers.Add("width".EmptyProperty(false));

            hostMembers.Add(new ClassSymbol("_HTMLImageElement", Option.None, Option.None, imageMembers, Option.None));

            return hostMembers;
        }

        private static Symbol[] _NoMembers = new Symbol[] { };

        public static ConstantSymbol ToConstant(this String @this, Boolean isPublic = true)
            => new ConstantSymbol(@this, Option.None, Option.None, isPublic, Option.None);
        public static PropertyGetSymbol EmptyPropertyGet(this String @this, Boolean isPublic = true)
            => new PropertyGetSymbol(@this, isPublic, Option.None, _NoMembers);
        public static PropertySetSymbol EmptyPropertySet(this String @this, Boolean isPublic = true)
            => new PropertySetSymbol(@this, isPublic, Option.None, false, _NoMembers);
        public static FunctionSymbol EmptyFunction(this String @this, String? errMessage = null, Boolean isPublic = true)
            => new FunctionSymbol(@this, Option.None, errMessage.AsOption(), isPublic, Option.None, _NoMembers);
        public static PropertySymbol EmptyProperty(this String @this, Boolean readOnly, Boolean isPublic = true, String? newName = null)
            => new PropertySymbol(@this,
                                  newName == null ? @this.EmptyPropertyGet(isPublic).Some<FunctionSymbol>()
                                                  : newName.EmptyPropertyGet(isPublic).Some<FunctionSymbol>(),
                                  readOnly
                                  ? Option.None
                                  : newName == null ? @this.EmptyPropertySet(isPublic).Some<FunctionSymbol>()
                                                    : newName.EmptyPropertySet(isPublic).Some<FunctionSymbol>());
        public static VariableSymbol ToVariable(this String @this, Boolean isPublic = true, ClassSymbol? @class = null, String? errMessage = null)
            => new VariableSymbol(@this, Option.None, errMessage.AsOption(), isPublic, @class == null ? Option.None : @class.Name.Some());

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

