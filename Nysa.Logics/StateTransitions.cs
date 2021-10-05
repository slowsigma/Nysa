using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Logics
{

    public class StateTransitions<S, V>
        where S : notnull, IEquatable<S>
        where V : notnull, IEquatable<V>
    {
        private static readonly String _TransitionExists          = @"Transition already exists for the given StateValue.";
        private static readonly String _TransitionDoesNotExists   = @"Transition does not exist for the given StateValue.";

        private Dictionary<StateValue<S, V>, S> _Items;

        public StateTransitions()
        {
            this._Items = new Dictionary<StateValue<S, V>, S>();
        }

        public void Add(S state, V value, S nextState)
        {
            if (this._Items.ContainsKey(new StateValue<S, V>(state, value))) throw new ArgumentException(_TransitionExists);

            this._Items.Add(new StateValue<S, V>(state, value), nextState);
        }

        public void Add(S state, IEnumerable<V> values, S nextState)
        {
            foreach (V value in values)
            {
                if (this._Items.ContainsKey(new StateValue<S, V>(state, value))) throw new ArgumentException(_TransitionExists);

                this._Items.Add(new StateValue<S, V>(state, value), nextState);
            }
        }

        public void Add(IEnumerable<S> states, V value, S nextState)
        {
            foreach (S state in states)
                this.Add(state, value, nextState);
        }

        public void Add(IEnumerable<S> states, IEnumerable<V> values, S nextState)
        {
            foreach (S state in states)
                this.Add(state, values, nextState);
        }

        public Boolean ContainsTransition(S state, V value)
            => this._Items.ContainsKey(new StateValue<S, V>(state, value));

        public S Transition(S state, V value)
        {
            S? nextState;

            if (this._Items.TryGetValue(new StateValue<S, V>(state, value), out nextState))
                return nextState;

            throw new ArgumentOutOfRangeException(_TransitionDoesNotExists);
        }

    }

}