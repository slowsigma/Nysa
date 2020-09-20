using System;

namespace Nysa.Text.Parsing
{

    /// <summary>
    /// With a parse, some nodes are not desired in the final tree. So,
    /// the parsing engine can remove them once the parse is complete 
    /// and the NodeRetentionType values specify how that happens.
    /// </summary>
    public enum NodeRetentionType
    {
        /// <summary>
        /// Keep nodes created for this symbol (default).
        /// </summary>
        Keep,
        /// <summary>
        /// Remove any node for this symbol regardless of child content.
        /// </summary>
        Remove,
        /// <summary>
        /// Remove nodes created for this symbol when they're empty (i.e., no children).
        /// </summary>
        RemoveEmpty,
        /// <summary>
        /// Give the content of this node to the parent in place of it,
        /// maintaining all content position relative to parent siblings.
        /// </summary>
        Collapse,
        /// <summary>
        /// When only one child node exists, give that child to the
        /// parent in place of this node, maintaining all content
        /// position relative to parent siblings.
        /// </summary>
        CollapseSingle,
        /// <summary>
        /// Give the content of this node to the parent in place of it
        /// when the symbol of this node is the same as the parent,
        /// maintaining all content position relative to parent siblings.
        /// Note, it is possible for the final tree to have a node with rollup
        /// within a node of the same symbol because an intermediate node
        /// had collapse retention that blocked rollup but was removed.
        /// </summary>
        Rollup
    }

}
