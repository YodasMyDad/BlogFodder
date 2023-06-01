﻿using BlogFodder.Core;
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
using MudBlazor;

namespace BlogFodder.App.Pages.Account.Manage
{
    [Authorize]
    public partial class UserProfile : ComponentBase
    {
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] public IMediator Mediator { get; set; }
        [Inject] public IDbContextFactory<BlogFodderDbContext> GabDbContext { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public CreateUpdateUserCommand UpdateProfileCommand { get; set; } = new();

        private User? CurrentUser { get; set; }
        private bool Loading { get; set; }
        private bool UserIsExternalLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            await using var context = await GabDbContext.CreateDbContextAsync();
            await SetCuUserCommand(authState.User.GetUserId(), context).ConfigureAwait(false);

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
                        Snackbar.Add(message.Message, Severity.Info);   
                    }
                }
            }

            Loading = false;
        }

        private async Task SetCuUserCommand(Guid userId, BlogFodderDbContext context)
        {
            CurrentUser = await context.Users.Include(x => x.ProfileImage).FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);
            if (CurrentUser != null)
            {
                var logins = context.UserLogins.AsNoTracking().Where(l => l.UserId == userId);
                UpdateProfileCommand.User = CurrentUser;
                UserIsExternalLogin = logins.Any();
            }
        }

        private async void HandleValidSubmit()
        {
            Loading = true;

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
                await using var context = await GabDbContext.CreateDbContextAsync();
                await SetCuUserCommand(CurrentUser.Id, context).ConfigureAwait(false);

                if (result.Messages.Count > 0)
                {
                    foreach (var resultMessage in result.Messages)
                    {
                        Snackbar.Add(resultMessage.Message, Severity.Info);   
                    }
                }
                else
                {
                    if (result.Success)
                    {
                        Snackbar.Add("Updated successfully", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add("There was an issue updating, please check the logs", Severity.Error);
                    }
                }
            }

            Loading = false;
            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }
    }
}