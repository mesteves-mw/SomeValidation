using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SomeValidation.Statements
{
    public static class ICollectionIBeStatementExtensions
    {
        public static IBeStatement<T> Empty<T>(this IBeStatement<T> ssb) where T : ICollection
        {
            return ssb.ApplyConstraint(ssb.Value.Count == 0, "empty");
        }

        public static IBeStatement<T> NullOrEmpty<T>(this IBeStatement<T> ssb) where T : ICollection
        {
            return ssb.ApplyConstraint(ssb.Value == null || ssb.Value.Count == 0, "null or empty");
        }

        public static IBeStatement<T> EquivalentTo<T,TK>(this IBeStatement<T> ssb, ICollection<TK> expected) where T : ICollection, ICollection<TK>
        {
            return ssb.ApplyConstraint(ssb.Value.SequenceEqual(expected), "equivalent to (" + String.Join(",", expected) + ")");
        }

        public static IBeStatement<int> Count<T>(this IBeStatement<T> ssb) where T : ICollection
        {
            return new BeStatement<int>
            {
                Validator = ssb.Validator,
                ParamameterName = ssb.ParamameterName,
                Message = ssb.Message.Replace("@parameterName", "@parameterName count"),
                Negation = ssb.Negation,
                Value = ssb.Value?.Count ?? 0
            };
        }
    }
}