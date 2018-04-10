namespace SomeValidation
{
    using System;

    public class ParameterValidationError : IValidationError
    {
        public ParameterValidationError(IParameterInfo parameter, string errorMessage)
        {
            this.Parameter = parameter;
            this.ErrorMessage = errorMessage;
        }

        public string ParameterName => Parameter.Name;
        public IParameterInfo Parameter { get; set; }
        public string ErrorMessage { get; set; }
    }
}
