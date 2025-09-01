using BlazorWishList.Client.Components.Account.Models;
using BlazorWishList.Client.Helpers;
using BlazorWishList.Domain.Entities;
using BlazorWishList.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace BlazorWishList.Client.Components.Account.Pages
{
    public partial class Login2
    {
        #region " Injects "

        [Inject]
        public required SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        public required ILogger<Login> Logger { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [Inject]
        public required IdentityRedirectManager RedirectManager { get; set; }

        #endregion

        #region " Parameters "

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        #endregion

        #region " Variables "

        private UserLogin LoginInfo { get; set; } = new();
        private List<FormValidationError> ValidationErrors { get; set; } = [];
        private string? errorMessage;

        #endregion

        #region " LifeCycle Events "

        protected override async Task OnInitializedAsync()
        {
            // Clear the existing external cookie to ensure a clean login process
            if (HttpMethods.IsGet(HttpContext.Request.Method))
            {
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
        }

        #endregion

        #region " Methods "

        private bool Validate()
        {
            ResetValidationErrors();

            ValidationHelper.ValidateField("Email", LoginInfo.Email, null, null, ValidationErrors);
            ValidationHelper.ValidateField("Password", LoginInfo.Password, null, null, ValidationErrors);

            return ValidationErrors.Count == 0;
        }

        private void ResetValidationErrors()
        {
            ValidationErrors = [];
        }

        private bool IsValidationError(string fieldName) => ValidationErrors.Any(e => e.FieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

        private string ValidationErrorMessage(string fieldName) => ValidationErrors.Find(e => e.FieldName == fieldName)?.ErrorMessage ?? string.Empty;

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        public async Task LoginUser()
        {
            Debugger.Break();
            if (Validate())
            {
                SignInResult result = await SignInManager.PasswordSignInAsync(LoginInfo.Email, LoginInfo.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Logger.LogInformation($"User logged in: {LoginInfo.Email}");
                    RedirectManager.RedirectTo("/");
                }
                else if (result.RequiresTwoFactor)
                {
                    RedirectManager.RedirectTo(
                        "Account/LoginWith2fa",
                        new() { ["returnUrl"] = "/", ["rememberMe"] = false });
                }
                else if (result.IsLockedOut)
                {
                    Logger.LogWarning("User account locked out.");
                    RedirectManager.RedirectTo("Account/Lockout");
                }
                else
                {
                    errorMessage = "Error: Invalid login attempt.";
                }
            }
        }

        #endregion
    }
}
