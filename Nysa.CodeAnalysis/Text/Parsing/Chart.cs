using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dorata.Logics;

namespace Nysa.Text.Parsing
{

    public partial class Chart : IEnumerable<Chart.Position>
    {
        // instance members
        public Grammar Grammar { get; private set; }

        private List<Entry>[]   _Data;
        private Position        _NullPosition;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<Int32, List<(Int32 Position, Int32 Entry)>> _TracePoints;
        public IReadOnlyDictionary<Int32, List<(Int32 Position, Int32 Entry)>> TracePoints { get => this._TracePoints; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<Int32, List<Int32>> _TraceContributors;
        public IReadOnlyDictionary<Int32, List<Int32>> TraceContributors { get => this._TraceContributors; }
        //private Dictionary<Int32, List<Int32>> _CompletionToTraces;

        internal Chart(Grammar grammar, Int32 size)
        {
            this.Grammar        = grammar;
            this._Data          = new List<Entry>[size];
            this._NullPosition  = new Position(null, 0);

            this._TracePoints       = new Dictionary<Int32, List<(Int32 Position, Int32 Entry)>>();
            this._TraceContributors = new Dictionary<Int32, List<Int32>>();
        }

        public Position this[Int32 index]
        {
            get => (index >= 0) && (index < this._Data.Length) ? new Position(this, index) : this._NullPosition;
        }

        public void AddTracePoint(Int32 traceId, Int32 position, Int32 entry)
        {
            if (!this._TracePoints.ContainsKey(traceId))
                this._TracePoints.Add(traceId, new List<(Int32 Position, Int32 Entry)>());
            // Ensure all trace identifiers have at least an empty entry in _TraceContributors.
            if (!this._TraceContributors.ContainsKey(traceId))
                this._TraceContributors.Add(traceId, new List<Int32>());

            this._TracePoints[traceId].Add((position, entry));
        }

        public void AddTraceContributor(Int32 traceId, Int32 contributorTraceId)
        {
            if (!this._TraceContributors.ContainsKey(traceId))
                this._TraceContributors.Add(traceId, new List<Int32>());

            this._TraceContributors[traceId].Add(contributorTraceId);
        }

        public IEnumerator<Position> GetEnumerator()
        {
            foreach (var p in this._Data.Select((l, i) => new Position(this, i)))
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
