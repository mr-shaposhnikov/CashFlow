using System;
using JetBrains.Annotations;

namespace CashFlow
{
    public interface IConnectionString
    {
        string Value { get; }
    }

    public class ConnectionString : IConnectionString
    {
        public ConnectionString([NotNull] string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public string Value { get; }
    }
}