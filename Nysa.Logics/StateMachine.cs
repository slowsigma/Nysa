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
        private V                       _ElseValue;
        private S                       _State;
        private V                       _Value;

        public StateMachine(V elseValue, S initialState, V initialValue)
            : this(new StateTransitions<S, V>(), elseValue, initialState, initialValue)
        {
        }

        public StateMachine(StateTransitions<S, V> transitions, V elseValue, S initialState, V initialValue)
        {
            this._Transitions   = transitions;
            this._ElseValue     = elseValue;
            this._State         = initialState;
            this._Value         = initialValue;
        }

        public V ElseValue => this._ElseValue;
        public S State => this._State;
        public V Value => this._Value;

        public void SetValue(V value)
        {
            if (value.Equals(this._ElseValue))
                throw new InputContainsElseValueException();
            else if (this._Transitions.ContainsTransition(this._State, value))
                this._State = this._Transitions.Transition(this._State, value);
            else if (this._Transitions.ContainsTransition(this._State, this._ElseValue))
                this._State = this._Transitions.Transition(this._State, this._ElseValue);
        }
    }

}