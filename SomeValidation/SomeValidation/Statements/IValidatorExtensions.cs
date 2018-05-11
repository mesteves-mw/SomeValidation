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
                Message = messageOverride ?? "@parameterName should be @constraintPredicator."
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
                Message = messageOverride ?? "@parameterName should not be @constraintPredicator."
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
                Message = messageOverride ?? "@parameterName cannot be @constraintPredicator."
            };
        }
    }
}
