using System;
using System.Xml.Linq;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.TSql;

/// <summary>
/// This visitor is for a tree created from a string that contains only a T-Sql object identifier.
/// </summary>
public class IdOnlyVisitor : TSqlFragmentVisitor
{
    public Option<ObjectIdentifier> FoundIdentifier;

    public IdOnlyVisitor() { this.FoundIdentifier = Option.None; }

    public override void Visit(MultiPartIdentifier node)
    {
        this.FoundIdentifier = node.ToObjectIdentifier(false).Some();

        base.Visit(node);
    }
}
