using BlazorWishList.Service;

namespace BlazorWishList.Client.Services;

public class WishListClientService(IWishListService wishListService) : IWishListClientService
{
    private readonly IWishListService _wishListService = wishListService;
    public string ErrorMessage { get; set; } = string.Empty;
    public string UserErrorMessage { get; set; } = string.Empty;
    public bool IsLoading { get; set; } = false;

    private string _selectedWisherId = string.Empty;
    public string SelectedWisherId => _selectedWisherId;

    private bool _isPurchased;
    public bool IsPurchased => _isPurchased;

    public List<WishListResponse> WishListItems { get; private set; } = [];
    public List<UserWishListCount> WishListUsers { get; private set; } = [];

    public event Action? OnWishListChange;
    public event Action? OnUserCountChange;

    // Initialization or refresh of filter
    //public async Task InitializeAsync(string wisherId, bool isPurchased)
    //{
    //    //await RefreshUserCount();
    //    //if (wisherId != string.Empty)
    //    //{
    //    //    _selectedWisherId = wisherId;
    //    //    _isPurchased = isPurchased;
    //    //    await RefreshWishListAsync();
    //    //}
    //}

    // UI-triggered filtering
    //public async Task SetFilterAsync(string wisherId, bool isPurchased)
    //{
    //    if (_selectedWisherId != wisherId || _isPurchased != isPurchased)
    //    {
    //        _selectedWisherId = wisherId;
    //        _isPurchased = isPurchased;
    //        await RefreshWishListAsync();
    //    }
    //}
    //public async Task<Result<List<UserWishListCount>>> GetUsersWithWishListInfoAsync()
    //{
    //    return await _wishListService.GetUsersWithWishListInfoAsync();
    //}
    public async Task<Result<WishListResponse>> CreateWishListItemAsync(string wisherId, WishListRequest createItem)
    {
        var response = await _wishListService.CreateWishListItemAsync(wisherId, createItem);
        //await RefreshWishListAsync();
        return response;
    }

    public async Task<Result<bool>> DeleteWishListItemAsync(string wisherId, int itemId)
    {
        var response = await _wishListService.DeleteWishListItemAsync(wisherId, itemId);
        //await RefreshWishListAsync();
        return response;
    }

    public async Task<Result<bool>> UnDeleteWishListItemAsync(string wisherId, int itemId)
    {
        var response = await _wishListService.UnDeleteWishListItemAsync(wisherId, itemId);
        //await RefreshWishListAsync();
        return response;
    }

    public async Task<Result<WishListResponse>> GetWishListItemAsync(string wisherId, int itemId)
    {
        if (string.IsNullOrWhiteSpace(wisherId))
            return Result<WishListResponse>.Fail("Failed to determine Wish Recipient.");

        return await _wishListService.GetWishListItemAsync(wisherId, itemId);
    }

    public async Task<Result<WishListResponse>> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
    {
        if (string.IsNullOrWhiteSpace(wisherId))
            return Result<WishListResponse>.Fail("Failed to determine Wish Recipient.");

        var response = await _wishListService.PurchaseWishListItemAsync(wisherId, itemId, purchaserId);
        //await RefreshWishListAsync();
        return response;
    }
    public async Task<Result<WishListResponse>> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId)
    {
        if (string.IsNullOrWhiteSpace(wisherId))
            return Result<WishListResponse>.Fail("Failed to determine Wish Recipient.");

        var response = await _wishListService.UnPurchaseWishListItemAsync(wisherId, itemId, purchaserId);
        //await RefreshWishListAsync();
        return response;
    }

    public async Task<Result<WishListResponse>> UpdateWishListItemAsync(string wisherId, int itemId, WishListRequest updateItem)
    {
        if (string.IsNullOrWhiteSpace(wisherId))
            return Result<WishListResponse>.Fail("Failed to determine Wish Recipient.");

        var response = await _wishListService.UpdateWishListItemAsync(wisherId, itemId, updateItem);
        //await RefreshWishListAsync();
        return response;
    }

    public async Task GetUsersWithCountAsync()
    {
        IsLoading = true;
        var userResult = await _wishListService.GetUsersWithWishListInfoAsync();
        if (userResult.IsSuccess)
        {
            WishListUsers = userResult.Value!;
        }
        else
        {
            UserErrorMessage = userResult.Error ?? Errors.GetUserWishListCount;
        }
        IsLoading = false;
        OnUserCountChange?.Invoke();
    }
    public async Task GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive)
    {
        if (!string.IsNullOrWhiteSpace(_selectedWisherId))
        {
            _selectedWisherId = wisherId;
            _isPurchased = isPurchased;
            IsLoading = true;
            WishListItems.Clear();
            var result = await _wishListService.GetWishListItemsAsync(userId, _selectedWisherId, _isPurchased, isActive);
            if (result.IsSuccess)
            {
                WishListItems.AddRange(result.Value!);
            }
            else
            {
                ErrorMessage = result.Error ?? Errors.GetWishListItems;
            }
            IsLoading = false;
            OnWishListChange?.Invoke();
        }
    }
    public async Task RefreshWishListAsync()
    {
        await Task.Delay(1);
        return;
    }
}

