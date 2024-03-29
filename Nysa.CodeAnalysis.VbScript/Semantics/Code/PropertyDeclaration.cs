﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Nysa.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * define("<PropertyDecl>").Is("<MethodAccessOpt>", "Property", "<PropertyAccessType>", "<ExtendedID>", "<MethodArgList>", "<NL>", "<MethodStmtList>", "End", "Property", "<NL>");
     */

    public class PropertyDeclaration : MethodDeclaration
    {
        public PropertyAccessTypes Access { get; private set; }

        public PropertyDeclaration(SyntaxNode source, Option<VisibilityTypes> visibility, Boolean isDefault, PropertyAccessTypes access, Identifier name, IEnumerable<ArgumentDefinition> arguments, IEnumerable<Statement> statements)
            : base(source, visibility, isDefault, name, arguments, statements)
        {
            this.Access = access;
        }
    }

}
