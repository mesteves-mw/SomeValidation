namespace SomeValidation
{
    using System;

    public class ValidationError : IValidationError
    {
        private string _errorMessageTemplate;

        public ValidationError(string parameterName, string errorMessage)
        {
            this.ParameterName = parameterName;
            this._errorMessageTemplate = errorMessage;
        }

        public string ParameterName { get; set; }

        public string ErrorMessage
        {
            get => _errorMessageTemplate.Replace("@parameterName", this.ParameterName);
            set => _errorMessageTemplate = value;
        }
    }
}
