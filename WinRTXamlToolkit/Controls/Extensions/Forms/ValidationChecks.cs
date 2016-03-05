using System;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Defines basic formats accepted by TextBox Text.
    /// </summary>
    [Flags]
    public enum ValidationChecks
    {
        /// <summary>
        /// Any content is valid.
        /// </summary>
        Any = 0,
        /// <summary>
        /// Field value can't be empty.
        /// </summary>
        NonEmpty = 1,
        /// <summary>
        /// Field value needs to be numeric (parse to a double).
        /// </summary>
        Numeric = 2,
        /// <summary>
        /// Field value needs to be as long as MaxLength.
        /// </summary>
        SpecificLength = 4,
        /// <summary>
        /// Field value needs to be at least as long as FieldValidationExtensions.MinLength.
        /// </summary>
        MinLength = 8,
        /// <summary>
        /// Field value needs to match Pattern treated as a regular expression.
        /// </summary>
        MatchesRegexPattern = 16,
        /// <summary>
        /// Field value equals value specified by the FieldValidationExtensions.Pattern property.
        /// </summary>
        EqualsPattern = 32,
        /// <summary>
        /// The field value needs to include lowercase characters.
        /// </summary>
        IncludesLowercase = 64,
        /// <summary>
        /// The field value needs to include uppercase characters.
        /// </summary>
        IncludesUppercase = 128,
        /// <summary>
        /// The field value needs to include digits.
        /// </summary>
        IncludesDigits = 256,
        /// <summary>
        /// The field value needs to include symbols (non-alphabetic, non-numeric, non-space characters - includes punctuation characters).
        /// </summary>
        IncludesSymbol = 512,
        /// <summary>
        /// The field value can't include doubled substrings (e.g. "BB" or "ABAB").
        /// </summary>
        NoDoubles = 1024,

        /// <summary>
        /// Field value needs to be numeric (parse to a double) and non-empty.
        /// </summary>
        NonEmptyNumeric = 3,
        /// <summary>
        /// The field value needs to be a strong password -
        /// match minimum length requirement,
        /// include upper and lowercase characters,
        /// numbers,
        /// symbols
        /// and can't have doubled substrings.
        /// </summary>
        IsStrongPassword = MinLength | IncludesLowercase | IncludesUppercase | IncludesDigits | IncludesSymbol | NoDoubles,
    }
}