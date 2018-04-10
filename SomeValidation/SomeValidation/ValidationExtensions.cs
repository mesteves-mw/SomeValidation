namespace SomeValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationExtensions
    {
        public static void RaiseError(this AbstractValidator validator, string parameterName, string errorMessage)
        {
            validator.RaiseError(new ValidationError(parameterName, errorMessage));
        }

        public static void RaiseError<T>(this StringParameterValidator<T> validator, string parameterName, string errorMessage)
        {
            validator.RaiseError(new ValidationError(parameterName, errorMessage));
        }

        public static void RaiseError<T>(this ParameterValidator<T> validator, IParameterInfo parameter, string errorMessage)
        {
            validator.RaiseError(new ParameterValidationError(parameter, errorMessage));
        }

        public static IEnumerable<IValidationError> Validate<T>(this AbstractValidator<T> validator, T instance, params Guid[] ruleSet)
        {
            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(instance, ruleSet);

            return errors;
        }

        public static void ValidateAndThrow<T>(this AbstractValidator<T> validator, T instance, params Guid[] ruleSet)
        {
            IEnumerable<IValidationError> errors = Validate(validator, instance, ruleSet);

            throw new ValidationException(
                "Validation failed:\r\n -- " +
                string.Join(
                    "\r\n -- ",
                    errors.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName))),
                errors);
        }

        public static IEnumerable<IValidationError> Validate<T>(this StringParameterValidator<T> validator, string parameterName, T instance, params Guid[] ruleSet)
        {
            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(parameterName, instance, ruleSet);

            return errors;
        }

        public static void ValidateAndThrow<T>(this StringParameterValidator<T> validator, string parameterName, T instance, params Guid[] ruleSet)
        {
            IEnumerable<IValidationError> errors = Validate(validator, parameterName, instance, ruleSet);
            
            throw new ValidationException(
                "Validation failed:\r\n -- " + 
                string.Join(
                    "\r\n -- ", 
                    errors.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName))),
                errors);
        }

        public static IEnumerable<IValidationError> Validate<T>(this ParameterValidator<T> validator, string parameterName, T instance, params Guid[] ruleSet)
        {
            var errors = new List<IValidationError>();
            validator.OnError += errors.Add;

            validator.Validate(new ParameterInfo(parameterName), instance, ruleSet);

            return errors;
        }

        public static void ValidateAndThrow<T>(this ParameterValidator<T> validator, string parameterName, T instance, params Guid[] ruleSet)
        {
            IEnumerable<IValidationError> errors = Validate(validator, parameterName, instance, ruleSet);

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

        public static void ShouldNotBeNull<T>(this ParameterValidator<T> validator, IParameterInfo parameter, string value)
        {
            if (value == null)
                validator.RaiseError(parameter, "{0} is null!");
        }
    }
}
