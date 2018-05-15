namespace SomeValidation.InlineValidation
{
    using System;
    using SomeValidation.Statements;
    using System.Collections.Generic;

    public class InlineValidator : AbstractValidator, IValidator
    {
        public InlineValidator(Action<IValidationError> onError)
        {
            this.OnError += onError;
        }

        public static IEnumerable<IValidationError> Validate<K>(K instance, Action<InlineValidator<K>, K> validationAction)
        {
            var validator = new InlineValidator<K>
            {
                ValidationAction = validationAction
            };

            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(instance);

            return errors;
        }

        public static IEnumerable<IValidationError> Validate<K>(string parameterName, K instance, Action<InlineStringParameterValidator<K>, StringParameterValidator<K>.ForName, K> validationAction)
        {
            var validator = new InlineStringParameterValidator<K>
            {
                ValidationAction = validationAction
            };

            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(parameterName, instance);

            return errors;
        }
    }
}
