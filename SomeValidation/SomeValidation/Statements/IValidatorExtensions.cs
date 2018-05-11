namespace SomeValidation.Statements
{
    public static class IValidatorExtensions
    {
        public static IBeStatement<T> ShouldBe<T>(this IValidator v, object param, T instance, string messageOverride = null)
        {
            return new BeStatement<T>
            {
                Validator = v,
                Param = param,
                Value = instance,
                Negation = false,
                Message = messageOverride ?? "{{0}} should be {0}."
            };
        }

        public static IBeStatement<T> ShouldNotBe<T>(this IValidator v, object param, T instance, string messageOverride = null)
        {
            return new BeStatement<T>
            {
                Validator = v,
                Param = param,
                Value = instance,
                Negation = true,
                Message = messageOverride ?? "{{0}} should not be {0}."
            };
        }

        public static IBeStatement<T> CannotBe<T>(this IValidator v, object param, T instance, string messageOverride = null)
        {
            return new BeStatement<T>
            {
                Validator = v,
                Param = param,
                Value = instance,
                Negation = true,
                Message = messageOverride ?? "{{0}} cannot be {0}."
            };
        }
    }
}
