using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.ComponentModel;
using Nysa.Scheduling.Dates;

namespace Nysa.Scheduling.RuleVisualizer
{

    public class RulesViewModel : ModelObject
    {
        public IReadOnlyList<RuleViewModel> Items { get; private set; }

        public RulesViewModel(IReadOnlyList<DateRule> dateRules)
            => this.Items = dateRules.Select(r => new RuleViewModel(r)).ToList();
    }

}
