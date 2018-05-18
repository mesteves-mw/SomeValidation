namespace SomeValidation
{
    using System;

    public class ValidationError : IValidationError
    {
        private string _errorMessageTemplate;

        public ValidationError(string parameterName, object parameterValue, string errorMessage)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
            this._errorMessageTemplate = errorMessage;
        }

        public string ParameterName { get; set; }
        public object ParameterValue { get; set;  }

        public string ErrorMessage
        {
            get => _errorMessageTemplate.Replace("@parameterName", this.ParameterName);
            set => _errorMessageTemplate = value;
        }
    }
}
