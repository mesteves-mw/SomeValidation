namespace SomeValidation
{
    public interface IValidationError
    {
        string ParameterName { get; }
        string ErrorMessage { get; }
    }
}
