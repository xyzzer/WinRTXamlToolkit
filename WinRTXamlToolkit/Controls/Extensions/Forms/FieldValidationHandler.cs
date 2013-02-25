using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    public abstract class FieldValidationHandler<T>
        where T : Control
    {
        protected T Field;
        protected abstract string GetFieldValue();
        protected abstract int GetMaxLength();

        protected virtual int GetMinLength()
        {
            return FieldValidationExtensions.GetMinLength(Field);
        }

        internal void Validate()
        {
            var format = FieldValidationExtensions.GetFormat(Field);

            bool isEmpty;
            if (!ValidateNonEmpty(format, out isEmpty))
                return;

            if (!ValidateNumeric(format, isEmpty))
                return;

            if (!ValidateSpecificLength(format))
                return;

            if (!ValidateMinLength(format))
                return;

            if (!ValidateMatchesRegexPattern(format))
                return;

            if (!ValidateEqualsPattern(format))
                return;

            if (!ValidateIncludesLowercase(format))
                return;

            if (!ValidateIncludesUppercase(format))
                return;

            if (!ValidateIncludesDigits(format))
                return;

            if (!ValidateIncludesSymbols(format))
                return;

            if (!ValidateNoDoubles(format))
                return;

            MarkValid();
        }

        private bool ValidateNonEmpty(ValidationChecks format, out bool isEmpty)
        {
            var expectNonEmpty = (format & ValidationChecks.NonEmpty) != 0;
            isEmpty = string.IsNullOrWhiteSpace(GetFieldValue());

            if (expectNonEmpty && isEmpty)
            {
                MarkInvalid(FieldValidationExtensions.GetNonEmptyErrorMessage(Field));
                return false;
            }

            return true;
        }

        private bool ValidateNumeric(ValidationChecks format, bool isEmpty)
        {
            var expectNumber = (format & ValidationChecks.Numeric) != 0;

            if (expectNumber &&
                !isEmpty &&
                !IsNumeric())
            {
                MarkInvalid(FieldValidationExtensions.GetNumericErrorMessage(Field));
                return false;
            }

            return true;
        }

        private bool ValidateSpecificLength(ValidationChecks format)
        {
            var expectSpecificLength = (format & ValidationChecks.SpecificLength) != 0;

            if (expectSpecificLength &&
                GetMaxLength() > 0 &&
                GetMaxLength() != GetFieldValue().Length)
            {
                var messageFormat =
                    FieldValidationExtensions.GetSpecificLengthErrorMessage(Field) ??
                    "";
                var message =
                    string.Format(messageFormat, GetMaxLength());
                MarkInvalid(message);
                return false;
            }

            return true;
        }

        private bool ValidateMinLength(ValidationChecks format)
        {
            var expectMinLength = (format & ValidationChecks.MinLength) != 0;

            if (expectMinLength &&
                GetMinLength() > GetFieldValue().Length)
            {
                var messageFormat =
                    FieldValidationExtensions.GetMinLengthErrorMessage(Field) ??
                    "";
                var message =
                    string.Format(messageFormat, GetMinLength());
                MarkInvalid(message);
                return false;
            }

            return true;
        }

        private bool ValidateMatchesRegexPattern(ValidationChecks format)
        {
            var expectPattern = (format & ValidationChecks.MatchesRegexPattern) != 0;
            var pattern = FieldValidationExtensions.GetPattern(Field);
            if (expectPattern &&
                pattern != null &&
                !Regex.IsMatch(GetFieldValue(), pattern))
            {
                var messageFormat =
                    FieldValidationExtensions.GetPatternErrorMessage(Field) ??
                    "";
                var message =
                    string.Format(messageFormat, pattern);
                MarkInvalid(message);

                return false;
            }

            return true;
        }

        private bool ValidateEqualsPattern(ValidationChecks format)
        {
            var expectEquality = (format & ValidationChecks.EqualsPattern) != 0;
            var pattern = FieldValidationExtensions.GetPattern(Field);
            if (expectEquality &&
                pattern != null &&
                !GetFieldValue().Equals(pattern, StringComparison.Ordinal))
            {
                MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                return false;
            }

            return true;
        }

        private bool ValidateIncludesLowercase(ValidationChecks format)
        {
            var expectLowercase = (format & ValidationChecks.IncludesLowercase) != 0;

            if (expectLowercase)
            {
                var fieldValue = GetFieldValue();

                for (int i = 0; i < fieldValue.Length; i++)
                {
                    if (char.IsLower(fieldValue, i))
                        return true;
                }

                MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                return false;
            }

            return true;
        }

        private bool ValidateIncludesUppercase(ValidationChecks format)
        {
            var expectUppercase = (format & ValidationChecks.IncludesUppercase) != 0;

            if (expectUppercase)
            {
                var fieldValue = GetFieldValue();

                for (int i = 0; i < fieldValue.Length; i++)
                {
                    if (char.IsUpper(fieldValue, i))
                        return true;
                }

                MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                return false;
            }

            return true;
        }

        private bool ValidateIncludesDigits(ValidationChecks format)
        {
            var expectDigits = (format & ValidationChecks.IncludesDigits) != 0;

            if (expectDigits)
            {
                var fieldValue = GetFieldValue();

                for (int i = 0; i < fieldValue.Length; i++)
                {
                    if (char.IsDigit(fieldValue, i))
                        return true;
                }

                MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                return false;
            }

            return true;
        }

        private bool ValidateIncludesSymbols(ValidationChecks format)
        {
            var expectSymbols = (format & ValidationChecks.IncludesSymbol) != 0;

            if (expectSymbols)
            {
                var fieldValue = GetFieldValue();

                for (int i = 0; i < fieldValue.Length; i++)
                {
                    if (char.IsSymbol(fieldValue, i) ||
                        char.IsPunctuation(fieldValue, i))
                        return true;
                }

                MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                return false;
            }

            return true;
        }

        private bool ValidateNoDoubles(ValidationChecks format)
        {
            var expectNoDoubles = (format & ValidationChecks.IncludesSymbol) != 0;

            if (expectNoDoubles)
            {
                var fieldValue = GetFieldValue();

                for (int i = 0; i < fieldValue.Length; i++)
                {
                    for (int j = 1; i + j * 2 < fieldValue.Length; j++)
                    {
                        var isDouble = true;

                        for (int k = 0; k < j; k++)
                        {
                            if (fieldValue[i + k] !=
                                fieldValue[i + j + k])
                            {
                                isDouble = false;
                                break;
                            }
                        }

                        if (isDouble)
                        {
                            MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage(Field));

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsNumeric()
        {
            double number;
            return double.TryParse(GetFieldValue(), out number);
        }

        protected virtual void MarkValid()
        {
            var brush = FieldValidationExtensions.GetValidBrush(Field);
            Field.Background = brush;
            FieldValidationExtensions.SetIsValid(Field, true);
            FieldValidationExtensions.SetValidationMessage(Field, null);
            FieldValidationExtensions.SetValidationMessageVisibility(Field, Visibility.Collapsed);
        }

        protected virtual void MarkInvalid(string errorMessage)
        {
            var brush = FieldValidationExtensions.GetInvalidBrush(Field);
            Field.Background = brush;
            FieldValidationExtensions.SetIsValid(Field, false);
            FieldValidationExtensions.SetValidationMessage(Field, errorMessage);
            FieldValidationExtensions.SetValidationMessageVisibility(Field, Visibility.Visible);
        }
    }
}
