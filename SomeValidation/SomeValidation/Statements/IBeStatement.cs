namespace SomeValidation.Statements
{
    public interface IBeStatement<T> : IStatement
    {
        T Value { get; set; }
    }

}
