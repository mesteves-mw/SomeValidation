namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        public event Action<string, string> OnError;
        protected Func<string> GetParameterName;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();

            t.GetParameterName = this.GetParameterName;

            t.OnError = (a, b) => OnError(a, b);

            return t;
        }

        public void RaiseError(string propertyName, string message)
        {
            OnError(this.GetParameterName() + "." + propertyName, message);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        public void Validate(T t, string pname)
        {
            if (this.GetParameterName != null)
                pname = this.GetParameterName() + "." + pname;

            this.GetParameterName = () => pname;

            this.Validate(t);
        }

        public abstract void Validate(T t);
    }

}
