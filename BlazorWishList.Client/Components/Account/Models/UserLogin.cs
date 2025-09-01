using System.ComponentModel.DataAnnotations;

namespace BlazorWishList.Client.Components.Account.Models
{
    public sealed class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
