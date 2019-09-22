using System;
using System.Collections.Generic;
using System.Text;

namespace Nito.Comparers.Fixes
{
    /// <summary>
    /// Provides string comparers that behave exactly like their <see cref="StringComparer"/> counterparts except that they handle <c>null</c> values correctly.
    /// </summary>
    public static class FixedStringComparer
    {
        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-sensitive ordinal string comparison.
        /// </summary>
        public static StringComparer Ordinal { get; } = StringComparer.Ordinal.WithStandardNullHandling().ToStringComparer();

        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-insensitive ordinal string comparison.
        /// </summary>
        public static StringComparer OrdinalIgnoreCase { get; } = StringComparer.OrdinalIgnoreCase.WithStandardNullHandling().ToStringComparer();

#if !NETSTANDARD1_0
        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-sensitive string comparison using the word comparison rules of the invariant culture.
        /// </summary>
        public static StringComparer InvariantCulture { get; } = StringComparer.InvariantCulture.WithStandardNullHandling().ToStringComparer();

        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-insensitive string comparison using the word comparison rules of the invariant culture.
        /// </summary>
        public static StringComparer InvariantCultureIgnoreCase { get; } = StringComparer.InvariantCultureIgnoreCase.WithStandardNullHandling().ToStringComparer();
#endif

        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-sensitive string comparison using the word comparison rules of the current culture.
        /// </summary>
        public static StringComparer CurrentCulture { get; } = StringComparer.CurrentCulture.WithStandardNullHandling().ToStringComparer();

        /// <summary>
        /// Gets a <see cref="StringComparer"/> object that performs a case-insensitive string comparison using the word comparison rules of the current culture.
        /// </summary>
        public static StringComparer CurrentCultureIgnoreCase { get; } = StringComparer.CurrentCultureIgnoreCase.WithStandardNullHandling().ToStringComparer();
    }
}
