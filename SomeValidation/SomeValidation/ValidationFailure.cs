namespace SomeValidation
{
    using System;

    public class ValidationFailure : IValidationFailure
    {
        public Guid? ParameterGuid { get; set; }
        public string ParameterName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
