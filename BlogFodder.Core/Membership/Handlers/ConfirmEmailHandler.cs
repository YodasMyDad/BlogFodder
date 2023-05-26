using System.Text;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Membership.Commands;
using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BlogFodder.Core.Membership.Handlers
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, AuthenticationResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ConfirmEmailHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthenticationResult { Success = true };
            if (request.UserId.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.AddMessage("The user id is null", ResultMessageType.Error);
                return result;
            }
            
            var user = await _userManager.FindByIdAsync(request.UserId).ConfigureAwait(false);
            if (user == null)
            {
                result.Success = false;
                result.AddMessage($"Unable to find a user with the id '{request.UserId}'.", ResultMessageType.Error);
                return result;
            }

            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            if (request.IsEmailUpdate)
            {
                // Get the new email from the extended data
                var newEmail = user!.ExtendedData.Get(Constants.ExtendedDataKeys.NewEmailAddress);
                if (!newEmail.IsNullOrWhiteSpace())
                {
                    var changeResult = await _userManager.ChangeEmailAsync(user, newEmail, request.Code).ConfigureAwait(false);
                    if (!changeResult.Succeeded)
                    {
                        result.Success = false;
                        changeResult.LogErrors();
                        result.AddMessage(changeResult.ToErrorsList(), ResultMessageType.Error);
                        return result;
                    }

                    // lear new email from user
                    user.ExtendedData.Remove(Constants.ExtendedDataKeys.NewEmailAddress);
                    var updateResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    if (!updateResult.Succeeded)
                    {
                        updateResult.LogErrors();
                        return result;
                    }

                    await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);

                    // return success message
                    result.AddMessage("Email address changed", ResultMessageType.Success);
                }
                else
                {
                    // error unable to get new email address
                    result.Success = false;
                    result.AddMessage("Unable to get users new email address", ResultMessageType.Error);
                    return result;
                }
            }
            else
            {
                var confirmResult = await _userManager.ConfirmEmailAsync(user!, request.Code).ConfigureAwait(false);
                if (confirmResult.Succeeded)
                {
                    result.AddMessage("Email confirmed, you can now login", ResultMessageType.Success);
                }
                else
                {
                    result.Success = false;
                    result.AddMessage("There was an error confirming your email", ResultMessageType.Error);
                }
            }
            return result;
        }
    }
}