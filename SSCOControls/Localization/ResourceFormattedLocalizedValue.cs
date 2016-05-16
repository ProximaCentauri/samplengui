﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SSCOControls
{
    /// <summary>
    /// Formats a list of objects to produce a string value. The formatting string is retrieved from
    /// resources.
    /// </summary>
    public class ResourceFormattedLocalizedValue : ResourceLocalizedValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedLocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="args">The args.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceKey"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="args"/> is null.</exception>
        public ResourceFormattedLocalizedValue(LocalizedProperty property, string resourceKey, params object[] args)
            : base(property, resourceKey)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            this.args = args;
        }

        /// <summary>
        /// Retrieves the localized value from resources or by other means.
        /// </summary>
        /// <returns>
        /// The localized value.
        /// </returns>
        protected override object GetLocalizedValue()
        {
            var result = base.GetLocalizedValue();
            if (result == null || false == (result is string))
            {
                return null;
            }
            return string.Format(Property.GetCulture(), (string)result, this.args);
        }

        private object[] args;        
    }
}
