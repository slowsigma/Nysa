using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    // Note that for the purpose of transformation, SemanticItem is restricted to CodeNode types.
    public sealed class SemanticItem : TransformItem
    {
        public static implicit operator SemanticItem(CodeNode codeNode) => new SemanticItem(codeNode);

        public CodeNode Value { get; private set; }

        public SemanticItem(CodeNode value) { this.Value = value; }
    }

}