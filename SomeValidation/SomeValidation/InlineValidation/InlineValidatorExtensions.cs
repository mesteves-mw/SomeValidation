namespace SomeValidation.InlineValidation
{
    using System;

    public static class InlineValidatorExtensions
    {
        public static void Validate<K>(this IInlineValidator parentValidator, string parameterName, K instance, Action<InlineStringParameterValidator<K>, StringParameterValidator<K>.ForName, K> validationAction)
        {
            var validator = parentValidator.Create<InlineStringParameterValidator<K>>();
            validator.ValidationAction = validationAction;

            validator.Validate(parameterName, instance);
        }
    }
}
