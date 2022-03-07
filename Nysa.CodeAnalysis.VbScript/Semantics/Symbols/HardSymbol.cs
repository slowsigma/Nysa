using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record HardSymbol : Symbol
    {
        public Option<String> NewName       { get; private set; }
        /// <summary>
        /// Used to flag the use of this in code as an error.
        /// Used primarily to flag global functions having no
        /// equivalent translation as needing manual attention.
        /// </summary>
        public Option<String> ErrMessage    { get; private set; }

        protected HardSymbol(String name, Option<String> newName, Option<String> errMessage)
            : base(name)
        {
            if (newName is Some<String> && errMessage is Some<String>)
                throw new Exception("Symbols that generate an error cannot be renamed.");

            this.NewName    = newName;
            this.ErrMessage = errMessage;
        }
    }

}

