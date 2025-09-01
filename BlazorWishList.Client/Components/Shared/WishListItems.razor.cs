using BlazorWishList.Domain.Entities;
using BlazorWishList.Service;

namespace BlazorWishList.Client.Components.Shared
{
    public partial class WishListItems : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
    {
        #region Inject

        [Inject]
        public required ISnackbar Snackbar { get; set; }

        [Inject]
        public required NavigationManager Navigation { get; set; }

        [Inject]
        public required IUserContextService UserContextService { get; set; }

        [Inject]
        public required IWishListService WishListService { get; set; }

        #endregion Inject

        #region Parameters

        [Parameter]
        public required string WisherId { get; set; }

        #endregion Parameters

        #region Variables

        private ApplicationUser CurrentUser { get; set; } = new();
        private bool IsActive { get; set; } = true;
        private bool IsUnpurchased { get; set; } = true;

        private MudTable<WishListResponse>? tableRef;

        public List<WishListResponse> WishListData { get; set; } = [];
        private List<WishListResponse> PagedData = [];

        private int totalItems;
        private bool isLoading;
        private string errorMessage = string.Empty;
        private string searchText { get; set; } = string.Empty;

        /// <summary>
        /// Description of active flag state.
        /// </summary>
        private string activeText => IsActive ? "Active Entries" : "Inactive Entries";
        private Color activeColor => IsActive ? Color.Primary : Color.Error;
        private string activeCss => IsActive ? "mud-switch-active" : "mud-switch-inactive";

        /// <summary>
        /// Description of purchase flag state.
        /// </summary>
        private string purchasedText => IsUnpurchased ? "Available for Purchase" : "Already Purchased";
        private Color purchaseColor => IsUnpurchased ? Color.Primary : Color.Error;
        private string purchaseCss => IsUnpurchased ? "mud-switch-inactive" : "mud-switch-active";

        #endregion Variables

        #region LifeCycle
        protected override async Task OnInitializedAsync()
        {
            CurrentUser = UserContextService.CurrentUser ?? CurrentUser;

            await LoadDataAsync();

            await base.OnInitializedAsync();

            //if (!WishListClientService.WishListItems.Any() || WisherId != WishListClientService.SelectedWisherId)
            //{
            //    await WishListClientService.GetWishListItemsAsync(WisherId, IsPurchased);
            //    //WishListClientService.OnWishListChange += StateHasChanged;
            //}
        }

        public void Dispose()
        {
            //WishListClientService.OnWishListChange -= StateHasChanged;
        }

        #endregion LifeCycle

        #region Events

        private void OnAdd()
        {
            Navigation.NavigateTo($"/Wish?w={WisherId}");
        }

        private void OnEdit(WishListResponse item)
        {
            if (item.Id <= 0)
            {
                Snackbar.Add("Failed to determine Wish List item to edit.", Severity.Error);
                return;
            }

            Navigation.NavigateTo($"/Wish/{item.Id}?w={WisherId}", true);
        }

        private async Task OnDelete(WishListResponse item)
        {
            if (item.Id <= 0)
            {
                Snackbar.Add("Failed to determine Wish List item to remove.", Severity.Error);
                return;
            }

            if (item.IsDeleted)
            {
                Snackbar.Add("Wish List item has already been removed.", Severity.Warning);
                return;
            }

            Result<bool> result = await WishListService.DeleteWishListItemAsync(WisherId, item.Id);
            if (result.IsSuccess)
            {
                await RefreshTable();
                Snackbar.Add("Successfully removed Wish List item.", Severity.Success);
            }
            else
            {
                string err = "Failed to remove Wish List item.";
                Snackbar.Add(result.Error ?? err, Severity.Error);
            }
        }

        private async Task OnReactivate(WishListResponse item)
        {
            if (item.Id <= 0)
            {
                Snackbar.Add("Failed to determine Wish List item to reactivate.", Severity.Error);
                return;
            }

            if (!item.IsDeleted)
            {
                Snackbar.Add("Wish List item has already been reactivated.", Severity.Warning);
                return;
            }

            Result<bool> result = await WishListService.UnDeleteWishListItemAsync(WisherId, item.Id);
            if (result.IsSuccess)
            {
                await RefreshTable();
                Snackbar.Add("Successfully reactivated Wish List item.", Severity.Success);
            }
            else
            {
                string err = "Failed to reactivate Wish List item.";
                Snackbar.Add(result.Error ?? err, Severity.Error);
            }
        }

        private async Task OnPurchase(WishListResponse item)
        {
            if (item.Id <= 0)
            {
                Snackbar.Add("Failed to determine Wish List item to mark as purchased.", Severity.Error);
                return;
            }

            if (item.IsPurchased)
            {
                Snackbar.Add("Wish List item has already been purchased.", Severity.Warning);
                return;
            }

            Result<WishListResponse> result = await WishListService.PurchaseWishListItemAsync(WisherId, item.Id, CurrentUser.Id);
            if (result.IsSuccess)
            {
                await RefreshTable();
                Snackbar.Add("Successfully marked Wish List item as purchased by you.", Severity.Success);
            }
            else
            {
                string err = "Failed to mark Wish List item as purchased by you.";
                Snackbar.Add(result.Error ?? err, Severity.Error);
            }
        }

        private async Task OnUnpurchase(WishListResponse item)
        {
            if (item.Id <= 0)
            {
                Snackbar.Add("Failed to determine Wish List item to mark as unpurchased.", Severity.Error);
                return;
            }

            if (!item.IsPurchased)
            {
                Snackbar.Add("Wish List item is already marked as not purchased.", Severity.Warning);
                return;
            }

            Result<WishListResponse> result = await WishListService.UnPurchaseWishListItemAsync(WisherId, item.Id, CurrentUser.Id);
            if (result.IsSuccess)
            {
                await RefreshTable();
                Snackbar.Add("Successfully marked Wish List item as not purchased.", Severity.Success);
            }
            else
            {
                string err = "Failed to mark Wish List item as not purchased.";
                Snackbar.Add(result.Error ?? err, Severity.Error);
            }
        }

        private async Task OnActiveChanged(bool value)
        {
            IsActive = value;
            await RefreshTable();
        }

        private async Task OnPurchaseChanged(bool value)
        {
            IsUnpurchased = value;
            await RefreshTable();
        }

        private async Task RefreshTable()
        {
            await LoadDataAsync();
            tableRef?.ReloadServerData();
        }

        #endregion Events

        #region Methods

        private async Task LoadDataAsync()
        {
            isLoading = true;
            StateHasChanged();

            var result = await WishListService.GetWishListItemsAsync(CurrentUser.Id, WisherId, !IsUnpurchased, IsActive);
            if (result.IsSuccess)
            {
                WishListData = result.Value ?? [];
            }
            else
            {
                string err = "Failed to fetch the Wish List items.";
                WishListData = [];
                errorMessage = err;
                Snackbar.Add(result.Error ?? err, Severity.Error);
            }

            isLoading = false;
            StateHasChanged();
        }

        private void OnSearch(string text)
        {
            searchText = text;
            tableRef?.ReloadServerData();
        }

        /// <summary>
        /// Getting the paged, filtered and ordered data.
        /// </summary>
        private async Task<TableData<WishListResponse>> ServerReload(TableState state, CancellationToken token)
        {
            if (WishListData is not null)
            {
                IEnumerable<WishListResponse> data = WishListData.Where(item =>
                {
                    if (string.IsNullOrWhiteSpace(searchText))
                        return true;
                    if (item.PriceRange.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (item.Category.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (item.Merchant.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (item.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (item.Size.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (item.Color.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                });

                switch (state.SortLabel)
                {
                    case "PriceRange":
                        data = data.OrderByDirection(state.SortDirection, o => o.PriceRange);
                        break;
                    case "Category":
                        data = data.OrderByDirection(state.SortDirection, o => o.Category);
                        break;
                    case "Merchant":
                        data = data.OrderByDirection(state.SortDirection, o => o.Merchant);
                        break;
                    case "Description":
                        data = data.OrderByDirection(state.SortDirection, o => o.Description);
                        break;
                    case "Size":
                        data = data.OrderByDirection(state.SortDirection, o => o.Size);
                        break;
                    case "Color":
                        data = data.OrderByDirection(state.SortDirection, o => o.Color);
                        break;
                }

                totalItems = data.Count();
                PagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
            }

            await Task.Delay(5);
            return new TableData<WishListResponse>() { TotalItems = totalItems, Items = PagedData };
        }

        #endregion Methods
    }
}
