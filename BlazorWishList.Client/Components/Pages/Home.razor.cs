namespace BlazorWishList.Client.Components.Pages
{
    public partial class Home : ComponentBase, IDisposable
    {
        #region Inject

        [Inject]
        public IMobileDetectionService MobileDetector { get; set; } = null!;

        [Inject]
        public IUserContextService UserContextService { get; set; } = null!;

        [Inject]
        public IWishListClientService WishListClientService { get; set; } = null!;

        [Inject]
        ISnackbar Snackbar { get; set; } = default!;

        [Inject]
        public required NavigationManager Navigation { get; set; }

        #endregion Inject

        #region "Variables"

        private bool IsLoading { get; set; } = false;
        private string UserName { get; set; } = string.Empty;
        private string ExpandedUserId { get; set; } = string.Empty;

        #endregion

        #region LifeCycle

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            StateHasChanged();

            if (WishListClientService.WishListUsers.Count == 0)
            {
                await WishListClientService.GetUsersWithCountAsync();
            }

            ExpandedUserId = QueryHelper.GetQueryValueFromUri(Navigation.Uri, "u");

            await Task.Delay(50);
            UserName = UserContextService?.UserName ?? UserName;

            IsLoading = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            //WishListClientService.OnUserCountChange -= StateHasChanged;
        }

        #endregion LifeCycle

        #region Events

        private void OnExpandedChanged(bool expanded, string userId)
        {
            if (expanded)
            {
                ExpandedUserId = userId;
            }
            else if (ExpandedUserId == userId)
            {
                ExpandedUserId = string.Empty;
            }
        }

        #endregion Events
    }
}
