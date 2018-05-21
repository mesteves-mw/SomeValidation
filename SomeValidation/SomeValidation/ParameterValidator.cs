namespace SomeValidation
{
    using System;

    public abstract class ParameterValidator<T> : AbstractValidator<T>
    {
        protected static ParameterInfo Param(string parameterName) => new ParameterInfo(parameterName);

        /// <summary>
        /// Call delegate to carry parent parameter name context. e.g. forName("MyParam") returns "parentName.MyParam".
        /// </summary>
        protected delegate ParameterInfoNode ForName(ParameterInfo parameter = null);

        public override void Validate(T instance, params Guid[] ruleSet)
        {
            this.Validate((ParameterInfo)null, instance, ruleSet);
        }

        public void Validate(string parameterName, T instance, params Guid[] ruleSet)
        {
            this.Validate(Param(parameterName), instance, ruleSet);
        }

        public void Validate(ParameterInfo parameter, T instance, params Guid[] ruleSet)
        {
            ForName forName = p => ParameterInfoNode.ChainParameters(parameter, p);

            this.Validate(forName, instance, ruleSet);
        }

        protected abstract void Validate(ForName forName, T instance, params Guid[] ruleSet);

        public virtual void RaiseError(IParameterInfo parameterName, object parameterValue, string errorMessage)
        {
            this.RaiseError(new ParameterValidationError(parameterName, parameterValue, errorMessage));
        }

        public override void RaiseError(object parameter, string errorMessage)
        {
            this.RaiseError(parameter as IParameterInfo, null, errorMessage);
        }

        public override void RaiseError(object parameterName, object parameterValue, string errorMessage)
        {
            this.RaiseError(parameterName as IParameterInfo, parameterValue, errorMessage);
        }
    }
}
