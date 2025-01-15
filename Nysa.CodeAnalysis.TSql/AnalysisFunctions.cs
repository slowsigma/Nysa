using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Nysa.CodeAnalysis.TSql;

public static class AnalysisFunctions
{

    internal static ObjectIdentifierPart ToObjectIdentifierPart(this Identifier @this)
        => new ObjectIdentifierPart(@this.StartOffset, @this.Value.Length == 0 ? 0 : @this.FragmentLength, (Int32)@this.QuoteType, @this.Value);

    internal static ObjectIdentifier ToObjectIdentifier(this MultiPartIdentifier @this, Boolean canAlias, Boolean isSelectInto = false)
        => new ObjectIdentifier(@this.StartOffset,
                                @this.FragmentLength,
                                @this.StartLine,
                                @this.StartColumn,
                                canAlias,
                                isSelectInto,
                                @this.Identifiers
                                     .Select(i => i.ToObjectIdentifierPart())
                                     .ToList());

    internal static ObjectIdentifier ToObjectIdentifier(this FunctionCall @this, MultiPartIdentifierCallTarget multi, Boolean canAlias)
        => new ObjectIdentifier(@this.StartOffset,
                                @this.FragmentLength,
                                @this.StartLine,
                                @this.StartColumn,
                                canAlias,
                                false,
                                multi.MultiPartIdentifier
                                     .Identifiers
                                     .Select(i => i.ToObjectIdentifierPart())
                                     .Concat(Return.Enumerable(new ObjectIdentifierPart(@this.FunctionName.StartOffset, @this.FunctionName.FragmentLength, (Int32)@this.FunctionName.QuoteType, @this.FunctionName.Value)))
                                     .ToList());

    internal static Option<ObjectIdentifier> ToObjectIdentifier(this FunctionCall @this, Boolean canAlias)
        => @this.CallTarget != null && @this.CallTarget is MultiPartIdentifierCallTarget multi
           ? @this.ToObjectIdentifier(multi, canAlias).Some()
           : Option.None;

    private static IdOnlyVisitor GetIdentifier(this String identifierString, TSqlParser parser)
    {
        var errs   = (IList<ParseError>?)null;
        var result = new IdOnlyVisitor();

        using (var reader = new StringReader(identifierString))
        {
            var tree = parser.Parse(reader, out errs);

            if (errs.Count == 0)
                tree.Accept(result);
        }

        return result;
    }

    public static TSqlAnalysis<T> Analyze<T>(
        this TSqlFragment tree,
        TSqlParser parser,
        T content,
        Func<ObjectIdentifier, Boolean> collectId,
        Func<ObjectIdentifier, Boolean>? collectLookupId,
        Boolean includeComplexLookups,
        Boolean includeDynamicExecs)
    {
        var collector  = new TSqlCollector(collectId, collectLookupId != null, includeComplexLookups, includeDynamicExecs);

        tree.Accept(collector);

        if (collectLookupId != null && collector.LookupLiterals.Count > 0)
        {
            foreach (var literal in collector.LookupLiterals)
            {
                var objId = literal.Value.GetIdentifier(parser);

                if (objId.FoundIdentifier is Some<ObjectIdentifier> someId && collectLookupId(someId.Value))
                    collector.Items.Add(new LookupLiteral(literal.StartOffset, literal.FragmentLength, literal.StartLine, literal.StartColumn, someId.Value.Parts));
            }
        }

        return new TSqlAnalysis<T>(content, collector.Items);
    }

    public static AggregateException Aggregate(this IList<ParseError> @this)
        => new AggregateException(@this.Select(e => new ParseException(e)));

    public static Suspect<TSqlFragment> Parse(this String @this, TSqlParser parser)
    {
        var errs = (IList<ParseError>?)null;

        using (var reader = new StringReader(@this))
        {
            var tree = parser.Parse(reader, out errs);

            return errs.Count == 0
                   ? tree.Confirmed()
                   : Return.Failed<TSqlFragment>(errs.Aggregate());
        }
    }


}