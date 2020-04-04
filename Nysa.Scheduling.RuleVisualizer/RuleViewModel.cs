using System;

using Nysa.ComponentModel;
using Nysa.Scheduling.Dates;

namespace Nysa.Scheduling.RuleVisualizer
{

    public class RuleViewModel : ModelObject
    {
        public String Name =>   this.Rule is DateSimpleRule simple      ? simple.Name
                              : this.Rule is DateCompoundRule compound  ? compound.Alternatives[0].Name
                              :                                           "{error in code}";
        public DateRule Rule { get; private set; }

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get { return this._IsSelected; }
            set { base.UpdateValueProperty(ref this._IsSelected, value, nameof(IsSelected)); }
        }

        public RuleViewModel(DateRule dateRule) => this.Rule = dateRule;
    }

}
