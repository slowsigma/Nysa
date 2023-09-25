using System;
using System.Collections.Generic;

using Microsoft.SqlServer.TransactSql.ScriptDom;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.TSql;

internal class TSqlCollector : TSqlFragmentVisitor
{
    public List<CollectedItem>                  Items;
    public List<Literal>                        LookupLiterals;    // literals in an OBJECT_ID lookup (only collected here not resolved/analyzed)

    private Boolean                             _CanAlias;
    private Func<ObjectIdentifier, Boolean>     _CollectIdentifier;
    private Boolean                             _IncludeLookupIdentifier;
    private Boolean                             _IncludeComplexLookups;
    private Boolean                             _IncludeDynamicExecutes;
    private HashSet<Int32>                      _Included;


    public TSqlCollector(
        Func<ObjectIdentifier, Boolean> collectIdentifier,
        Boolean includeLookupIdentifier,
        Boolean includeComplexLookups = true,
        Boolean includeDynamicExecs = true)
    {
        this.Items = new List<CollectedItem>();
        this.LookupLiterals             = new List<Literal>();

        this._CanAlias                  = false;
        this._CollectIdentifier         = collectIdentifier;
        this._IncludeLookupIdentifier   = includeLookupIdentifier;
        this._IncludeComplexLookups     = includeComplexLookups;
        this._IncludeDynamicExecutes    = includeDynamicExecs;
        this._Included                  = new HashSet<Int32>();
    }

    public override void Visit(TSqlStatement node)
    {
        this._CanAlias =    !((node as StatementWithCtesAndXmlNamespaces) is null)
                         || (node is SetVariableStatement);

        base.Visit(node);
    }

    public override void Visit(BooleanExpression node)
    {
        this._CanAlias = true;

        base.Visit(node);
    }

    public override void Visit(QueryExpression node)
    {
        this._CanAlias = true;

        base.Visit(node);
    }

    public override void Visit(MultiPartIdentifier node)
    {
        if (!this._Included.Contains(node.StartOffset))
        {
            var id = node.ToObjectIdentifier(this._CanAlias);

            if (this._CollectIdentifier(id))
            {
                this.Items.Add(id);
                this._Included.Add(node.StartOffset);
            }
        }

        base.Visit(node);
    }

    public override void Visit(DbccStatement node)
    {
        if (   (   node.Command == DbccCommand.CheckIdent
                || node.Command == DbccCommand.CheckTable)
            && node.Literals.Count > 0
            && node.Literals[0].Value is Literal tableId)
        {
            if (this._IncludeLookupIdentifier)
            {
                this.LookupLiterals.Add(tableId);
                this._Included.Add(node.StartOffset);
            }
        }

        base.Visit(node);
    }

    public override void Visit(FunctionCall node)
    {
        if (!this._Included.Contains(node.StartOffset))
        {
            if (   (   node.FunctionName.Value.DataEquals("object_id") 
                    || node.FunctionName.Value.DataEquals("ident_current"))
                && node.Parameters.Count > 0)
            {
                // literal first argument?
                if (node.Parameters[0] is Literal paramOne && paramOne.LiteralType == LiteralType.String)
                {
                    if (this._IncludeLookupIdentifier)
                    {
                        this.LookupLiterals.Add(paramOne);
                        this._Included.Add(node.StartOffset);
                    }
                }
                else if (this._IncludeComplexLookups)
                {
                    this.Items.Add(new ComplexIdentifierLookup(node.StartOffset, node.FragmentLength, node.StartLine, node.StartColumn));
                    this._Included.Add(node.StartOffset);
                }
            }
            else if (node.ToObjectIdentifier(this._CanAlias) is Some<ObjectIdentifier> someId && this._CollectIdentifier(someId.Value))
            {
                this.Items.Add(someId.Value);
                this._Included.Add(node.StartOffset);
            }
        }

        base.Visit(node);
    }

    public override void Visit(ExecuteStatement node)
    {
        if (node.ExecuteSpecification.ExecutableEntity is ExecutableStringList && this._IncludeDynamicExecutes)
            this.Items.Add(new DynamicExecute(node.StartOffset, node.FragmentLength, node.StartLine, node.StartColumn));
        else if (node.ExecuteSpecification.ExecutableEntity is ExecutableProcedureReference execProcRef)
        {
            var procRef = execProcRef.ProcedureReference.ProcedureReference;

            if (   (procRef != null)
                && procRef.Name.BaseIdentifier.Value.DataEquals("sp_executesql")
                && (procRef.Name.SchemaIdentifier?.Value ?? "sys").DataEquals("sys"))
            {
                this.Items.Add(new DynamicExecute(node.StartOffset, node.FragmentLength, node.StartLine, node.StartColumn));
            }
        }

        this._CanAlias = node.ExecuteSpecification.ExecutableEntity is ExecutableProcedureReference;

        base.Visit(node);
    }

}
