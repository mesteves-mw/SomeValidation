using System;

namespace SomeValidation.Statements
{
    public static class IEquatableIBeStatementExtensions
    {
        public static IBeStatement<T> EqualTo<T>(this IBeStatement<T> ssb, T expected) where T : IEquatable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.Equals(expected), "equal to " + expected);
        }

        public static IBeStatement<T> Zero<T>(this IBeStatement<T> ssb) where T : IEquatable<T>, IConvertible
        {
            return ssb.ApplyConstraint(ssb.Value.Equals(0), "zero");
        }
    }
}