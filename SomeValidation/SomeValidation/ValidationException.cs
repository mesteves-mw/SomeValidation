namespace SomeValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An exception that represents failed validation.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Gets validation errors.
        /// </summary>
        public IEnumerable<IValidationError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        public ValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The string message.</param>
        public ValidationException(string message)
            : this(message, Enumerable.Empty<IValidationError>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The string message.</param>
        /// <param name="errors">Validation errors.</param>
        public ValidationException(string message, IEnumerable<IValidationError> errors)
            : base(message)
        {
            this.Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The string message.</param>
        /// <param name="inner">The inner exception.</param>
        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {
            this.Errors = Enumerable.Empty<IValidationError>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The string message.</param>
        /// <param name="errors">Validation errors.</param>
        /// <param name="inner">The inner exception.</param>
        public ValidationException(string message, IEnumerable<IValidationError> errors, Exception inner)
            : base(message, inner)
        {
            this.Errors = errors;
        }
    }
}
