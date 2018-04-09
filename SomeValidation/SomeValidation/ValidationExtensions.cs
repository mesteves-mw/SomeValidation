namespace SomeValidation
{
    using System;

    public static class ValidationExtensions
    {
        public static void RaiseError(this AbstractValidator validator, string parameterName, string errorMessage, Guid? guid = null, string ErrorCode = null)
        {
            validator.RaiseError(new ValidationFailure { ErrorMessage = errorMessage, ParameterName = parameterName, ParameterGuid = guid });
        }
    }
}
