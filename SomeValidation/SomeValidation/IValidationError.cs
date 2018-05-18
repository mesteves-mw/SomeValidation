namespace SomeValidation
{
    public interface IValidationError
    {
        string ParameterName { get; }
        object ParameterValue { get; }
        string ErrorMessage { get; }
    }
}
