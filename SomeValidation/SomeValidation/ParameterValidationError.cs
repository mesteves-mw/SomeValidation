namespace SomeValidation
{
    using System;

    public class ParameterValidationError : IValidationError
    {
        private string _errorMessageTemplate;

        public ParameterValidationError(IParameterInfo parameter, object parameterValue, string errorMessage)
        {
            this.Parameter = parameter;
            this.ParameterValue = parameterValue;
            this._errorMessageTemplate = errorMessage;
        }

        public string ParameterName => Parameter.Name;
        public object ParameterValue { get; }
        public IParameterInfo Parameter { get; set; }
        public string ErrorMessage
        {
            get => _errorMessageTemplate.Replace("@parameterName", this.ParameterName);
            set => _errorMessageTemplate = value;
        }
    }
}
