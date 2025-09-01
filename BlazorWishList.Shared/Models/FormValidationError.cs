namespace BlazorWishList.Domain.Models
{
    public class FormValidationError
    {
        public string FieldName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
