namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        public event Action<string, string> OnError;
        public Func<string> GetNewParameterName;
        public Func<string> GetOldParameterName;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();

            t.GetOldParameterName = this.GetNewParameterName;

            t.OnError = (a, b) => OnError(a, b);

            return t;
        }

        public void RaiseError(string propertyName, string message)
        {
            OnError(this.GetNewParameterName() + "." + propertyName, message);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        public void Validate(T t, string pname)
        {
            this.GetNewParameterName = () => this.GetOldParameterName != null ? this.GetOldParameterName() + "." + pname : pname;

            this.Validate(t);
        }

        public abstract void Validate(T t);
    }
}
