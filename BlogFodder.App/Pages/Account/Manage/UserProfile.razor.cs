/*using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlogFodder.App.Pages.Account.Manage
{
    [Authorize]
    public partial class UserProfile : ComponentBase
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] public IHeadElementHelper HeadElementHelper { get; set; }
        [Inject] public IMediator Mediator { get; set; }
        [Inject] public IDbContextFactory<BlogFodderDbContext> GabDbContext { get; set; }
        [Inject] public IMapper Mapper { get; set; }
        [Inject] public IUiNotificationService NotificationService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Parameter] public CreateUpdateUserCommand UpdateProfileCommand { get; set; } = new CreateUpdateUserCommand();

        private User? CurrentUser { get; set; }
        private bool Loading { get; set; }
        private bool UserIsExternalLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            await HeadElementHelper.SetTitleAsync("Edit Profile").ConfigureAwait(false);
            await using var context = await GabDbContext.CreateDbContextAsync();
            await SetCuUserCommand(authState.User.GetUserId(), context).ConfigureAwait(false);

            // Check if this is a refresh and look for messages to display
            // This is shite, but it's a hack to get around RefreshSignInAsync
            bool refresh;
            NavigationManager.TryGetQueryString("refresh", out refresh);
            if (refresh)
            {
                var tempUimessages = CurrentUser.ExtendedData.GetTempUiMessages();
                if (tempUimessages?.Any() == true)
                {
                    NotificationService.ShowNotification(tempUimessages);
                }
            }

            Loading = false;
        }

        private async Task SetCuUserCommand(Guid userId, BlogFodderDbContext context)
        {
            CurrentUser = await context.Users.Include(x => x.ProfileImage).FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);
            if (CurrentUser != null)
            {
                UpdateProfileCommand = CurrentUser.ToCuCommand(Mapper);
                UserIsExternalLogin = CurrentUser.ExtendedData.Get<bool>(Constants.ExtendedDataKeys.IsExternalLogin);
            }
        }

        private async void HandleValidSubmit()
        {
            Loading = true;

            // Set the id as it will be missing and we don't want the user fiddling it via a hidden form field
            UpdateProfileCommand.User.Id = CurrentUser.Id;

            // Set email if external login as it's not shown on the page
            if (UserIsExternalLogin)
            {
                UpdateProfileCommand.User.Email = CurrentUser.Email;
            }

            var result = await Mediator.Send(UpdateProfileCommand).ConfigureAwait(false);

            // Yes this is really shit for
            // Cannot refresh RefreshSignInAsync in Blazor, so have to redirect to a razor page and then 
            // straight back.
            if (result.RefreshSignIn)
            {
                NavigationManager.NavigateTo(Constants.Urls.Account.RefreshSignIn, true);
            }
            else
            {
                using var context = GabDbContext.CreateDbContext();
                await SetCuUserCommand(CurrentUser.Id, context).ConfigureAwait(false);

                if (result.Messages.Count > 0)
                {
                    NotificationService.ShowNotification(result.Messages);
                }
                else
                {
                    if (result.Success)
                    {
                        NotificationService.ShowSuccess("Updated successfully");
                    }
                    else
                    {
                        NotificationService.ShowError("There was an issue updating, please check the logs");
                    }
                }
            }

            Loading = false;
            await InvokeAsync(() => StateHasChanged()).ConfigureAwait(false);
        }
    }
}*/