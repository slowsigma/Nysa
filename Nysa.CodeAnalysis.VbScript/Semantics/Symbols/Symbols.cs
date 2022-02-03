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
            var vbsErrClass = new ClassSymbol("_VbScriptEngineErrClass", Symbols.Members("Description".BlindProperty(true),
                                                                                         "HelpContext".BlindProperty(true),
                                                                                         "HelpFile".BlindProperty(true),
                                                                                         "Number".BlindProperty(true, true),
                                                                                         "Source".BlindProperty(true),
                                                                                         "Clear".EmptyFunction(),
                                                                                         "Raise".EmptyFunction()));
            
            var hostMembers = new List<Symbol>();

            hostMembers.Add(vbsErrClass);
            hostMembers.Add(new VariableSymbol("Err", true, vbsErrClass));

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

            return hostMembers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static String LookupKey(this Symbol @this)
            => @this switch
            {
                HardSymbol hard => hard.NewName is Some<String> someNew ? someNew.Value : hard.Name,
                PropertySymbol prop => prop.Name,
                _ => throw new Exception("Unexpected type.")
            };

        private static Symbol[] _NoMembers = new Symbol[] { };

        public static PropertyGetSymbol EmptyPropertyGet(this String @this, Boolean isPublic = true)
            => new PropertyGetSymbol(@this, isPublic, _NoMembers);
        public static PropertySetSymbol EmptyPropertySet(this String @this, Boolean isPublic = true)
            => new PropertySetSymbol(@this, isPublic, _NoMembers);
        public static FunctionSymbol EmptyFunction(this String @this, Boolean isPublic = true)
            => new FunctionSymbol(@this, isPublic, _NoMembers);
        public static PropertySymbol BlindProperty(this String @this, Boolean readOnly, Boolean isPublic = true)
            => new PropertySymbol(@this,
                                  @this.EmptyPropertyGet(true).Some<FunctionSymbol>(),
                                  readOnly ? Option.None : @this.EmptyPropertySet(true).Some<FunctionSymbol>());

        public static IEnumerable<Symbol> Members(params Symbol[] members)
            => members;
    }

}

