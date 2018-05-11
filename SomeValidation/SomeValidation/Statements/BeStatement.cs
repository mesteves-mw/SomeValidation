namespace SomeValidation.Statements
{
    public class BeStatement<T> : IBeStatement<T>
    {
        public IValidator Validator { get; set; }
        public string Message { get; set; }
        public object Param { get; set; }
        public T Value { get; set; }
        public bool Negation { get; set; }
        public int ErrorsRaised { get; set; }
    }
}