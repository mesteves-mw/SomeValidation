namespace SomeValidation
{
    using System;

    public abstract class AbstractValidator
    {
        public event Action<IValidationFailure> OnError;

        public T Create<T>() where T : AbstractValidator, new()
        {
            var t = new T();

            t.OnError = OnError;

            return t;
        }

        public void RaiseError(IValidationFailure failure)
        {
            OnError(failure);
        }
    }

    public abstract class AbstractValidator<T> : AbstractValidator
    {
        /// <summary>
        /// Call delegate to carry parent parameter name context. e.g. forName("MyParam") returns "parentName.MyParam".
        /// </summary>
        protected delegate string ForName(string parameterName = null);

        public void Validate(T t, string pname)
        {
            ForName forName = p => p != null ? pname + "." + p : pname;

            this.Validate(t, forName);
        }

        protected abstract void Validate(T t, ForName forName);
    }
}
