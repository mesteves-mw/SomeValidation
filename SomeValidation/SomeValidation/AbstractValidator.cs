namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        public string ClassParameterName { get; set; }

        public event Action<string, string> OnError;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();
            t.ClassParameterName = this.ClassParameterName;

            t.OnError = (a, b) => OnError(a, b);

            return t;
        }

        public void RaiseError(string propertyName, string message)
        {
            OnError(this.ClassParameterName + "." + propertyName, message);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        public void Validate(T t, string pname)
        {
            if (string.IsNullOrEmpty(this.ClassParameterName))
            {
                this.ClassParameterName = pname;
            }
            else
            {
                this.ClassParameterName += "." + pname;
            }

            this.Validate(t);
        }

        public abstract void Validate(T t);
    }
}
