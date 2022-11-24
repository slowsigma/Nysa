using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing;

public interface IGrammarBuildContinue
{
    IGrammarBuildContinue Or(params String[] definition);
    void OrOptional();
}
