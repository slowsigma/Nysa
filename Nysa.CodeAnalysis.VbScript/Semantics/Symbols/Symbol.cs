using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    // assumptions
    //    there will be no dim statements as class members
    //    there will be no const statements as class members

    //    a class getter with one or more args is really a function
    //    a class set/let with more than one arg is really a function
    //    we assume for now, class get vs set/let may have different own visibility

    public abstract record Symbol
    {
        /// <summary>
        /// The name that is used to lookup the symbol.
        /// </summary>
        public String Name { get; private set; }

        protected Symbol(String name)
        {
            this.Name = name;
        }
    }

}

