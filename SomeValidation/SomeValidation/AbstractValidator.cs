namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        protected string ClassParameterName { get; set; }

        public event Action<string, string> OnError;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();
            t.ClassParameterName = this.ClassParameterName;

            t.OnError = OnError;

            return t;
        }

        public void RaiseError(string propertyName, string message)
        {
            OnError(this.ClassParameterName + "." + propertyName, message);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        private readonly object syncRoot = new object();
        public void Validate(T t, string pname)
        {
            lock (syncRoot)
            {
                var oldParam = this.ClassParameterName;

                if (!string.IsNullOrEmpty(this.ClassParameterName))
                {
                    pname = this.ClassParameterName + "." + pname;
                }

                this.ClassParameterName = pname;

                this.Validate(t);

                this.ClassParameterName = oldParam;
            }
        }

        public abstract void Validate(T t);
    }

}
