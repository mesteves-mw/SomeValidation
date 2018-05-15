namespace SomeValidation.InlineValidation
{
    using SomeValidation.Statements;

    public interface IInlineValidator : IValidator
    {
        T Create<T>() where T : AbstractValidator, new();
    }
}
