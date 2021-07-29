using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{
    public sealed class Failed
    {
        public static Failed How(String message) => new Failed(new Exception(message));
        public static Failed How(String message, IEnumerable<Exception> errs) => new Failed(new AggregateException(message, errs));
        public static Failed How(String message, params Exception[] errs) => new Failed(new AggregateException(message, errs));
        public static Failed With(params Exception[] exceptions)
            => exceptions.Length == 1 ? new Failed(exceptions[0]) : new Failed(new AggregateException(exceptions));
        public static Failed With(IEnumerable<Exception> exceptions) => new Failed(new AggregateException(exceptions));

        public Exception Value { get; }
        public Failed(Exception value) { this.Value = value; }
        public override String ToString() => this.Value.Message;
    }
}
