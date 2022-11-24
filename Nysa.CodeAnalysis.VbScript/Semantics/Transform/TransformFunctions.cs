using System;

using Nysa.Logics;

using ParseId     = Nysa.Text.Identifier;
using SyntaxNode  = Nysa.Text.Parsing.Node;
using SyntaxToken = Nysa.Text.Lexing.Token;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public static class TransformFunctions
    {
        public static Transform Then(this Transform @this, Transform next)
            => (c, m) => next(c, @this(c, m));

        // It seems likely that a bottom up approach to building the semantic model would result in less code and complexity.
        // From this assumption, we need to think about each lowest level node (not token). In some cases we take the lowest
        // level syntax node and create a semantic node. As we progress up, we may take the result of lower processing and
        // simply flatten it into a higher level semantic node.

        // If a SyntaxNode does not have a builder for it, then what comes back from processing it is an in-order list of any
        // member CodeNode objects mixed with sibling syntax tokens that bubble up because they were not "caught" in processing
        // (the phrase "not caught" here means that either a CodeNode build returned them or no CodeNode build existed to even
        // process them)

        // The first version of this transformation logic included a builder for tokens as well as syntax nodes. However, it
        // became clear early on that having builders for syntax nodes was overkill for most cases and a mistake for others.
        // It was hard to find any cases where semantic meaning would be derived exclusively from a token.  In most cases,
        // the level of semantic information found in a token is limited to property values of a higher level semantic node.
        // For example, the visibility of a method on a class.  It could be argued that some grammar may exist where a token
        // might have more semantic meaning, but the grammar can always be modified to make the abstract syntax tree contain
        // a parent node for such a token.

        // A NodeTransform function does the following:
        //    1. Takes an array of the tokens and code nodes created for each child node
        //    2. Either:
        //       a. Constructs a single CodeNode to represent the Syntax node which will typically contain the child CodeNode objects passed in
        //       b. Return a new list of CodeNode objects based on the child CodeNode objects passed in
        //       c. Throw an error if something is not consistent with expectations

        // Note: Flattening nodes from a recursive grammar rule...
        // 
        // Is there a syntax node that can be used to gather the results?  In the following case, we don't
        // define a function for <ArgumentList>. That lets the Select method below do all the work of
        // flattening for us. We use a function for <ArgumentListOption> to pull all the semantic nodes
        // created for <Argument> from the BuildResult[] into a higher semantic node for the list.
        //
        //   <ArgumentListOption> ::= <ArgumentList> |
        //   <ArgumentList>       ::=   <Argument> "," <ArgumentList>
        //                            | <Argument>
        //
        // If there is no syntax node that can be used to gather the results, then we need the transform
        // function to do some work to unwrap the child semantic nodes created as the Select function
        // processed the nested syntax nodes.  So, for the previous grammar, if <ArgumentListOption> is
        // not present, and given that our semantic node classes have the same names as the grammar
        // rules, the TransformNode function would look something like this:
        //
        // private static readonly TransformNode ArgListTransform =
        //    (n, m) => m.NextNode<Argument>(Index.Start)
        //               .Make(a => (First: a, Second: m.MaybeNextNode<ArgumentList>(a.Remainder)))
        //               .Make(t => new ArgumentList(n.ToNodeSource(),
        //                                           t.First.Item.Enumerable()
        //                                            .Concat(t.Second.Item.Match(el => el as IEnumerable<Argument>,
        //                                                                        () => Enumerable.Empty<Argument>()))))
        //               .Make(l => new BuildResult[] { (NodeResult) l });
        //
        // This code works because the Select function below works from the bottom up and this means
        // that at every level where ArgListTransform is called, the members will alway be either
        // a single Argument semantic node by itself, or a single Argument semantic node followed by
        // an ArgumentList semantic node where it contains the flat list of Argument semantic nodes
        // created for all lower ArgumentList syntax nodes previously processed.


        public static TransformItem[] Select(this TransformContext @this, Func<ParseId, Option<Transform>> getNodeTransform)
        {
            // create Transform item objects for every child node and token in @this
            // note how each member node gets its own context object from @this
            var members = @this.Node
                               .Members
                               .SelectMany(m => m.Match(n => @this.ForMember(n).Select(getNodeTransform),
                                                        t => Return.Enumerable((TransformItem)(TokenItem)t)))
                               .ToArray();

               /* ?tranform function for this symbol? */
            return getNodeTransform(@this.Node.Id).Match(f => f(@this, members), // yes - call to transform members into one or more new items
                                                         () => members);         //  no - just return all child members in order
        }

        internal static Suspect<Program> ToProgram(this Suspect<SyntaxNode> root)
            => root.Map(r => new TransformContext(r))
                   .Map(c => c.Select(nid => nid.NodeTransform()))
                   .Bind(t => t.Length == 1 && t[0] is SemanticItem item && item.Value is Program program
                              ? program.Confirmed()
                              : (new Exception("Program Error: Could not translate valid parse into semantic model.")).Failed<Program>());

    }

}