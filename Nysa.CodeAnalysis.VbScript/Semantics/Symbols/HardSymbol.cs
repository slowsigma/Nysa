using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public abstract record HardSymbol : Symbol
    {
        public Option<String>       NewName     { get; private set; }
        /// <summary>
        /// Used to flag the use of this in code as an error.
        /// Used primarily to flag global functions having no
        /// equivalent translation as needing manual attention.
        /// </summary>
        public Option<String>       Message     { get; private set; }
        public IReadOnlySet<String> Tags        { get;  private set; }

        private protected HardSymbol(String name, Option<String> newName, Option<String> message, IReadOnlySet<String> tags)
            : base(name)
        {
            this.NewName = newName;
            this.Message = message;
            this.Tags    = tags;
        }

        protected HardSymbol(String name, Option<String> newName, Option<String> message, String[]? tags)
            : this(name, newName, message, new HashSet<String>(tags ?? new String[] { }, StringComparer.OrdinalIgnoreCase))
        {
        }

    }

}

