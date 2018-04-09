namespace SomeValidation
{
    public interface IValidationError
    {
        string ParameterName { get; set; }
        string ErrorMessage { get; set; }
    }
}
