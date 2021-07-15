using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Logics
{

    public class StateMachine<S, V>
        where S : IEquatable<S>
        where V : IEquatable<V>
    {
        public class InputContainsElseValueException : Exception
        {
            public InputContainsElseValueException() : base(@"The StateMachine.ElseValue is not allowed as a value argument. Pick an ElseValue that does not occur in the values the StateMachine must process.") { }
        }

        private StateTransitions<S, V>  _Transitions;

        public StateMachine(StateTransitions<S, V> transitions, V elseValue, S initialState)
        {
            this._Transitions   = transitions;
            this.ElseValue      = elseValue;
            this.State          = initialState;
        }

        public V ElseValue { get; init; }
        public S State;

        public void Change(V value)
        {
            if (value.Equals(this.ElseValue))
                throw new InputContainsElseValueException();
            else if (this._Transitions.ContainsTransition(this.State, value))
                this.State = this._Transitions.Transition(this.State, value);
            else if (this._Transitions.ContainsTransition(this.State, this.ElseValue))
                this.State = this._Transitions.Transition(this.State, this.ElseValue);
        }
    }

}