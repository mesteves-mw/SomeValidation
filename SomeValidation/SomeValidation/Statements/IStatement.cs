namespace SomeValidation.Statements
{
    public interface IStatement
    {
        IValidator Validator { get; set; }
        string Message { get; set; }
        object Param { get; set; }
        bool Negation { get; }
        int ErrorsRaised { get; set; }
    }
}