using BlogFodder.Core.Data;
using BlogFodder.Core.Email.Commands;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogFodder.Core.Membership.Handlers;

public class CreateUpdateUserHandler : IRequestHandler<CreateUpdateUserCommand, HandlerResult<User>>
{
    private readonly IMediator _mediator;
    private readonly BlogFodderDbContext _db;
    private readonly ProviderService _providerService;
    private readonly ILogger<CreateUpdateUserHandler> _logger;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly UserManager<User> _userManager;

    public CreateUpdateUserHandler(IMediator mediator, BlogFodderDbContext blogFodderDbContext,
        ProviderService providerService,
        ILogger<CreateUpdateUserHandler> logger, AuthenticationStateProvider authenticationStateProvider,
        UserManager<User> userManager)
    {
        _mediator = mediator;
        _db = blogFodderDbContext;
        _providerService = providerService;
        _logger = logger;
        _authenticationStateProvider = authenticationStateProvider;
        _userManager = userManager;
    }

    public async Task<HandlerResult<User>> Handle(CreateUpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Get the current user first via the authstate
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
        var handlerResult = new HandlerResult<User>();

        var efUser = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == authState.User.GetUserId(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (efUser == null)
        {
            // new users should only be created by the register page
            handlerResult.Success = false;
            handlerResult.AddMessage("Unable to create a new user, use the registration form", ResultMessageType.Error);
            return handlerResult;
        }

        // Profile Image - Need to save image and then create a file
        if (request.ProfileImageUpload != null)
        {
            // TODO - What if the user is changing the image and one already exists, need to delete

            // Save the file, create a gab file and attach it to the user
            var fileSaveResult = await _providerService.StorageProvider!
                .SaveFile(request.ProfileImageUpload, efUser.Id.ToString()).ConfigureAwait(false);
            if (!fileSaveResult.Success)
            {
                handlerResult.Success = fileSaveResult.Success;
                handlerResult.AddMessage(fileSaveResult.ErrorMessages, ResultMessageType.Error);
                return handlerResult;
            }

            // Create the gabfile
            var gabFile = await _providerService.StorageProvider.ToBlogFodderFile(fileSaveResult).ConfigureAwait(false);

            // Save the file first
            _db.Files.Add(gabFile);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Set the file to the user
            efUser.ProfileImage = gabFile;

            // Save the user
            handlerResult = await _db.CreateOrUpdate(efUser, handlerResult, false, cancellationToken)
                .ConfigureAwait(false);
            if (!handlerResult.Success)
            {
                return handlerResult;
            }
        }

        // Get user from user manager to update all this
        var managerUser = await _userManager.GetUserAsync(authState.User).ConfigureAwait(false);

        if (managerUser != null)
        {
            // Need to use user manager and then refresh signin.
            if (request.User.UserName != managerUser.UserName)
            {
                var userNameResult = await _userManager.SetUserNameAsync(managerUser, request.User.UserName)
                    .ConfigureAwait(false);
                if (userNameResult.Succeeded)
                {
                    handlerResult.RefreshSignIn = true;
                    handlerResult.AddMessage("Username updated successfully", ResultMessageType.Success);
                }
                else
                {
                    handlerResult.Success = userNameResult.Succeeded;
                    handlerResult.AddMessage(userNameResult.ToErrorsList(), ResultMessageType.Error);
                    userNameResult.LogErrors(_logger);
                    return handlerResult;
                }
            }

            // Need to use user manager and then refresh signin. So save other fields first
            if (request.User.Email != managerUser.Email)
            {
                // See if email needs to be confirmed
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var sendConfirmationEmailCommand = new SendEmailConfirmationCommand
                    {
                        User = managerUser,
                        NewEmailAddress = request.User.Email
                    };

                    // Send the email
                    await _mediator.Send(sendConfirmationEmailCommand, cancellationToken).ConfigureAwait(false);

                    // Save the new email in the users extended data
                    managerUser.ExtendedData.Add(Constants.ExtendedDataKeys.NewEmailAddress, request.User.Email!);

                    handlerResult.Success = true;
                    handlerResult.RefreshSignIn = true;
                    handlerResult.AddMessage("Please check your email and click the link to confirm your email address",
                        ResultMessageType.Info);
                }
                else
                {
                    // Just generate the code and change the email
                    var code = await _userManager.GenerateChangeEmailTokenAsync(managerUser, request.User.Email!)
                        .ConfigureAwait(false);
                    var emailResult =
                        await _userManager.ChangeEmailAsync(managerUser, request.User.Email!, code)
                            .ConfigureAwait(false);
                    if (emailResult.Succeeded)
                    {
                        handlerResult.Success = true;
                        handlerResult.RefreshSignIn = true;
                        handlerResult.AddMessage("Email address changed", ResultMessageType.Success);
                    }
                    else
                    {
                        handlerResult.Success = emailResult.Succeeded;
                        handlerResult.AddMessage(emailResult.ToErrorsList(), ResultMessageType.Error);
                        emailResult.LogErrors(_logger);
                        return handlerResult;
                    }
                }
            }

            // Password. Again need to use user manager
            if (!request.CurrentPassword.IsNullOrWhiteSpace() && !request.NewPassword.IsNullOrWhiteSpace())
            {
                var changePasswordResult = await _userManager
                    .ChangePasswordAsync(managerUser, request.CurrentPassword, request.NewPassword)
                    .ConfigureAwait(false);
                if (changePasswordResult.Succeeded)
                {
                    handlerResult.Success = true;
                    handlerResult.RefreshSignIn = true;
                    handlerResult.AddMessage("Password changed", ResultMessageType.Success);
                }
                else
                {
                    handlerResult.Success = changePasswordResult.Succeeded;
                    handlerResult.AddMessage(changePasswordResult.ToErrorsList(), ResultMessageType.Error);
                    changePasswordResult.LogErrors(_logger);
                    return handlerResult;
                }
            }

            // Messages to TempUiMessages if refresh sign in
            if (handlerResult.RefreshSignIn)
            {
                    managerUser.ExtendedData.RemoveTempUiMessages();
                managerUser.ExtendedData.SetTempUiMessage(handlerResult.Messages);
                var tempUiUpdateResult = await _userManager.UpdateAsync(managerUser).ConfigureAwait(false);
                if (!tempUiUpdateResult.Succeeded)
                {
                    tempUiUpdateResult.LogErrors(_logger);
                }
            }
        }
        else
        {
            handlerResult.Success = false;
            handlerResult.AddMessage("Unable to get the database user from the logged in user", ResultMessageType.Error);
        }


        return handlerResult;
    }
}