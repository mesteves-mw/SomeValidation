namespace SomeValidation.Statements
{
    public interface IValidator
    {
        void RaiseError(object parameterName, string errorMessage);
    }
}