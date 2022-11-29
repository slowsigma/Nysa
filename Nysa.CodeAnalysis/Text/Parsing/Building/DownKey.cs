using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Text.Parsing.Building;

// Key used to memoize building down.
internal record struct DownKey(ChartEntry Entry, Int32 Position);
