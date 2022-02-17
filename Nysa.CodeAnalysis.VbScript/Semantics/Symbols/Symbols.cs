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
            hostMembers.Add(new VariableSymbol("Err", true, vbsErrClass));
            hostMembers.Add(new VariableSymbol("me", "this".Some(), true, Option<String>.None));

            hostMembers.Add("Abs".EmptyFunction());
            hostMembers.Add("Array".EmptyFunction());
            hostMembers.Add("Asc".EmptyFunction());
            hostMembers.Add("Atn".EmptyFunction());
            hostMembers.Add("CBool".EmptyFunction());
            hostMembers.Add("CByte".EmptyFunction());
            hostMembers.Add("CCur".EmptyFunction());
            hostMembers.Add("CDate".EmptyFunction());
            hostMembers.Add("CDbl".EmptyFunction());
            hostMembers.Add("CInt".EmptyFunction());
            hostMembers.Add("CLng".EmptyFunction());
            hostMembers.Add("Cos".EmptyFunction());
            hostMembers.Add("CreateObject".EmptyFunction());
            hostMembers.Add("CSng".EmptyFunction());
            hostMembers.Add("CStr".EmptyFunction());
            hostMembers.Add("Date".EmptyFunction());
            hostMembers.Add("DateAdd".EmptyFunction());
            hostMembers.Add("DateDiff".EmptyFunction());
            hostMembers.Add("DatePart".EmptyFunction());
            hostMembers.Add("DateSerial".EmptyFunction());
            hostMembers.Add("DateValue".EmptyFunction());
            hostMembers.Add("Day".EmptyFunction());
            hostMembers.Add("Eval".EmptyFunction());
            hostMembers.Add("Exp".EmptyFunction());
            hostMembers.Add("Filter".EmptyFunction());
            hostMembers.Add("Fix".EmptyFunction());
            hostMembers.Add("FormatCurrency".EmptyFunction());
            hostMembers.Add("FormatDateTime".EmptyFunction());
            hostMembers.Add("FormatNumber".EmptyFunction());
            hostMembers.Add("FormatPercent".EmptyFunction());
            hostMembers.Add("GetLocale".EmptyFunction());
            hostMembers.Add("GetObject".EmptyFunction());
            hostMembers.Add("GetRef".EmptyFunction());
            hostMembers.Add("Hex".EmptyFunction());
            hostMembers.Add("Hour".EmptyFunction());
            hostMembers.Add("Chr".EmptyFunction());
            hostMembers.Add("InputBox".EmptyFunction());
            hostMembers.Add("InStr".EmptyFunction());
            hostMembers.Add("InStrRev".EmptyFunction());
            hostMembers.Add("Int".EmptyFunction());
            hostMembers.Add("IsArray".EmptyFunction());
            hostMembers.Add("IsDate".EmptyFunction());
            hostMembers.Add("IsEmpty".EmptyFunction());
            hostMembers.Add("IsNull".EmptyFunction());
            hostMembers.Add("IsNumeric".EmptyFunction());
            hostMembers.Add("IsObject".EmptyFunction());
            hostMembers.Add("Join".EmptyFunction());
            hostMembers.Add("LBound".EmptyFunction());
            hostMembers.Add("LCase".EmptyFunction());
            hostMembers.Add("Left".EmptyFunction());
            hostMembers.Add("Len".EmptyFunction());
            hostMembers.Add("LoadPicture".EmptyFunction());
            hostMembers.Add("Log".EmptyFunction());
            hostMembers.Add("LTrim".EmptyFunction());
            hostMembers.Add("Mid".EmptyFunction());
            hostMembers.Add("Minute".EmptyFunction());
            hostMembers.Add("Month".EmptyFunction());
            hostMembers.Add("MonthName".EmptyFunction());
            hostMembers.Add("MsgBox".EmptyFunction());
            hostMembers.Add("Now".EmptyFunction());
            hostMembers.Add("Oct".EmptyFunction());
            hostMembers.Add("Replace".EmptyFunction());
            hostMembers.Add("RGB".EmptyFunction());
            hostMembers.Add("Right".EmptyFunction());
            hostMembers.Add("Rnd".EmptyFunction());
            hostMembers.Add("Round".EmptyFunction());
            hostMembers.Add("RTrim".EmptyFunction());
            hostMembers.Add("ScriptEngine".EmptyFunction());
            hostMembers.Add("ScriptEngineBuildVersion".EmptyFunction());
            hostMembers.Add("ScriptEngineMajorVersion".EmptyFunction());
            hostMembers.Add("ScriptEngineMinorVersion".EmptyFunction());
            hostMembers.Add("Second".EmptyFunction());
            hostMembers.Add("SetLocale".EmptyFunction());
            hostMembers.Add("Sgn".EmptyFunction());
            hostMembers.Add("Sin".EmptyFunction());
            hostMembers.Add("Space".EmptyFunction());
            hostMembers.Add("Split".EmptyFunction());
            hostMembers.Add("Sqr".EmptyFunction());
            hostMembers.Add("StrComp".EmptyFunction());
            hostMembers.Add("String".EmptyFunction());
            hostMembers.Add("StrReverse".EmptyFunction());
            hostMembers.Add("Tan".EmptyFunction());
            hostMembers.Add("Time".EmptyFunction());
            hostMembers.Add("Timer".EmptyFunction());
            hostMembers.Add("TimeSerial".EmptyFunction());
            hostMembers.Add("TimeValue".EmptyFunction());
            hostMembers.Add("Trim".EmptyFunction());
            hostMembers.Add("TypeName".EmptyFunction());
            hostMembers.Add("UBound".EmptyFunction());
            hostMembers.Add("UCase".EmptyFunction());
            hostMembers.Add("VarType".EmptyFunction());
            hostMembers.Add("Weekday".EmptyFunction());
            hostMembers.Add("WeekdayName".EmptyFunction());
            hostMembers.Add("Year".EmptyFunction());
            hostMembers.Add("Randomize".EmptyFunction());

            hostMembers.Add("vbEmpty".ToConstant());
            hostMembers.Add("vbNull".ToConstant());
            hostMembers.Add("vbBoolean".ToConstant());
            hostMembers.Add("vbByte".ToConstant());
            hostMembers.Add("vbInteger".ToConstant());
            hostMembers.Add("vbLong".ToConstant());
            hostMembers.Add("vbSingle".ToConstant());
            hostMembers.Add("vbDouble".ToConstant());
            hostMembers.Add("vbDate".ToConstant());
            hostMembers.Add("vbString".ToConstant());
            hostMembers.Add("vbObject".ToConstant());
            hostMembers.Add("vbArray".ToConstant());

            hostMembers.Add("vbCr".ToConstant());
            hostMembers.Add("vbCrLf".ToConstant());
            hostMembers.Add("vbLf".ToConstant());
            hostMembers.Add("vbNullChar".ToConstant());
            hostMembers.Add("vbNullString".ToConstant());
            hostMembers.Add("vbTab".ToConstant());

            hostMembers.Add("vbSunday".ToConstant());
            hostMembers.Add("vbMonday".ToConstant());
            hostMembers.Add("vbTuesday".ToConstant());
            hostMembers.Add("vbWednesday".ToConstant());
            hostMembers.Add("vbThursday".ToConstant());
            hostMembers.Add("vbFriday".ToConstant());
            hostMembers.Add("vbSaturday".ToConstant());

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
            domElemMembers.Add("tabIndex".EmptyProperty(false));
            domElemMembers.Add("tagName".EmptyProperty(true));
            domElemMembers.Add("textContent".EmptyProperty(false));
            domElemMembers.Add("title".EmptyProperty(false));
            domElemMembers.Add("toString".EmptyFunction());

            hostMembers.Add(new ClassSymbol("_Html_Element", domElemMembers));

            var namedNodeMapMembers = new List<Symbol>();

            namedNodeMapMembers.Add("length".EmptyProperty(true));
            namedNodeMapMembers.Add("getNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("getNamedItemNS".EmptyFunction());
            namedNodeMapMembers.Add("item".EmptyFunction());
            namedNodeMapMembers.Add("removeNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("removeNamedItemNS".EmptyFunction());
            namedNodeMapMembers.Add("setNamedItem".EmptyFunction());
            namedNodeMapMembers.Add("setNamedItemNS".EmptyFunction());

            hostMembers.Add(new ClassSymbol("_Html_NamedNodeMap", namedNodeMapMembers));

            var attributeMembers = new List<Symbol>();

            attributeMembers.Add("name".EmptyProperty(true));
            attributeMembers.Add("value".EmptyProperty(false));
            attributeMembers.Add("specified".EmptyProperty(true));

            hostMembers.Add(new ClassSymbol("_Html_Attribute", attributeMembers));

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

            hostMembers.Add(new ClassSymbol("_Html_Document", documentMembers));

            return hostMembers;
        }

        private static Symbol[] _NoMembers = new Symbol[] { };

        public static ConstantSymbol ToConstant(this String @this, Boolean isPublic = true)
            => new ConstantSymbol(@this, isPublic);
        public static PropertyGetSymbol EmptyPropertyGet(this String @this, Boolean isPublic = true)
            => new PropertyGetSymbol(@this, isPublic, _NoMembers);
        public static PropertySetSymbol EmptyPropertySet(this String @this, Boolean isPublic = true)
            => new PropertySetSymbol(@this, isPublic, false, _NoMembers);
        public static FunctionSymbol EmptyFunction(this String @this, Boolean isPublic = true)
            => new FunctionSymbol(@this, isPublic, _NoMembers);
        public static PropertySymbol EmptyProperty(this String @this, Boolean readOnly, Boolean isPublic = true)
            => new PropertySymbol(@this,
                                  @this.EmptyPropertyGet(true).Some<FunctionSymbol>(),
                                  readOnly ? Option.None : @this.EmptyPropertySet(true).Some<FunctionSymbol>());

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
    }

}

