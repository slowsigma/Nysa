using System;

namespace Nysa.CodeAnalysis.VbScript;

// Because VbScript only allows comments as the last token before a line return,
// the LineCommentIndex is a single nullable value.
public record struct VbScriptLine(
    Int32 StartTokenIndex,      // token index of the line start (first line always has zero here)
    Int32 EndTokenIndex,        // token index of the line end (all but the last line point to {new-line} tokens)
    Int32? LineCommentIndex     // when not null, the index of the comment on this line
);
