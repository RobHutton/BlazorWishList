using BlazorWishList.Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorWishList.Client.Services;
public class UserContextService : IUserContextService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUser? CurrentUser { get; private set; }

    public string? UserId => CurrentUser?.Id;
    public string? UserName => CurrentUser?.UserName;
    public string? UserPhoto => CurrentUser?.Photo;
    public bool IsAuthenticated => CurrentUser != null;

    public UserContextService(AuthenticationStateProvider authStateProvider, UserManager<ApplicationUser> userManager)
    {
        _authStateProvider = authStateProvider;
        _userManager = userManager;
    }

    public async Task InitializeAsync()
    {
        // Only initialize if not already
        if (CurrentUser != null) return;

        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated ?? false)
        {
            var dbUser = await _userManager.Users
                                           .AsNoTracking() // DETACH from DbContext
                                           .FirstOrDefaultAsync(u => u.UserName == user.Identity.Name);

            if (dbUser != null)
            {
                // Store a detached copy only
                CurrentUser = new ApplicationUser
                {
                    Id = dbUser.Id,
                    UserName = dbUser.UserName,
                    Photo = dbUser.Photo,
                    DateOfBirth = dbUser.DateOfBirth
                };
            }
        }
    }
}
