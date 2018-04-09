namespace SomeValidation
{
    public interface IValidationFailure
    {
        string ParameterName { get; set; }
        string ErrorMessage { get; set; }
    }
}
