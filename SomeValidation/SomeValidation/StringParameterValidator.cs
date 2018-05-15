namespace SomeValidation
{
    using System;

    public abstract class StringParameterValidator<T> : AbstractValidator<T>
    {
        /// <summary>
        /// Call delegate to carry parent parameter name context. e.g. forName("MyParam") returns "parentName.MyParam".
        /// </summary>
        public delegate string ForName(string parameterName = null);

        public override void Validate(T instance, params Guid[] ruleSet)
        {
            this.Validate((string)null, instance, ruleSet);
        }

        public void Validate(string parameterName, T instance, params Guid[] ruleSet)
        {
            ForName forName = p => p != null 
                                    ? parameterName + "." + p 
                                    : parameterName;

            this.Validate(forName, instance, ruleSet);
        }
        
        protected abstract void Validate(ForName forName, T instance, params Guid[] ruleSet);
    }
}
