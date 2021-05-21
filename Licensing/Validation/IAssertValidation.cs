﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Licensing.Validation
{
    /// <summary>
    /// Interface for the fluent validation syntax.
    /// </summary>
    public interface IAssertValidation : IFluentInterface
    {
        /// <summary>
        /// Invokes the license assertion.
        /// </summary>
        /// <returns>An array is <see cref="IValidationFailure"/> when the validation fails.</returns>
        IEnumerable<IValidationFailure> AssertValidLicense();
    }
}
