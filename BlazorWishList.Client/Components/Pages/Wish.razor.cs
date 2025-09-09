using Mapster;
using Microsoft.JSInterop;

namespace BlazorWishList.Client.Components.Pages;

public partial class Wish
{
    #region " Inject "

    [Inject]
    public IMobileDetectionService MobileDetector { get; set; } = null!;

    [Inject]
    public IWishListClientService WishListClientService { get; set; } = null!;

    [Inject]
    ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    #endregion

    #region Parameters

    /// <summary>
    /// Wish List Item Id
    /// </summary>
    [Parameter]
    public int Id { get; set; }

    #endregion Parameters

    #region " Variables "

    /// <summary>
    /// Wisher Id querystring parameter.
    /// </summary>
    private string WisherId { get; set; } = string.Empty;

    /// <summary>
    /// Wish List data.
    /// </summary>
    private WishListRequest MyWish { get; set; } = default!;

    /// <summary>
    /// Original Wish List data.
    /// </summary>
    private WishListRequest OriginalWish { get; set; } = default!;

    /// <summary>
    /// Is saved flag.
    /// </summary>
    private bool _isSaved;
    private bool IsFading;

    private bool IsSaved
    {
        get => _isSaved;
        set
        {
            if (value)
            {
                _isSaved = true;
                IsFading = false;
                _ = ResetFlagWithFade();
            }
            else
            {
                _isSaved = false;
            }
        }
    }

    /// <summary>
    /// Unsaved changes flag.
    /// </summary>
    private bool UnsavedChanges => !JsonHelper.IsObjectMatch(OriginalWish, MyWish) && !IsSaved;

    /// <summary>
    /// Is loading flag.
    /// </summary>
    private bool IsLoading { get; set; }

    /// <summary>
    /// Is read only flag.
    /// </summary>
    private bool IsReadOnly { get; set; }

    /// <summary>
    /// Back button context.
    /// </summary>
    private string Referrer => "Wish List Items";
    private string ReferrerLink => $"/Home?u={WisherId}";
    private string ReferrerTitle => !string.IsNullOrWhiteSpace(Referrer) ? $"Back To {Referrer}" : string.Empty;

    /// <summary>
    /// Default configuration insert flag.
    /// </summary>
    private bool IsInsert => Id <= 0;

    /// <summary>
    /// Page title.
    /// </summary>
    private string PageTitle => IsInsert ? "Create Wish List Item" : "Update Wish List Item";

    #endregion

    #region " Lifecycle Methods "

    /// <summary>
    /// Initializes the component by loading data asynchronously.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Wisher Id
        WisherId = QueryHelper.GetQueryValueFromUri(Navigation.Uri, "w");

        // IsSaved ("1" = true, "0" = false, case insensitive)
        var savedString = QueryHelper.GetQueryValueFromUri(Navigation.Uri, "s");
        IsSaved = savedString == "1" ||
                  savedString?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;

        await LoadDataAsync();

        await base.OnInitializedAsync();
    }

    #endregion

    #region " Events "

    /// <summary>
    /// Wish List item save event.
    /// </summary>
    private async Task<bool> OnSaveAsync(int id, WishListRequest wishListItem)
    {
        IsLoading = true;

        string action = id > 0 ? "updated" : "inserted";
        Result<WishListResponse> result;

        if (id > 0)
        {
            result = await WishListClientService.UpdateWishListItemAsync(WisherId, id, wishListItem);
        }
        else
        {
            result = await WishListClientService.CreateWishListItemAsync(WisherId, wishListItem);
        }

        if (result.IsSuccess)
        {
            MyWish = result.Value.Adapt<WishListRequest>();
            OriginalWish = JsonHelper.Clone(MyWish);
            Snackbar.Add($"Successfully {action} Wish List item.", Severity.Success);
            IsSaved = true;
            Back();
            return true;
        }
        else
        {
            string err = $"Failed to {action} Wish List item.";
            Snackbar.Add(result.Error ?? err, Severity.Error);
            return false;
        }
    }

    #endregion

    #region " Methods "

    private async Task ResetFlagWithFade()
    {
        await Task.Delay(2000); // wait before fading
        IsFading = true;
        StateHasChanged();

        await Task.Delay(2000); // allow fade transition
        _isSaved = false;
        IsFading = false;
        StateHasChanged();
    }

    /// <summary>
    /// Handles back navigation.
    /// </summary>
    private void Back()
    {

        Navigation.NavigateTo(ReferrerLink);
    }

    /// <summary>
    /// Loads the golden setup.
    /// </summary>
    private async Task LoadDataAsync()
    {
        IsLoading = true;
        WishListResponse response = new();

        if (!IsInsert)
        {
            Result<WishListResponse> result = await WishListClientService.GetWishListItemAsync(WisherId, Id);
            if (result.IsSuccess)
            {
                response = result.Value;
                if (string.IsNullOrWhiteSpace(WisherId))
                {
                    WisherId = MyWish.WisherId;
                }
            }
            else
            {
                Snackbar.Add("Failed to load Wish List Item. Return to List.", Severity.Error);
                Back();
            }
        }
        else if (string.IsNullOrWhiteSpace(WisherId))
        {
            Snackbar.Add("Failed to determine Wish List Recipient. Return to List.", Severity.Error);
            Back();
        }

        MyWish = response.Adapt<WishListRequest>();
        OriginalWish = JsonHelper.Clone(MyWish);

        IsLoading = false;
    }

    private void OpenLink()
    {
        string _link = MyWish.Url ?? string.Empty;
        if (string.IsNullOrWhiteSpace(_link))
        {
            Snackbar.Add("Please paste a valid link to your Wish List item.", Severity.Error);
        }

        // Ensure the link starts with http/https
        var url = _link.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? _link
            : $"https://{_link}";

        // Open the link in a new browser tab
        _ = JS.InvokeVoidAsync("open", url, "_blank");
    }

    #endregion
}
