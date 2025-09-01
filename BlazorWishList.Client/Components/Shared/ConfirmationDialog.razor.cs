using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWishList.Client.Components.Shared
{
    public partial class ConfirmationDialog
    {
        #region " Inject "

        [Inject]
        public required IDialogService DialogService { get; set; }

        #endregion

        #region " Parameters "

        /// <summary>
        /// Dialog title parameter.
        /// </summary>
        [Parameter]
        public required string DialogTitle { get; set; }

        /// <summary>
        /// Dialog text parameter.
        /// </summary>
        [Parameter]
        public required string DialogText { get; set; }

        /// <summary>
        /// Dialog header class.
        /// </summary>
        [Parameter]
        public string? DialogHeaderClass { get; set; }

        /// <summary>
        /// Dialog content alert severity.
        /// </summary>
        [Parameter]
        public Severity AlertSeverity { get; set; } = Severity.Normal;

        /// <summary>
        /// OK button color.
        /// </summary>
        [Parameter]
        public Color OkButtonColor { get; set; } = Color.Primary;

        #endregion

        #region " Events "

        /// <summary>
        /// Save configuration changes.
        /// </summary>
        public void Confirm()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }

        #endregion
    }
}
