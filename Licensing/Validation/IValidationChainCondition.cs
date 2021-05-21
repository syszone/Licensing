﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Licensing.Validation
{
    /// <summary>
    /// Interface for the fluent validation syntax.
    /// </summary>
    public interface IValidationChainCondition : IFluentInterface
    {
        /// <summary>
        /// Adds a when predicate to the current validator.
        /// </summary>
        /// <param name="predicate">The predicate that defines the conditions.</param>
        /// <returns>An instance of <see cref="ICompleteValidationChain"/>.</returns>
        ICompleteValidationChain When(Predicate<License> predicate);
    }
}
