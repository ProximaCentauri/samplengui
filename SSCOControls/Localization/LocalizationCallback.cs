using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows;

namespace SSCOControls
{
    /// <summary>
    /// Provides a localized value.
    /// </summary>
    /// <param name="culture">The culture to use for formatting.</param>
    /// <param name="uiCulture">The culture to use for language.</param>
    /// <returns>The localized value.</returns>
    public delegate object LocalizationCallback(CultureInfo culture, CultureInfo uiCulture, object parameter);

    /// <summary>
    /// Returns whether the target is in primary language or not.
    /// </summary>
    /// <param name="target">The target Dependency Object.</param>
    /// <returns>If target is in primary language or not.</returns>
    public delegate bool PrimaryLanguageCallback(DependencyObject target);
}
