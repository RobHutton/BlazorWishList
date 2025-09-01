using Microsoft.AspNetCore.Identity;

namespace BlazorWishList.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Photo { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public List<WishListItem> WishListItems { get; set; } = [];
    public int Sort { get; set; } = 0;
}
