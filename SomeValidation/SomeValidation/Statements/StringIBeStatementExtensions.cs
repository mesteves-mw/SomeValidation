using System.Text.RegularExpressions;

namespace SomeValidation.Statements
{
    public static class StringIBeStatementExtensions
    {
        public static IBeStatement<string> Empty(this IBeStatement<string> ssb)
        {
            return ssb.ApplyConstraint(ssb.Value == string.Empty, "empty");
        }

        public static IBeStatement<string> NullOrEmpty(this IBeStatement<string> ssb)
        {
            return ssb.ApplyConstraint(string.IsNullOrEmpty(ssb.Value), "null or empty");
        }
        public static IBeStatement<string> Matching(this IBeStatement<string> ssb, string pattern)
        {
            return ssb.ApplyConstraint(Regex.IsMatch(ssb.Value, pattern), "matching pattern '" + pattern + "'");
        }

        public static IBeStatement<int> Length(this IBeStatement<string> ssb)
        {
            return new BeStatement<int>
            {
                Validator =  ssb.Validator,
                Param = ssb.Param,
                Message = ssb.Message.Replace("@parameterName", "@parameterName length"),
                Negation = ssb.Negation,
                Value = ssb.Value?.Length ?? 0
            };
        }
    }
}