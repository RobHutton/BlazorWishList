namespace BlazorWishList.Client.Services;

public interface IWishListClientService
{
    string ErrorMessage { get; set; }
    string UserErrorMessage { get; set; }
    bool IsLoading { get; set; }

    string SelectedWisherId { get; }
    bool IsPurchased { get; }

    List<WishListResponse> WishListItems { get; }
    List<UserWishListCount> WishListUsers { get; }

    event Action? OnWishListChange;
    event Action? OnUserCountChange;

    // Initialization or filter update
    //Task InitializeAsync(string wisherId, bool isPurchased);
    //Task SetFilterAsync(string wisherId, bool isPurchased);

    Task GetUsersWithCountAsync();
    Task GetWishListItemsAsync(string userId, string wisherId, bool isPurchased, bool isActive);

    // CRUD Operations
    Task<Result<WishListResponse>> CreateWishListItemAsync(string wisherId, WishListRequest createItem);
    Task<Result<bool>> DeleteWishListItemAsync(string wisherId, int itemId);
    Task<Result<bool>> UnDeleteWishListItemAsync(string wisherId, int itemId);
    Task<Result<WishListResponse>> GetWishListItemAsync(string wisherId, int itemId);
    Task<Result<WishListResponse>> UpdateWishListItemAsync(string wisherId, int itemId, WishListRequest updateItem);
    Task<Result<WishListResponse>> PurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    Task<Result<WishListResponse>> UnPurchaseWishListItemAsync(string wisherId, int itemId, string purchaserId);
    Task RefreshWishListAsync();
}

