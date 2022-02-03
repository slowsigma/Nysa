using System;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    /// <summary>
    /// Get some number of the input items and transform into the type T
    /// then return that with the position representing what remains of
    /// the original input items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    public delegate (T Item, Index Remainder) Get<T>(TransformItem[] items, Index startPosition);

}