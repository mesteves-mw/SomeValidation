using System;
using System.Globalization;

namespace SomeValidation.Statements
{
    public static class IComparableIBeStatementExtensions
    {
        public static IBeStatement<T> GreaterThan<T>(this IBeStatement<T> ssb, T expected) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(expected) > 0, "greater than " + expected);
        }

        public static IBeStatement<T> GreaterThanOrEqualTo<T>(this IBeStatement<T> ssb, T expected) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(expected) >= 0, "greater than or equal to " + expected);
        }

        public static IBeStatement<T> LessThan<T>(this IBeStatement<T> ssb, T expected) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(expected) < 0, "less than " + expected);
        }

        public static IBeStatement<T> LessThanOrEqualTo<T>(this IBeStatement<T> ssb, T expected) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(expected) <= 0, "less than or equal to " + expected);
        }

        public static IBeStatement<T> Between<T>(this IBeStatement<T> ssb, T min, T max) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(min) >= 0 && ssb.Value.CompareTo(max) <= 0, "between " + min + " and " + max);
        }

        public static IBeStatement<T> InRange<T>(this IBeStatement<T> ssb, T min, T max) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(min) >= 0 && ssb.Value.CompareTo(max) <= 0, "in range (" + min + "," + max + ")");
        }

        public static IBeStatement<T> AtLeastAndAtMost<T>(this IBeStatement<T> ssb, T least, T most) where T : IComparable<T>
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(least) >= 0 && ssb.Value.CompareTo(most) <= 0, "at least " + least + " and at most " + most);
        }

        public static IBeStatement<T> Negative<T>(this IBeStatement<T> ssb) where T : IComparable, IConvertible
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(default(T)) < 0, "negative");
        }

        public static IBeStatement<T> Positive<T>(this IBeStatement<T> ssb) where T : IComparable<T>, IComparable, IConvertible
        {
            return ssb.ApplyConstraint(ssb.Value.CompareTo(default(T)) > 0, "positive");
        }
    }
}