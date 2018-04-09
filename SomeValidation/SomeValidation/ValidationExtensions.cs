namespace SomeValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static void RaiseError(this AbstractValidator validator, string parameterName, string errorMessage, Guid? guid = null, string ErrorCode = null)
        {
            validator.RaiseError(new ValidationError { ErrorMessage = errorMessage, ParameterName = parameterName, ParameterGuid = guid });
        }

        public static IEnumerable<IValidationError> Validate<T>(this AbstractValidator<T> validator, T instance, string parameterName, params Guid[] ruleSet)
        {
            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(instance, parameterName, ruleSet);

            return errors;
        }

        public static void ValidateAndThrow<T>(this AbstractValidator<T> validator, T instance, string parameterName, params Guid[] ruleSet)
        {
            IEnumerable<IValidationError> errors = Validate(validator, instance, parameterName, ruleSet);
            
            throw new ValidationException(
                "Validation failed:\r\n -- " + 
                string.Join(
                    "\r\n -- ", 
                    errors.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName))),
                errors);
        }

        public static void ShouldNotBeNull(this AbstractValidator validator, string parameterName, string value)
        {
            if (value == null)
                validator.RaiseError(parameterName, "{0} is null!");
        }
    }
}
