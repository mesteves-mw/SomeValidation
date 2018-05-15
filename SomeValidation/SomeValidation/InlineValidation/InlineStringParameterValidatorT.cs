namespace SomeValidation.InlineValidation
{
    using System;
    public class InlineStringParameterValidator<T> : StringParameterValidator<T>, IInlineValidator
    {
        public Action<InlineStringParameterValidator<T>, ForName, T> ValidationAction { get; set; }

        protected override void Validate(ForName forName, T instance, params Guid[] ruleSet)
        {
            this.ValidationAction(this, forName, instance);
        }
    }
}
