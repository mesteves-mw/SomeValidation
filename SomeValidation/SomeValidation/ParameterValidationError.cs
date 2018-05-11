namespace SomeValidation
{
    using System;

    public class ParameterValidationError : IValidationError
    {
        private string _errorMessageTemplate;

        public ParameterValidationError(IParameterInfo parameter, string errorMessage)
        {
            this.Parameter = parameter;
            this._errorMessageTemplate = errorMessage;
        }

        public string ParameterName => Parameter.Name;
        public IParameterInfo Parameter { get; set; }
        public string ErrorMessage
        {
            get => _errorMessageTemplate.Replace("@parameterName", this.ParameterName);
            set => _errorMessageTemplate = value;
        }
    }
}
