namespace SomeValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static void RaiseError(this AbstractValidator validator, string parameterName, string errorMessage, Guid? guid = null, string ErrorCode = null)
        {
            validator.RaiseError(new ValidationFailure { ErrorMessage = errorMessage, ParameterName = parameterName, ParameterGuid = guid });
        }

        public static IEnumerable<IValidationFailure> Validate<T>(this AbstractValidator<T> validator, T instance, string parameterName, params Guid[] ruleSet)
        {
            var failures = new List<IValidationFailure>();
            validator.OnError += failures.Add;

            validator.Validate(instance, parameterName, ruleSet);

            return failures;
        }

        public static void ValidateAndThrow<T>(this AbstractValidator<T> validator, T instance, string parameterName, params Guid[] ruleSet)
        {
            IEnumerable<IValidationFailure> failures = Validate(validator, instance, parameterName, ruleSet);
            //TODO: add failures list to the new ValidationException
            throw new Exception("Validation failed:\r\n -- " + string.Join("\r\n -- ", failures.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName))));
        }

        public static void ShouldNotBeNull(this AbstractValidator validator, string parameterName, string value)
        {
            if (value == null)
                validator.RaiseError(parameterName, "{0} is null!");
        }
    }
}
