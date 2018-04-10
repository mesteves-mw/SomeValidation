namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        public event Action<IValidationError> OnError;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();

            t.OnError = OnError;

            return t;
        }

        public void RaiseError(IValidationError failure)
        {
            OnError(failure);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        public abstract void Validate(T instance, params Guid[] ruleSet);
    }
}
