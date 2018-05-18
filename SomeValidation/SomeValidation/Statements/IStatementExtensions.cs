using System;

namespace SomeValidation.Statements
{
    public static class IStatementExtensions
    {
        public static IBeStatement<T> ApplyConstraint<T>(this IBeStatement<T> stmt, bool constraintCheck, string constraintPredicator)
        {
            if (!stmt.Negation ^ constraintCheck)
            {
                stmt.Validator.RaiseError(stmt.ParamameterName, stmt.Value, stmt.Message.Replace("@constraintPredicator", constraintPredicator));
                stmt.ErrorsRaised++;
            }

            return stmt;
        }

        public static IBeStatement<T> RaiseError<T>(this IBeStatement<T> stmt)
        {
            stmt.Validator.RaiseError(stmt.ParamameterName, stmt.Value, stmt.Message);
            stmt.ErrorsRaised++;
            return stmt;
        }

        public static IBeStatement<T> OverrideMessage<T>(this IBeStatement<T> stmt, string newStatementMessage)
        {
            stmt.Message = newStatementMessage;
            return stmt;
        }

        public static void Do<T>(this IBeStatement<T> stmt, Action<IBeStatement<T>> action) => action(stmt);

        public static void Then<T>(this IBeStatement<T> stmt, Action<IBeStatement<T>> action)
        {
            if (stmt.ErrorsRaised == 0)
            {
                action(stmt);
            }
        }

        public static IBeStatement<T> Do<T>(this IBeStatement<T> stmt, Func<IBeStatement<T>, IBeStatement<T>> func) => func(stmt);

        public static IBeStatement<T> Then<T>(this IBeStatement<T> stmt, Func<IBeStatement<T>, IBeStatement<T>> func) => stmt.ErrorsRaised == 0 ? func(stmt) : stmt;
    }
}