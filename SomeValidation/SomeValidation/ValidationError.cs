namespace SomeValidation
{
    using System;

    public class ValidationError : IValidationError
    {
        public ValidationError(string parameterName, string errorMessage)
        {
            this.ParameterName = parameterName;
            this.ErrorMessage = errorMessage;
        }

        public Guid? ParameterGuid { get; set; }
        public string ParameterName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
