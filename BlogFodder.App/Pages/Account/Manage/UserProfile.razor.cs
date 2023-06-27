using Blazored.Toast.Services;
using BlogFodder.Core;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
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
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject] public IToastService ToastService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Parameter] public CreateUpdateUserCommand CreateUpdateUserCommand { get; set; } = new();
        
        [Inject] public IServiceProvider ServiceProvider { get; set; } = default!;

        private User? CurrentUser { get; set; }
        private bool Loading { get; set; }
        private bool UserIsExternalLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            
            await SetCuUserCommand(authState.User.GetUserId(), dbContext);

            // Check if this is a refresh and look for messages to display
            // This is shite, but it's a hack to get around RefreshSignInAsync
            bool refresh;
            NavigationManager.TryGetQueryString("refresh", out refresh);
            if (refresh)
            {
                var tempMessages = CurrentUser?.ExtendedData.GetTempUiMessages();
                var resultMessages = tempMessages as ResultMessage[] ?? tempMessages?.ToArray();
                if (resultMessages?.Any() == true)
                {
                    foreach (var message in resultMessages)
                    {
                        if (message.Message != null) ToastService.ShowInfo(message.Message);
                    }
                }
            }

            Loading = false;
        }

        private async Task SetCuUserCommand(Guid userId, BlogFodderDbContext context)
        {
            CurrentUser = await context.Users.Include(x => x.ProfileImage).FirstOrDefaultAsync(x => x.Id == userId);
            if (CurrentUser != null)
            {
                var logins = context.UserLogins.AsNoTracking().Where(l => l.UserId == userId);
                CreateUpdateUserCommand.User = CurrentUser;
                UserIsExternalLogin = logins.Any();
            }
        }

        private async void HandleValidSubmit()
        {
            Loading = true;

            // Set email if external login as it's not shown on the page
            if (UserIsExternalLogin)
            {
                CreateUpdateUserCommand.User.Email = CurrentUser.Email;
            }

            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var result = await mediatr.Send(CreateUpdateUserCommand);

            // Yes this is really shit for
            // Cannot refresh RefreshSignInAsync in Blazor, so have to redirect to a razor page and then 
            // straight back.
            if (result.RefreshSignIn)
            {
                NavigationManager.NavigateTo(Constants.Urls.Account.RefreshSignIn, true);
            }
            else
            {
                await SetCuUserCommand(CurrentUser.Id, dbContext);

                if (result.Messages.Count > 0)
                {
                    foreach (var resultMessage in result.Messages)
                    {
                        if (resultMessage.Message != null) ToastService.ShowInfo(resultMessage.Message);
                    }
                }
                else
                {
                    if (result.Success)
                    {
                        ToastService.ShowSuccess("Updated successfully");
                    }
                    else
                    {
                        ToastService.ShowError("There was an issue updating, please check the logs");
                    }
                }
            }

            Loading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}