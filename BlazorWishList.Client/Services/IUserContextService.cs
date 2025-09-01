using BlazorWishList.Domain.Entities;

namespace BlazorWishList.Client.Services
{
    public interface IUserContextService
    {
        ApplicationUser? CurrentUser { get; }
        string? UserPhoto { get; }
        string? UserId { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }

        Task InitializeAsync();
    }
}
