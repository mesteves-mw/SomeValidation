using System;

namespace SomeValidation.Statements
{
    public static class IBeStatementExtensions
    {
        public static IBeStatement<T> Null<T>(this IBeStatement<T> ssb) where T : class
        {
            return ssb.ApplyConstraint(ssb.Value == null, "null");
        }

        public static IBeStatement<Nullable<T>> Null<T>(this IBeStatement<Nullable<T>> ssb) where T : struct
        {
            return ssb.ApplyConstraint(ssb.Value == null, "null");
        }
    }

}
