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

        // Updating the user using UserManager and a EF context is a bit fiddly
        // Do all the usermanager stuff after the EF context stuff
        // This does mean there will be duplicate calls from the different contexts

        // -- Start EF Context stuff -- //

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

        // TODO - Think of a better way?
        // We store these here as we don't want to map the new ones and do an update via EF because we need to refresh the cookie
        // and Username, Email and Phonenumber changes need to be done via the UserManager
        var phoneNumber = request.User.PhoneNumber;
        var email = request.User.Email;
        var username = request.User.UserName;

        request.User.PhoneNumber = efUser.PhoneNumber;
        request.User.Email = efUser.Email;
        request.User.UserName = efUser.UserName;

        // Profile Image - Need to save image and then create a gabfile
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

            // Set the user
            //gabFile.CreatedBy = efUser;

            // Save the file first
            _db.Files.Add(gabFile);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Set the file to the user
            efUser.ProfileImage = gabFile;
        }

        //TODO - Map other standard fields that need updating - What is this?
        //efUser = request.ToUser(efUser, _mapper);

        // Save the user
        handlerResult = await _db.CreateOrUpdate(efUser, handlerResult, false, cancellationToken).ConfigureAwait(false);
        if (!handlerResult.Success)
        {
            return handlerResult;
        }

        // -- Start Usermanager stuff -- //

        // Get user from Usermanager to update all this
        var managerUser = await _userManager.GetUserAsync(authState.User).ConfigureAwait(false);

        // Need to use usermanager and then refresh signin.
        if (username != managerUser.UserName)
        {
            var userNameResult = await _userManager.SetUserNameAsync(managerUser, username).ConfigureAwait(false);
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

        // Do not allow password or email to be changed if this is an external login
        if (!managerUser.ExtendedData.Get<bool>(Constants.ExtendedDataKeys.IsExternalLogin))
        {
            // Need to use usermanager and then refresh signin. So save other fields first
            if (email != managerUser.Email)
            {
                // See if email needs to be confirmed
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var sendConfirmationEmailCommand = new SendEmailConfirmationCommand
                    {
                        User = managerUser,
                        NewEmailAddress = email
                    };

                    // Send the email
                    await _mediator.Send(sendConfirmationEmailCommand, cancellationToken).ConfigureAwait(false);

                    // Save the new email in the users extended data
                    managerUser.ExtendedData.Add(Constants.ExtendedDataKeys.NewEmailAddress, email);

                    handlerResult.Success = true;
                    handlerResult.RefreshSignIn = true;
                    handlerResult.AddMessage("Please check your email and click the link to confirm your email address",
                        ResultMessageType.Info);
                }
                else
                {
                    // Just generate the code and change the email
                    var code = await _userManager.GenerateChangeEmailTokenAsync(managerUser, email)
                        .ConfigureAwait(false);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    var emailResult =
                        await _userManager.ChangeEmailAsync(managerUser, email, code).ConfigureAwait(false);
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

            // Password. Again need to use usermanager
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
        }

        // Phonenumber - Need to use usermanager
        if (!phoneNumber.IsNullOrWhiteSpace() &&
            phoneNumber != managerUser.PhoneNumber)
        {
            // TODO - This will need updating if we add SMS support
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(managerUser, phoneNumber)
                .ConfigureAwait(false);
            var phoneResult = await _userManager.ChangePhoneNumberAsync(managerUser, phoneNumber, code)
                .ConfigureAwait(false);
            if (phoneResult.Succeeded)
            {
                handlerResult.Success = true;
                handlerResult.AddMessage("Phone number updated", ResultMessageType.Success);
            }
            else
            {
                handlerResult.Success = phoneResult.Succeeded;
                handlerResult.AddMessage(phoneResult.ToErrorsList(), ResultMessageType.Error);
                phoneResult.LogErrors(_logger);
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

        return handlerResult;
    }
}