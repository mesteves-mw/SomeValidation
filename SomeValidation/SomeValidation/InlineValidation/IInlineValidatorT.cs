namespace SomeValidation.InlineValidation
{
    using System;

    public class InlineValidator<T> : AbstractValidator<T>, IInlineValidator
    {
        public Action<InlineValidator<T>, T> ValidationAction { get; set; }

        public override void Validate(T instance, params Guid[] ruleSet)
        {
            this.ValidationAction(this, instance);
        }
    }
}
