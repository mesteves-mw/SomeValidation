namespace SomeValidation.Statements
{
    public interface IValidator
    {
        void RaiseError(object parameterName, string errorMessage);

        void RaiseError(object parameterName, object parameterValue, string errorMessage);
    }
}