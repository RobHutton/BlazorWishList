using BlazorWishList.Domain.Models;

namespace BlazorWishList.Client.Helpers
{
    public static class ValidationHelper
    {
        public static void AddValidationError(string fieldName, string errorMessage, List<FormValidationError> validation)
        {
            validation.Add(new FormValidationError
            {
                FieldName = fieldName,
                ErrorMessage = errorMessage
            });
        }

        public static void ValidateField(string fieldName, string? fieldValue, int? maxLength, int? minLength, List<FormValidationError> validation)
        {
            string? trimmed = fieldValue?.Trim();
            string? errorMessage = null;

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                errorMessage = "Required";
            }
            else if (maxLength.HasValue && trimmed!.Length > maxLength.Value)
            {
                errorMessage ??= $"Max Length {maxLength.Value}";
            }
            else if (minLength.HasValue && trimmed!.Length < minLength.Value)
            {
                errorMessage ??= $"Min Length {minLength.Value}";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                AddValidationError(fieldName, errorMessage, validation);
            }
        }

        public static void ValidateField(string fieldName, int? fieldValue, bool allowZero, int? maxValue, int? minValue, List<FormValidationError> validation)
        {
            string? errorMessage = null;

            if (!fieldValue.HasValue || (fieldValue.Value == 0 && !allowZero))
            {
                errorMessage = "Required";
            }
            else if (maxValue.HasValue && fieldValue.Value > maxValue.Value)
            {
                errorMessage ??= $"Max Value {maxValue.Value}";
            }
            else if (minValue.HasValue && fieldValue.Value < minValue.Value)
            {
                errorMessage ??= $"Min Value {minValue.Value}";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                AddValidationError(fieldName, errorMessage, validation);
            }
        }

    }
}
