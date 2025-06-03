using System;
using System.Collections.Generic;
using System.Windows.Media;

using Nysa.Text;
using Nysa.Text.Lexing;

namespace Nysa.CodeAnalysis.Documents;

public class ColorKey
{
    private Int32                         _DefaultColor;
    private Dictionary<Identifier, Int32> _Index;

    public Int32 ColorFor(Identifier id)
        => this._Index.ContainsKey(id)
           ? this._Index[id]
           : this._DefaultColor;

    public Int32 ColorFor(TokenIdentifier tokenId)
    {
        foreach (var id in tokenId.Values())
            if (this._Index.ContainsKey(id))
                return this._Index[id];

        return this._DefaultColor;
    }

    public ColorKey(Int32 defaultColor, IReadOnlyDictionary<Identifier, Int32> index)
    {
        this._DefaultColor  = defaultColor;
        this._Index         = index.ToDictionary();
    }

    public ColorKey(Int32 defaultColor)
    {
        this._DefaultColor  = defaultColor;
        this._Index         = new Dictionary<Identifier, Int32>();
    }

    public void Add(Identifier id, Int32 color)
    {
        if (this._Index.ContainsKey(id))
            this._Index[id] = color;
        else
            this._Index.Add(id, color);
    }

    public void AddRange(IEnumerable<Identifier> ids, Int32 color)
    {
        foreach (var id in ids)
            this.Add(id, color);
    }
}