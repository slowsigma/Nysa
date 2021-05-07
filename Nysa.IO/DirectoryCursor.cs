using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.IO
{
    
    public class DirectoryCursor
    {
        private static readonly Char   SepChar   = Path.DirectorySeparatorChar;
        private static readonly String SepString = new String(Path.DirectorySeparatorChar, 1);
        private static readonly String[] SepSplit = new String[] { SepString };

        // instance members
        private List<String>    _Parts;
        private Int32           _Search;     // the index where the folder is not yet resolved
                                             // the rule is that if current is > 0 then some existing path has been selected
                                             // this._Search also acts as a count to this._Parts
        private List<String>    _Alternates; // should always have something; starts with drives when _Search = 0;
        private Int32           _Completion; // -1 when the search part has no match to an alternate,
                                             // points to a selected alternate when searh is empty or matches the start of an alternate

        /// <summary>
        /// Represents the current confirmed path without the unconfirmed search portion. This path is never elided.
        /// </summary>
        public String Current           => this.ConfirmedPath();


        // used as a basis for Search and Completion properties
        private String CurrentElided    =>   this._Search >  2 ? String.Concat("...", SepString, String.Join(SepString, this._Parts.Skip(this._Search - 2).Take(2)), SepString)
                                           : this._Search == 2 ? String.Concat(String.Join(SepString, this._Parts.Take(this._Search)), SepString)
                                           : this._Search == 1 ? String.Concat(this._Parts[0], SepString)
                                           :                     String.Empty;

        /// <summary>
        /// Represents the current path along with the currently selected completion.
        /// </summary>
        public String Completion        => this._Completion >= 0
                                           ? String.Concat(this.CurrentElided, this._Alternates[this._Completion])
                                           : this.CurrentElided;

        /// <summary>
        /// Represents the current path along with whatever string of characters have been entered to find a possible completion.
        /// </summary>
        public String Search            => String.Concat(this.CurrentElided, this._Parts[this._Search]);

        public event EventHandler? CurrentChanged;

        public DirectoryCursor(String path = "")
        {
            // initialize
            this._Parts         = new List<String>(String.Empty.Enumerable());
            this._Search        = 0;
            this._Alternates    = DriveInfo.GetDrives()
                                           .Select(v => v.Name.EndsWith(SepString) ? v.Name.Substring(0, v.Name.Length - 1) : v.Name)
                                           .ToList();
            this._Completion    = this._Alternates.Count > 0 ? 0 : -1;

            this.SetFullPath(path);
        }

        private void SetCompletion()
        {
            this._Completion = this.AlternateStartingWith(this._Parts[this._Search]).Or(-1); // will be zero if search part is an empty string
            this.EnsureSearchCasing();
        }

        private void SetFullPath(String fullPath)
        {
            var parts   = fullPath.Split(SepSplit, StringSplitOptions.None); // get path parts
            var current = 0;                                                // start at beginning
            var hit     = this.AlternateMatching(parts[current]);           // get possible hit @ current

            while (hit is Some<Int32> found)                                // while we keep getting a hit
            {
                this.TakeAlternate(found.Value);                            // accept the hit

                current++;                                                  // move to next part
                if (current >= parts.Length)                                // unless there are no more
                    break;

                hit = this.AlternateMatching(parts[current]);               // update the current hit
            }

            if (current < parts.Length)                                     // if we bailed due to non-hit
                this._Parts[this._Search] = parts[current];                 // then current is really the search

            // this.CurrentElided will need to change to a transformation on this._Parts

            this.SetCompletion();                                           // update completion index and search casing
        }

        private void EnsureSearchCasing()
        {
            if (this._Completion >= 0 && this._Parts[this._Search].Length > 0)
                this._Parts[this._Search] = this._Alternates[this._Completion].Substring(0, this._Parts[this._Search].Length);
        }

        private Option<Int32> AlternateMatching(String folder)
            => this._Alternates.Select((a, i) => a.DataEquals(folder) ? i : -1).FirstOrNone(i => i >= 0);

        private Option<Int32> AlternateStartingWith(String folder)
            => this._Alternates.Select((a, i) => a.DataStartsWith(folder) ? i : -1).FirstOrNone(i => i >= 0);

        private String ConfirmedPath()
            => this._Search == 0
               ? String.Empty
               : String.Concat(String.Join(SepString, this._Parts.Take(this._Search)), SepChar);

        private void TakeAlternate(Int32 index) // must be passing in a valid index in this._Alternates
        {
            this._Parts[this._Search] = this._Alternates[index];
            this._Search++;
            if (this._Search >= this._Parts.Count)
                this._Parts.Add(String.Empty);
            else
                this._Parts[this._Search] = String.Empty;

            this._Alternates = Directory.EnumerateDirectories(this.ConfirmedPath()).Select(f => f.Substring(f.LastIndexOf(SepChar) + 1)).ToList();
            this._Completion = this._Alternates.Count > 0 ? 0 : -1;

            if (CurrentChanged != null)
                CurrentChanged.Invoke(this, new EventArgs() { });
        }

        private IEnumerable<(String Value, Int32 Index)> AlternatesFiltered()
            => this._Alternates
                   .Select((p, i) => (Value: p, Index: p.DataStartsWith(this._Parts[this._Search]) ? i : -1))
                   .Where(t => t.Index >= 0);

        public void TakeCompletion()
        {
            if (this._Completion >= 0)
                this.TakeAlternate(this._Completion);
        }

        public void MoveCompletion(Boolean forward)
        {
            if (this._Completion >= 0)
            {
                var move    = forward ? 1 : -1;
                var ready   = this.AlternatesFiltered().ToArray();
                var current = ready.Select((f, i) => f.Index == this._Completion ? i : -1)
                                   .FirstOrNone(v => v >= 0)
                                   .Or(0); // this should not be possible

                if (ready.Length > 1)
                {
                    var next = (current + move).Make(n => n < 0 ? ready.Length - 1 : n % ready.Length);
                    this._Completion = ready[next].Index;
                    this.EnsureSearchCasing();
                }
            }
        }

        public void Reset(String path = "")
        {
            // initialize
            this._Parts         = new List<String>(Return.Enumerable(String.Empty));
            this._Search        = 0;
            this._Alternates    = DriveInfo.GetDrives()
                                           .Select(v => v.Name.EndsWith(SepString) ? v.Name.Substring(0, v.Name.Length - 1) : v.Name)
                                           .ToList();
            this._Completion    = this._Alternates.Count > 0 ? 0 : -1;

            this.SetFullPath(path);

            if (CurrentChanged != null)
                CurrentChanged.Invoke(this, new EventArgs() { });
        }

        public void Backspace()
        {
            if (this._Parts[this._Search].Length > 0)
                this._Parts[this._Search] = this._Parts[this._Search].Substring(0, this._Parts[this._Search].Length - 1);
            else if (this._Search > 0) // we're backing over a separator
            {
                this._Search--;

                this._Alternates = this._Search == 0
                                   ? DriveInfo.GetDrives()
                                              .Select(v => v.Name.EndsWith(SepString) ? v.Name.Substring(0, v.Name.Length - 1) : v.Name)
                                              .ToList()
                                   : Directory.EnumerateDirectories(this.ConfirmedPath()).Select(f => f.Substring(f.LastIndexOf(SepChar) + 1))
                                              .ToList();

                this._Completion = this.AlternateMatching(this._Parts[this._Search])
                                       .Or(-1); // this should not be possible

                if (CurrentChanged != null)
                    CurrentChanged.Invoke(this, new EventArgs() { });
            }
        }

        public void Append(String values)
        {
            foreach (var letter in values)
            {
                var value = new String(letter, 1);

                if (value == SepString && this._Completion >= 0)
                    this.TakeAlternate(this._Completion);
                else
                {
                    this._Parts[this._Search] = String.Concat(this._Parts[this._Search], value);
                    this.SetCompletion();
                    this.EnsureSearchCasing();
                }
            }
        }

    }

}
