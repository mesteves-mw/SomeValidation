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
        /// <summary>
        /// Call delegate to carry parent parameter name context. e.g. forName("MyParam") returns "parentName.MyParam".
        /// </summary>
        protected delegate string ForName(string parameterName = null);

        public void Validate(T t, string parameterName, params Guid[] ruleSet)
        {
            ForName forName = p => p != null 
                                    ? parameterName + "." + p 
                                    : parameterName;

            this.Validate(t, forName, ruleSet);
        }

        protected abstract void Validate(T t, ForName forName, params Guid[] ruleSet);
    }
}
