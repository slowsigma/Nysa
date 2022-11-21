using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing
{

    public class Chart : IEnumerable<ChartPosition>
    {
        // instance members
        public Grammar Grammar { get; private set; }

        internal List<ChartEntry>[]   _Data;

        public IReadOnlyList<List<ChartEntry>> Data => this._Data;

        internal Chart(Grammar grammar, Int32 size)
        {
            this.Grammar        = grammar;
            this._Data          = new List<ChartEntry>[size];
        }

        public ChartPosition this[Int32 index]
        {
            get => (index >= 0) && (index < this._Data.Length)
                   ? new ChartPosition(this, index)
                   : throw new IndexOutOfRangeException();
        }

        public IEnumerator<ChartPosition> GetEnumerator()
        {
            foreach (var p in this._Data.Select((l, i) => new ChartPosition(this, i)))
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public Int32 Length { get => this._Data.Length; }

        public override string ToString()
        {
            var build = new StringBuilder();

            foreach (var position in this)
            {
                build.AppendLine($"[Position {position.Index}]");
                
                foreach (var entry in position.Where(e => true))
                {
                    build.AppendLine(entry.ToString());
                }
            }

            return build.ToString();
        }

    }

}
